# BetterWYD Jira Integration

This tool provides integration between the BetterWYD Unity project and Jira using the Jira REST API v3 with API token authentication.

## Features

- Fetch issues from Jira project
- Create new issues
- Update existing issues
- Add comments to issues
- Transition issues between statuses
- Generate reports on project progress

## Setup

1. Install the required Python dependencies:
   ```
   pip install -r requirements.txt
   ```

2. Create a `.env` file in this directory based on the `.env.example` template:
   ```
   cp .env.example .env
   ```

3. Edit the `.env` file with your Jira credentials:
   - JIRA_EMAIL: Your Atlassian account email
   - JIRA_API_TOKEN: Your Atlassian API token (generate at https://id.atlassian.com/manage-profile/security/api-tokens)
   - JIRA_URL: Your Jira instance URL (e.g., https://your-domain.atlassian.net)
   - JIRA_PROJECT_KEY: Your project key (e.g., BWYD)

## Usage

Run the script with Python 3:

```
python jira_integration.py
```

This will:
1. Test connection to your Jira instance
2. Fetch and display recent issues
3. Generate a project progress report

## Advanced Usage

You can import the JiraAPI class into your own scripts:

```python
from jira_integration import JiraAPI

# Initialize the API
jira = JiraAPI()

# Create a new issue
jira.create_issue(
    summary="Implement feature X",
    description="We need to implement feature X according to the spec.",
    issue_type="Story",
    priority="High"
)

# Get project issues
issues = jira.get_project_issues()
```

## Integration with Unity

This tool is designed to be used alongside your Unity development process. You can:

1. Generate reports on project progress
2. Automate issue creation for bugs found during testing
3. Update issue status as features are implemented
4. Link commits and pull requests to Jira issues

For CI/CD integration, consider using this script as part of automated pipelines to update issue status when building or deploying.