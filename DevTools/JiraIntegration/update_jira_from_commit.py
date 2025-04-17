#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
BetterWYD Jira Update From Git Commit

This script is used by the post-commit Git hook to update Jira tickets
mentioned in commit messages. It extracts Jira ticket IDs from commit messages,
adds the commit as a comment to the Jira ticket, and can transition issues
to a different status when specific keywords are used.

Usage:
This script is meant to be called from a Git hook, but can also be run manually:
python update_jira_from_commit.py [commit_hash]

If no commit hash is provided, it will use the most recent commit.

Commit message format:
- To reference a ticket: "BWYD-123: Add new feature"
- To transition a ticket: "BWYD-123 #done: Fix critical bug"
  (supported transitions: #inprogress, #review, #done)

"""

import os
import sys
import re
import subprocess
from pathlib import Path
from typing import Optional, Tuple, List, Dict, Any
from datetime import datetime

# Add the parent directory to sys.path so we can import the JiraAPI class
SCRIPT_DIR = Path(os.path.dirname(os.path.abspath(__file__)))
sys.path.append(str(SCRIPT_DIR))

from jira_integration import JiraAPI

# Regex to match Jira ticket IDs (e.g., BWYD-123)
JIRA_TICKET_PATTERN = r'([A-Z]+-\d+)'

# Regex to match transition commands (e.g., #done)
TRANSITION_PATTERN = r'#(\w+)'

# Mapping of command keywords to transition names
# These should match your actual Jira workflow transitions
TRANSITION_MAPPING = {
    'inprogress': 'In Progress',
    'review': 'In Review',
    'done': 'Done',
    'resolved': 'Done',
    'fixed': 'Done',
    'complete': 'Done',
    'completed': 'Done',
    'close': 'Done',
    'closed': 'Done'
}

def get_git_commit_info(commit_hash: Optional[str] = None) -> Dict[str, str]:
    """
    Get commit information from Git
    
    Args:
        commit_hash: The hash of the commit to get info for. If None, use the latest commit.
        
    Returns:
        Dictionary with commit information (hash, author, date, message)
    """
    # If no commit hash is provided, use HEAD
    if not commit_hash:
        commit_hash = "HEAD"
    
    # Get commit info
    try:
        # Get the full commit message
        message = subprocess.check_output(
            ['git', 'log', '--format=%B', '-n', '1', commit_hash],
            universal_newlines=True
        ).strip()
        
        # Get other commit details
        commit_format = {
            'hash': '%H',
            'short_hash': '%h',
            'author_name': '%an',
            'author_email': '%ae',
            'date': '%ad',
            'subject': '%s'
        }
        
        commit_info = {}
        for key, format_str in commit_format.items():
            commit_info[key] = subprocess.check_output(
                ['git', 'log', f'--format={format_str}', '-n', '1', commit_hash],
                universal_newlines=True
            ).strip()
        
        # Add the full message
        commit_info['message'] = message
        
        return commit_info
    except subprocess.CalledProcessError as e:
        print(f"Error getting commit info: {e}")
        sys.exit(1)

def extract_jira_info(commit_message: str) -> List[Dict[str, Any]]:
    """
    Extract Jira ticket IDs and transition commands from a commit message
    
    Args:
        commit_message: The Git commit message
        
    Returns:
        List of dictionaries with ticket IDs and transition commands
    """
    # Find all Jira ticket IDs in the commit message
    ticket_matches = re.finditer(JIRA_TICKET_PATTERN, commit_message)
    
    results = []
    for match in ticket_matches:
        ticket_id = match.group(1)
        
        # Find transition command that might be associated with this ticket ID
        # Look for #command in the same line as the ticket ID
        line_with_ticket = next(
            (line for line in commit_message.splitlines() if ticket_id in line),
            ""
        )
        
        transition_match = re.search(TRANSITION_PATTERN, line_with_ticket)
        transition_command = transition_match.group(1).lower() if transition_match else None
        
        # Map the transition command to an actual transition name
        transition_name = None
        if transition_command and transition_command in TRANSITION_MAPPING:
            transition_name = TRANSITION_MAPPING[transition_command]
        
        results.append({
            'ticket_id': ticket_id,
            'transition_command': transition_command,
            'transition_name': transition_name
        })
    
    return results

def format_commit_comment(commit_info: Dict[str, str]) -> str:
    """
    Format the commit information as a comment for Jira
    
    Args:
        commit_info: Dictionary with commit information
        
    Returns:
        Formatted comment text
    """
    # Format the Git commit as a Jira comment
    comment = f"""
Git commit referencing this issue:

*Commit:* {commit_info['hash']} 
*Author:* {commit_info['author_name']} <{commit_info['author_email']}>
*Date:* {commit_info['date']}

*Message:*
{commit_info['message']}

This comment was automatically added by the BetterWYD Git-Jira integration.
"""
    return comment

def update_jira_issues(commit_hash: Optional[str] = None):
    """
    Main function to update Jira issues from a Git commit
    
    Args:
        commit_hash: The hash of the commit to process. If None, use the latest commit.
    """
    # Initialize Jira API
    try:
        jira = JiraAPI()
    except ValueError as e:
        print(f"Error initializing Jira API: {e}")
        sys.exit(1)
    
    # Test the connection to Jira
    if not jira.test_connection():
        print("Failed to connect to Jira. Please check your credentials.")
        sys.exit(1)
    
    # Get commit info
    commit_info = get_git_commit_info(commit_hash)
    print(f"Processing commit: {commit_info['short_hash']} - {commit_info['subject']}")
    
    # Extract Jira ticket IDs and transition commands
    jira_info = extract_jira_info(commit_info['message'])
    
    if not jira_info:
        print("No Jira ticket IDs found in the commit message. Nothing to update.")
        sys.exit(0)
    
    # Format the comment to add to Jira
    comment_text = format_commit_comment(commit_info)
    
    # Process each ticket
    for issue_info in jira_info:
        ticket_id = issue_info['ticket_id']
        transition_name = issue_info['transition_name']
        
        print(f"Processing Jira ticket: {ticket_id}")
        
        # Add the commit as a comment
        comment_result = jira.add_comment(ticket_id, comment_text)
        
        if comment_result:
            print(f"Added commit information as a comment to {ticket_id}")
        else:
            print(f"Failed to add comment to {ticket_id}")
        
        # If a transition was requested, perform it
        if transition_name:
            print(f"Attempting to transition {ticket_id} to '{transition_name}'")
            
            # Get available transitions
            transitions = jira.get_transitions(ticket_id)
            
            # Find the transition ID
            transition_id = next(
                (t['id'] for t in transitions if t['name'].lower() == transition_name.lower()),
                None
            )
            
            if transition_id:
                result = jira.transition_issue(ticket_id, transition_id)
                if result:
                    print(f"Successfully transitioned {ticket_id} to '{transition_name}'")
                else:
                    print(f"Failed to transition {ticket_id}")
            else:
                print(f"Transition '{transition_name}' not available for {ticket_id}")
                print("Available transitions:", ", ".join(t['name'] for t in transitions))

if __name__ == "__main__":
    # Get commit hash from command line if provided
    commit_hash = sys.argv[1] if len(sys.argv) > 1 else None
    update_jira_issues(commit_hash)