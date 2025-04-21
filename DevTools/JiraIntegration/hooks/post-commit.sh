#!/bin/sh

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
repo_root=$(git rev-parse --show-toplevel)

# Path to the Jira integration script
jira_script="$repo_root/DevTools/JiraIntegration/update_jira_from_commit.py"

# Check if the script exists
if [ ! -f "$jira_script" ]; then
    echo "Error: Jira integration script not found at $jira_script"
    exit 1
fi

# Get the latest commit hash
commit_hash=$(git rev-parse HEAD)

# Run the script
echo "Updating Jira from commit $commit_hash..."
python "$jira_script" "$commit_hash"

# Exit with the same status as the script
exit $?