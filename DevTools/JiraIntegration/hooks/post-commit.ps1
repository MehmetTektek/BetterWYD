#!/usr/bin/env pwsh

# BetterWYD - Git post-commit hook to update Jira tickets
# 
# This hook will run after each commit and update Jira tickets mentioned in the commit message.
# Features:
# - Adds a comment with commit details to referenced Jira tickets
# - Can transition tickets to different states using hashtags (#inprogress, #review, #done)
#
# Example commit messages:
# - "BWYD-123: Add new feature"  --> Just adds a comment to BWYD-123
# - "BWYD-123 #done: Fix bug"   --> Adds comment and transitions BWYD-123 to Done status

# Get the repository root directory
$repoRoot = git rev-parse --show-toplevel

# Path to the Jira integration script
$jiraScript = Join-Path $repoRoot "DevTools\JiraIntegration\update_jira_from_commit.py"

# Check if the script exists
if (-not (Test-Path $jiraScript)) {
    Write-Error "Error: Jira integration script not found at $jiraScript"
    exit 1
}

# Get the latest commit hash
$commitHash = git rev-parse HEAD

# Run the script
Write-Host "Updating Jira from commit $commitHash..."
python $jiraScript $commitHash

# Exit with the same status as the script
exit $LASTEXITCODE