#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
BetterWYD Jira Integration

This script provides integration with Jira for the BetterWYD project using
the Jira REST API v3 with API token authentication.

Features:
- Fetch issues from Jira project
- Create new issues
- Update existing issues
- Add comments to issues
- Transition issues between statuses
- Generate reports on project progress

Usage:
1. Set up your Jira credentials in a .env file (see .env.example)
2. Run specific functions as needed for your workflow

"""

import os
import json
import base64
import requests
import sys
from datetime import datetime
from typing import Dict, List, Any, Optional
from pathlib import Path
from dotenv import load_dotenv

# Get the script directory for proper file path handling
SCRIPT_DIR = Path(os.path.dirname(os.path.abspath(__file__)))

# Load environment variables from .env file
env_path = SCRIPT_DIR / '.env'
load_dotenv(dotenv_path=env_path)

class JiraAPI:
    """Class to handle all Jira API interactions"""
    
    def __init__(self):
        """Initialize the Jira API with credentials from environment variables"""
        self.jira_email = os.getenv("JIRA_EMAIL")
        self.api_token = os.getenv("JIRA_API_TOKEN")
        self.jira_url = os.getenv("JIRA_URL")
        self.project_key = os.getenv("JIRA_PROJECT_KEY")
        
        if not all([self.jira_email, self.api_token, self.jira_url, self.project_key]):
            raise ValueError("Missing required environment variables. "
                             "Please set JIRA_EMAIL, JIRA_API_TOKEN, JIRA_URL, and JIRA_PROJECT_KEY.")
        
        # Authentication header using API token
        auth_str = f"{self.jira_email}:{self.api_token}"
        self.auth_header = base64.b64encode(auth_str.encode()).decode()
        
        # Default headers for all requests
        self.headers = {
            "Authorization": f"Basic {self.auth_header}",
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    
    def test_connection(self) -> bool:
        """Test the connection to the Jira API"""
        try:
            response = requests.get(
                f"{self.jira_url}/rest/api/3/myself",
                headers=self.headers
            )
            
            if response.status_code == 200:
                user_data = response.json()
                print(f"Successfully connected to Jira as {user_data.get('displayName', 'Unknown User')}")
                return True
            else:
                print(f"Failed to connect to Jira: {response.status_code}")
                print(response.text)
                return False
        except Exception as e:
            print(f"Error connecting to Jira: {str(e)}")
            return False
    
    def get_project_issues(self, max_results: int = 50) -> List[Dict[str, Any]]:
        """
        Get issues from the project
        
        Args:
            max_results: Maximum number of results to return
            
        Returns:
            List of issue dictionaries
        """
        try:
            url = f"{self.jira_url}/rest/api/3/search"
            jql_query = f"project = {self.project_key} ORDER BY created DESC"
            
            payload = {
                "jql": jql_query,
                "maxResults": max_results,
                "fields": ["summary", "description", "status", "assignee", "priority", "issuetype", "created", "updated"]
            }
            
            response = requests.post(
                url,
                headers=self.headers,
                json=payload
            )
            
            if response.status_code == 200:
                data = response.json()
                return data.get("issues", [])
            else:
                print(f"Error fetching issues: {response.status_code}")
                print(response.text)
                return []
        except Exception as e:
            print(f"Exception when fetching issues: {str(e)}")
            return []
    
    def create_issue(self, summary: str, description: str, issue_type=None, 
                     parent_key: Optional[str] = None, priority: Optional[str] = None, 
                     assignee: Optional[str] = None) -> Optional[Dict[str, Any]]:
        """
        Create a new issue in the Jira project
        
        Args:
            summary: Issue summary/title
            description: Detailed description
            issue_type: Type of issue (ID or dict with type info)
            parent_key: Parent issue key (required for sub-tasks)
            priority: Priority level (optional)
            assignee: Account ID of assignee (optional)
            
        Returns:
            Issue data if successful, None otherwise
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issue"
            
            # Convert description to Jira's ADFV3 format
            description_adf = {
                "type": "doc",
                "version": 1,
                "content": [
                    {
                        "type": "paragraph",
                        "content": [
                            {
                                "type": "text",
                                "text": description
                            }
                        ]
                    }
                ]
            }
            
            # Prepare the payload
            payload = {
                "fields": {
                    "project": {
                        "key": self.project_key
                    },
                    "summary": summary,
                    "description": description_adf
                }
            }
            
            # Handle different formats of issue_type
            if isinstance(issue_type, dict):
                # If issue_type is a dict with ID and subtask info
                issue_type_id = issue_type.get('id')
                is_subtask = issue_type.get('subtask', False)
                
                if issue_type_id:
                    payload["fields"]["issuetype"] = {"id": issue_type_id}
                    
                    # If this is a sub-task, parent is required
                    if is_subtask and parent_key:
                        payload["fields"]["parent"] = {"key": parent_key}
                    elif is_subtask and not parent_key:
                        raise ValueError("Parent key is required for sub-task issue types")
            elif issue_type:
                # If issue_type is a string (ID or name)
                if isinstance(issue_type, str) and issue_type.isdigit():
                    # If it's an ID, use the ID directly
                    payload["fields"]["issuetype"] = {"id": issue_type}
                else:
                    # If it's a name, use the name
                    payload["fields"]["issuetype"] = {"name": issue_type}
                
                # If parent_key is provided, assume this is a sub-task
                if parent_key:
                    payload["fields"]["parent"] = {"key": parent_key}
            
            # Add priority if provided
            if priority:
                payload["fields"]["priority"] = {"name": priority}
            
            # Add assignee if provided
            if assignee:
                payload["fields"]["assignee"] = {"id": assignee}
            
            response = requests.post(
                url,
                headers=self.headers,
                json=payload
            )
            
            if response.status_code in [200, 201]:
                data = response.json()
                print(f"Successfully created issue: {data.get('key')}")
                return data
            else:
                print(f"Error creating issue: {response.status_code}")
                print(response.text)
                return None
        except ValueError as e:
            print(f"Validation error when creating issue: {str(e)}")
            return None
        except Exception as e:
            print(f"Exception when creating issue: {str(e)}")
            return None
    
    def update_issue(self, issue_key: str, fields_to_update: Dict[str, Any]) -> bool:
        """
        Update an existing issue
        
        Args:
            issue_key: The key of the issue to update (e.g., 'BWYD-123')
            fields_to_update: Dictionary of field keys and values to update
            
        Returns:
            True if successful, False otherwise
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issue/{issue_key}"
            
            # Build the payload based on provided fields
            payload = {"fields": {}}
            
            for field, value in fields_to_update.items():
                payload["fields"][field] = value
            
            response = requests.put(
                url,
                headers=self.headers,
                json=payload
            )
            
            if response.status_code in [200, 204]:
                print(f"Successfully updated issue: {issue_key}")
                return True
            else:
                print(f"Error updating issue: {response.status_code}")
                print(response.text)
                return False
        except Exception as e:
            print(f"Exception when updating issue: {str(e)}")
            return False
    
    def add_comment(self, issue_key: str, comment_text: str) -> Optional[Dict[str, Any]]:
        """
        Add a comment to an issue
        
        Args:
            issue_key: The key of the issue (e.g., 'BWYD-123')
            comment_text: The text of the comment
            
        Returns:
            Comment data if successful, None otherwise
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issue/{issue_key}/comment"
            
            # Convert comment to Jira's ADFV3 format
            comment_adf = {
                "body": {
                    "type": "doc",
                    "version": 1,
                    "content": [
                        {
                            "type": "paragraph",
                            "content": [
                                {
                                    "type": "text",
                                    "text": comment_text
                                }
                            ]
                        }
                    ]
                }
            }
            
            response = requests.post(
                url,
                headers=self.headers,
                json=comment_adf
            )
            
            if response.status_code in [200, 201]:
                data = response.json()
                print(f"Successfully added comment to issue: {issue_key}")
                return data
            else:
                print(f"Error adding comment: {response.status_code}")
                print(response.text)
                return None
        except Exception as e:
            print(f"Exception when adding comment: {str(e)}")
            return None
    
    def get_transitions(self, issue_key: str) -> List[Dict[str, Any]]:
        """
        Get available transitions for an issue
        
        Args:
            issue_key: The key of the issue (e.g., 'BWYD-123')
            
        Returns:
            List of available transitions
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issue/{issue_key}/transitions"
            
            response = requests.get(
                url,
                headers=self.headers
            )
            
            if response.status_code == 200:
                data = response.json()
                return data.get("transitions", [])
            else:
                print(f"Error getting transitions: {response.status_code}")
                print(response.text)
                return []
        except Exception as e:
            print(f"Exception when getting transitions: {str(e)}")
            return []
    
    def transition_issue(self, issue_key: str, transition_id: str) -> bool:
        """
        Transition an issue to a new status
        
        Args:
            issue_key: The key of the issue (e.g., 'BWYD-123')
            transition_id: The ID of the transition
            
        Returns:
            True if successful, False otherwise
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issue/{issue_key}/transitions"
            
            payload = {
                "transition": {
                    "id": transition_id
                }
            }
            
            response = requests.post(
                url,
                headers=self.headers,
                json=payload
            )
            
            if response.status_code in [200, 204]:
                print(f"Successfully transitioned issue: {issue_key}")
                return True
            else:
                print(f"Error transitioning issue: {response.status_code}")
                print(response.text)
                return False
        except Exception as e:
            print(f"Exception when transitioning issue: {str(e)}")
            return False
    
    def generate_progress_report(self) -> Dict[str, Any]:
        """
        Generate a report on project progress
        
        Returns:
            Dictionary with project statistics
        """
        try:
            # Get project issues
            issues = self.get_project_issues(max_results=1000)
            
            # Initialize counters
            total_issues = len(issues)
            status_counts = {}
            issue_type_counts = {}
            
            # Calculate metrics
            for issue in issues:
                # Count by status
                status = issue["fields"]["status"]["name"]
                status_counts[status] = status_counts.get(status, 0) + 1
                
                # Count by issue type
                issue_type = issue["fields"]["issuetype"]["name"]
                issue_type_counts[issue_type] = issue_type_counts.get(issue_type, 0) + 1
            
            # Calculate completion percentage (if "Done" status exists)
            completion_percentage = 0
            if "Done" in status_counts and total_issues > 0:
                completion_percentage = (status_counts["Done"] / total_issues) * 100
            
            # Create report
            report = {
                "timestamp": datetime.now().isoformat(),
                "project_key": self.project_key,
                "total_issues": total_issues,
                "status_breakdown": status_counts,
                "issue_type_breakdown": issue_type_counts,
                "completion_percentage": completion_percentage
            }
            
            return report
        except Exception as e:
            print(f"Exception when generating report: {str(e)}")
            return {"error": str(e)}

    def get_issue_types(self) -> List[Dict[str, Any]]:
        """
        Get all available issue types in the Jira instance
        
        Returns:
            List of available issue types
        """
        try:
            url = f"{self.jira_url}/rest/api/3/issuetype"
            
            response = requests.get(
                url,
                headers=self.headers
            )
            
            if response.status_code == 200:
                data = response.json()
                print(f"Found {len(data)} issue types")
                return data
            else:
                print(f"Error fetching issue types: {response.status_code}")
                print(response.text)
                return []
        except Exception as e:
            print(f"Exception when fetching issue types: {str(e)}")
            return []


def create_env_file():
    """Create .env.example file with required environment variables"""
    env_content = """# Jira API Configuration
JIRA_EMAIL=your-email@example.com
JIRA_API_TOKEN=your-api-token
JIRA_URL=https://your-domain.atlassian.net
JIRA_PROJECT_KEY=BWYD

# Note: Generate an API token from your Atlassian account:
# https://id.atlassian.com/manage-profile/security/api-tokens
"""
    
    env_example_path = SCRIPT_DIR / '.env.example'
    with open(env_example_path, "w") as f:
        f.write(env_content)
    
    print(f"Created .env.example file at {env_example_path}")
    print("Create a copy named .env and fill in your actual credentials.")


def configure_urllib3():
    """Configure urllib3 to work with LibreSSL"""
    try:
        # Attempt to fix the LibreSSL warning
        import urllib3
        urllib3.disable_warnings(urllib3.exceptions.NotOpenSSLWarning)
        
        # For older versions of urllib3
        try:
            from urllib3.exceptions import InsecureRequestWarning
            urllib3.disable_warnings(InsecureRequestWarning)
        except ImportError:
            pass
        
        print("Configured urllib3 to suppress SSL warnings")
    except Exception as e:
        print(f"Note: Could not configure urllib3 warnings: {e}")


def main():
    """Main function to demonstrate usage"""
    # Configure urllib3 to suppress LibreSSL warnings
    configure_urllib3()
    
    # Check if .env.example exists, create if not
    env_example_path = SCRIPT_DIR / '.env.example'
    if not os.path.exists(env_example_path):
        create_env_file()
        print("Please create a .env file with your credentials before continuing.")
        return
    
    # Check if .env file exists
    env_path = SCRIPT_DIR / '.env'
    if not os.path.exists(env_path):
        print(f"Please create a .env file at {env_path} with your credentials.")
        print(f"You can copy the template from {env_example_path}")
        return
    
    # Create Jira API instance
    try:
        jira = JiraAPI()
    except ValueError as e:
        print(f"Error: {str(e)}")
        return
    
    # Test connection
    if not jira.test_connection():
        print("Failed to connect to Jira. Please check your credentials.")
        return
    
    # Example: List recent issues
    print("\n==== Recent Issues ====")
    issues = jira.get_project_issues(max_results=5)
    for issue in issues:
        print(f"{issue['key']}: {issue['fields']['summary']} - {issue['fields']['status']['name']}")
    
    # Example: Generate progress report
    print("\n==== Project Progress Report ====")
    report = jira.generate_progress_report()
    print(json.dumps(report, indent=2))


if __name__ == "__main__":
    main()