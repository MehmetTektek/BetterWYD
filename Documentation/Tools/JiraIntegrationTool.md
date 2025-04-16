,# Jira Integration Tool

## Overview

The Jira Integration Tool provides a bridge between the BetterWYD Unity project and Jira project management, allowing the development team to manage tasks, track progress, and maintain project visibility through the Jira REST API v3.

## Purpose

This tool streamlines the development process by enabling:
- Programmatic creation of Jira issues based on the Development Roadmap
- Progress tracking of development tasks
- Automated status reporting
- Simplified team communication through task management

## Directory Structure

```
DevTools/
  └── JiraIntegration/
      ├── jira_integration.py    # Core API class for Jira integration
      ├── setup_jira_project.py  # Script to populate Jira with project roadmap
      ├── requirements.txt       # Python dependencies
      ├── .env.example           # Template for credentials
      └── README.md              # Usage instructions
```

## Setup Instructions

### Prerequisites

- Python 3.6 or higher
- Jira Cloud account with API token
- Existing Jira project

### Installation

1. Create a Python virtual environment:
   ```
   cd /Users/mehmet.tektek/BetterWYD
   python3 -m venv jira_venv
   ```

2. Activate the virtual environment:
   ```
   source jira_venv/bin/activate
   ```

3. Install required dependencies:
   ```
   cd DevTools/JiraIntegration
   pip install -r requirements.txt
   ```

4. Configure your Jira credentials:
   ```
   cp .env.example .env
   ```

5. Edit the `.env` file with your specific credentials:
   ```
   JIRA_EMAIL=your-email@example.com
   JIRA_API_TOKEN=your-api-token
   JIRA_URL=https://your-domain.atlassian.net
   JIRA_PROJECT_KEY=BWYD
   ```

## Core Features

### JiraAPI Class

The `JiraAPI` class provides methods for interacting with Jira:

- **test_connection()**: Verify connectivity to Jira
- **create_issue()**: Create a new Jira issue
- **get_project_issues()**: Retrieve issues from the project
- **update_issue()**: Update fields on an existing issue
- **add_comment()**: Add comments to an issue
- **transition_issue()**: Change the status of an issue
- **generate_progress_report()**: Create a summary of project progress

### Project Setup Script

The `setup_jira_project.py` script uses the BetterWYD Development Roadmap to create a structured set of Jira items:

- **Phase Epics**: Three main development phases (Foundation, Gameplay, Polishing)
- **Stories**: Major feature areas within each phase
- **Tasks**: Individual development tasks required to complete each story

## Usage

### Basic Connectivity Test

```python
from jira_integration import JiraAPI

jira = JiraAPI()
if jira.test_connection():
    print("Successfully connected to Jira")
```

### Creating Issues

```python
jira.create_issue(
    summary="Implement Character Movement", 
    description="Create a flexible character controller with keyboard/gamepad support",
    issue_type="Task"
)
```

### Generating Progress Reports

```python
report = jira.generate_progress_report()
print(f"Project progress: {report['completion_percentage']:.1f}%")
```

### Populating Project with Roadmap

To initialize your Jira project with all items from the Development Roadmap:

```
cd /Users/mehmet.tektek/BetterWYD
source jira_venv/bin/activate
python DevTools/JiraIntegration/setup_jira_project.py
```

## Development Timeline Integration

The tool automatically creates Jira items with the following timeline structure:

- **Phase 1: Foundation & Core Systems** (April 15 - May 15, 2025)
  - Project Setup & Architecture
  - Core Game Systems Implementation

- **Phase 2: Gameplay Implementation** (May 16 - June 15, 2025)
  - Class System & Combat
  - World Building & Environment

- **Phase 3: Polishing & Testing** (June 16 - July 15, 2025)
  - Multiplayer Framework & Social Features
  - Testing & Optimization

## Technical Details

### API Authentication

The tool uses Jira API token authentication through Basic Auth headers:

```python
auth_str = f"{email}:{api_token}"
auth_header = base64.b64encode(auth_str.encode()).decode()
headers = {"Authorization": f"Basic {auth_header}"}
```

### Issue Field Format

Jira issue descriptions use the Atlassian Document Format (ADF):

```python
description_adf = {
    "type": "doc",
    "version": 1,
    "content": [
        {
            "type": "paragraph",
            "content": [
                {
                    "type": "text",
                    "text": "Description text here"
                }
            ]
        }
    ]
}
```

## Troubleshooting

### Connection Issues

If you encounter connection problems:
- Verify your API token is valid
- Check that your Jira URL is correct
- Ensure your account has appropriate permissions

### Error Messages

- **Field cannot be set**: The field is not available on your Jira screens
- **Specify an issue type**: The issue type doesn't exist or is misspelled
- **Could not find transition**: Status transition is not valid for the workflow

## Maintenance

### Adding New Issue Types

To add support for new issue types:

1. Verify the issue type exists in your Jira project
2. Use the exact name as it appears in Jira
3. Update the `issue_type` parameter when creating issues

### Extending Functionality

To add new features to the integration:

1. Add new methods to the `JiraAPI` class
2. Follow the existing error handling pattern
3. Update this documentation with the new capabilities

## Future Enhancements

- Integration with Unity Editor UI for in-editor task management
- Automatic issue creation from Unity errors/exceptions
- Two-way sync between Git commits and Jira issues
- Custom reports and dashboards for team visibility