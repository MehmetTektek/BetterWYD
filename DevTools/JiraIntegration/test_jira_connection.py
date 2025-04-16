#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Simple test script to check Jira API connection
"""

import os
from pathlib import Path
import sys

# Get the script directory
SCRIPT_DIR = Path(os.path.dirname(os.path.abspath(__file__)))

# Add the JiraIntegration directory to the Python path
sys.path.append(str(SCRIPT_DIR))

try:
    from jira_integration import JiraAPI, configure_urllib3
    print("Successfully imported JiraAPI")
except Exception as e:
    print(f"Error importing JiraAPI: {e}")
    sys.exit(1)

def test_jira_connection():
    """Test connection to Jira API"""
    print("Starting Jira connection test...")
    
    # Configure urllib3 to suppress LibreSSL warnings
    configure_urllib3()
    
    # Check if .env file exists
    env_path = SCRIPT_DIR / '.env'
    if not os.path.exists(env_path):
        print(f"Error: .env file not found at {env_path}")
        print("Please create a .env file with your Jira credentials.")
        return
    else:
        print(f"Found .env file at {env_path}")
    
    # Print environment variables for debugging (without showing the actual API token)
    jira_email = os.getenv("JIRA_EMAIL")
    jira_token = os.getenv("JIRA_API_TOKEN")
    jira_url = os.getenv("JIRA_URL")
    jira_project = os.getenv("JIRA_PROJECT_KEY")
    
    print(f"JIRA_EMAIL: {jira_email if jira_email else 'Not set'}")
    print(f"JIRA_API_TOKEN: {'*****' if jira_token else 'Not set'}")
    print(f"JIRA_URL: {jira_url if jira_url else 'Not set'}")
    print(f"JIRA_PROJECT_KEY: {jira_project if jira_project else 'Not set'}")
    
    # Create Jira API instance
    try:
        print("Initializing Jira API...")
        jira = JiraAPI()
        print("Jira API initialized successfully")
    except ValueError as e:
        print(f"Error initializing Jira API: {str(e)}")
        return
    except Exception as e:
        print(f"Unexpected error initializing Jira API: {str(e)}")
        import traceback
        traceback.print_exc()
        return
    
    # Test connection
    print("Testing connection to Jira...")
    if jira.test_connection():
        print("Successfully connected to Jira!")
        
        # Get and display 5 recent issues
        print("\nFetching 5 recent issues to verify project access:")
        issues = jira.get_project_issues(max_results=5)
        if issues:
            for issue in issues:
                print(f"- {issue['key']}: {issue['fields']['summary']}")
        else:
            print("No issues found in the project.")
    else:
        print("Failed to connect to Jira. Please check your credentials.")

if __name__ == "__main__":
    test_jira_connection()