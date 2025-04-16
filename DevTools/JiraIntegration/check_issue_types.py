#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Jira Issue Types Diagnostic Tool

This script prints exactly what issue types are available in your Jira instance.
"""

import os
import sys
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

def print_issue_types():
    """Print all available issue types in the Jira instance"""
    print("Starting Jira issue types diagnostic...")
    
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
    
    print("Connected to Jira successfully. Fetching issue types...")
    
    # Fetch available issue types from the Jira API
    url = f"{jira.jira_url}/rest/api/3/issuetype"
    response = None
    
    try:
        import requests
        response = requests.get(url, headers=jira.headers)
    except Exception as e:
        print(f"Error making request: {str(e)}")
        return
    
    if not response or response.status_code != 200:
        print(f"Failed to get issue types. Status code: {response.status_code if response else 'None'}")
        if response:
            print(f"Response: {response.text}")
        return
    
    # Parse and print issue types
    try:
        issue_types = response.json()
        
        print("\n==== Available Issue Types in Jira ====")
        print(f"Found {len(issue_types)} issue types:")
        
        for issue_type in issue_types:
            type_id = issue_type.get('id', 'N/A')
            name = issue_type.get('name', 'Unknown')
            description = issue_type.get('description', 'No description')
            print(f"ID: {type_id} | Name: {name} | Description: {description}")
            print("-" * 80)
        
        print("\nTo use these in your script, update the EPIC_TYPE_ID, STORY_TYPE_ID, and TASK_TYPE_ID constants in setup_jira_project.py")
    except Exception as e:
        print(f"Error parsing response: {str(e)}")
        import traceback
        traceback.print_exc()

if __name__ == "__main__":
    print_issue_types()