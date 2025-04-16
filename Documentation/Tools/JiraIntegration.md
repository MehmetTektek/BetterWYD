# Jira Integration Tool Documentation

## Overview

The Jira Integration Tool is a Python-based utility for managing and tracking BetterWYD project tasks in Jira. It allows team members to interact with Jira directly from the development environment, streamlining project management and development workflows.

## Features

- Fetch issues from Jira project 
- Create new issues with detailed descriptions
- Update existing issues
- Add comments to issues
- Transition issues between statuses (e.g., "To Do" → "In Progress" → "Done")
- Generate project progress reports

## Requirements

- Python 3.6+
- `requests` package
- `python-dotenv` package
- Jira account with API token

## Setup

1. Install the required Python dependencies:
   ```
   cd DevTools/JiraIntegration
   pip install -r requirements.txt
   ```

2. Create a `.env` file in the `DevTools/JiraIntegration` directory with your Jira credentials:
   ```
   cp .env.example .env
   ```

3. Edit the `.env` file with your credentials:
   - `JIRA_EMAIL`: Your Atlassian account email
   - `JIRA_API_TOKEN`: Your API token from https://id.atlassian.com/manage-profile/security/api-tokens
   - `JIRA_URL`: Your Jira instance URL (e.g., https://your-domain.atlassian.net)
   - `JIRA_PROJECT_KEY`: Your project key (e.g., BWYD)

## Usage

### Running the Main Script

The main script provides a demonstration of the tool's capabilities:

```
cd DevTools/JiraIntegration
python jira_integration.py
```

This will:
1. Test the connection to your Jira instance
2. Display recent issues from your project
3. Generate a project progress report

### Using in Your Own Scripts

You can import the JiraAPI class into your own scripts for custom functionality:

```python
from jira_integration import JiraAPI

# Initialize the API
jira = JiraAPI()

# Create a new issue
issue = jira.create_issue(
    summary="Implement enemy AI behavior",
    description="Create a state machine for enemy AI with patrolling, chasing, and attacking states.",
    issue_type="Task",
    priority="Medium"
)

# Get all issues from the project
issues = jira.get_project_issues()

# Add a comment to an issue
jira.add_comment("BWYD-123", "Implemented the patrolling state with waypoint system.")

# Transition an issue to "In Progress"
transitions = jira.get_transitions("BWYD-123")
for transition in transitions:
    if transition["name"] == "In Progress":
        jira.transition_issue("BWYD-123", transition["id"])
```

## Integration with Unity Workflow

### Issue Tracking for Game Features

Use the tool to create and track issues for game features:

```python
jira.create_issue(
    summary="Player Movement System",
    description="Implement a smooth player movement system with acceleration and deceleration.",
    issue_type="Story",
    priority="High"
)
```

### Bug Reporting

When bugs are discovered during testing, they can be reported directly to Jira:

```python
jira.create_issue(
    summary="Player clipping through walls at high speeds",
    description="When the player moves at high speeds, they can clip through thin walls. Steps to reproduce: 1. Use speed boost powerup 2. Run directly into a thin wall.",
    issue_type="Bug",
    priority="Medium"
)
```

### Progress Reporting

Generate reports on project progress for team meetings:

```python
report = jira.generate_progress_report()
print(f"Project is {report['completion_percentage']:.1f}% complete")
print(f"Open issues: {report['total_issues'] - report['status_breakdown'].get('Done', 0)}")
```

## Best Practices

1. **Regular Updates**: Keep Jira issues updated to reflect the current state of development.
2. **Detailed Descriptions**: Provide detailed descriptions when creating issues.
3. **Link to Code**: Reference relevant code files or commits in issue comments.
4. **Consistent Issue Types**: Use consistent issue types across the project:
   - Story: A feature from a user's perspective
   - Task: A specific development task
   - Bug: A defect that needs to be fixed
   - Epic: A large body of work that can be broken down
5. **Prioritize**: Set appropriate priorities for issues to focus development efforts.

## Troubleshooting

### Connection Issues

If you encounter connection issues:
- Verify your API token is correct and has not expired
- Check that your Jira URL is correct
- Ensure you have appropriate permissions in the Jira project

### API Rate Limiting

Jira Cloud has API rate limits. If you hit these limits:
- Reduce the frequency of API calls
- Implement caching for frequently requested data
- Add retry logic with exponential backoff

### Missing Issues

If issues are not appearing in queries:
- Check your JQL query syntax
- Verify you have permission to view the issues
- Ensure the issues exist in the specified project

## Future Enhancements

- Integration with Unity Editor for in-editor task management
- Automated issue creation from Unity Console errors
- Visual reporting dashboard
- Two-way synchronization with Git commits

## References

- [Jira REST API Documentation](https://developer.atlassian.com/cloud/jira/platform/rest/v3/intro/)
- [Python Requests Library](https://docs.python-requests.org/)
- [Atlassian API Tokens](https://id.atlassian.com/manage-profile/security/api-tokens)