#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Jira Project Metadata Diagnostic Tool

This script checks what issue types can be created in your specific Jira project.
"""

import os
import sys
import json
from pathlib import Path

# Get the script directory
SCRIPT_DIR = Path(os.path.dirname(os.path.abspath(__file__)))

# Add the JiraIntegration directory to the Python path
sys.path.append(str(SCRIPT_DIR))

try:
    from jira_integration import JiraAPI, configure_urllib3
except Exception as e:
    print(f"Error importing JiraAPI: {e}")
    sys.exit(1)

def check_project_metadata():
    """Check project metadata including valid issue types for the project"""
    print("Starting Jira project metadata diagnostic...")
    
    # Configure urllib3 to suppress LibreSSL warnings
    configure_urllib3()
    
    # Create Jira API instance
    try:
        jira = JiraAPI()
    except ValueError as e:
        print(f"Error initializing Jira API: {str(e)}")
        return
    
    # Test connection
    if not jira.test_connection():
        print("Failed to connect to Jira. Please check your credentials.")
        return
    
    print("Connected to Jira successfully. Fetching project metadata...")
    
    # Fetch issue creation metadata for this specific project
    url = f"{jira.jira_url}/rest/api/3/issue/createmeta?projectKeys={jira.project_key}&expand=projects.issuetypes"
    response = None
    
    try:
        import requests
        response = requests.get(url, headers=jira.headers)
    except Exception as e:
        print(f"Error making request: {str(e)}")
        return
    
    if not response or response.status_code != 200:
        print(f"Failed to get project metadata. Status code: {response.status_code if response else 'None'}")
        if response:
            print(f"Response: {response.text}")
        return
    
    # Parse and print project metadata
    try:
        metadata = response.json()
        projects = metadata.get('projects', [])
        
        if not projects:
            print(f"No project found with key: {jira.project_key}")
            return
        
        project = projects[0]
        issue_types = project.get('issuetypes', [])
        
        print(f"\n==== Available Issue Types for Project {jira.project_key} ====")
        print(f"Found {len(issue_types)} issue types that can be created in this project:")
        
        for issue_type in issue_types:
            type_id = issue_type.get('id', 'N/A')
            name = issue_type.get('name', 'Unknown')
            description = issue_type.get('description', 'No description')
            print(f"ID: {type_id} | Name: {name}")
            print(f"Description: {description}")
            print("-" * 80)
        
        print("\nRecommended values for setup_jira_project.py:")
        
        # Find the Epic type
        epic_type = next((t for t in issue_types if t.get('name', '').lower() == 'epic'), None)
        if epic_type:
            print(f"EPIC_TYPE_ID = \"{epic_type.get('id')}\"  # {epic_type.get('name')}")
        else:
            print("Epic issue type not found!")
        
        # Find the Story type
        story_type = next((t for t in issue_types if t.get('name', '').lower() == 'story'), None)
        if story_type:
            print(f"STORY_TYPE_ID = \"{story_type.get('id')}\"  # {story_type.get('name')}")
        else:
            print("Story issue type not found!")
        
        # Find the Task type
        task_type = next((t for t in issue_types if t.get('name', '').lower() == 'task'), None)
        if task_type:
            print(f"TASK_TYPE_ID = \"{task_type.get('id')}\"  # {task_type.get('name')}")
        else:
            print("Task issue type not found!")
            
    except Exception as e:
        print(f"Error parsing response: {str(e)}")
        import traceback
        traceback.print_exc()

if __name__ == "__main__":
    check_project_metadata()