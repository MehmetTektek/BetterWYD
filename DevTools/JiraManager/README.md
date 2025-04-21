# JiraManager API for BetterWYD

## Overview

The JiraManager API provides a seamless integration between the BetterWYD Unity project and Jira project management system. It allows developers to create, update, and track Jira issues directly from within the Unity editor or during gameplay, streamlining the development workflow.

## Features

- **Direct Integration with Jira**: Connect to your Jira instance using the Atlassian REST API v3
- **Issue Management**: Create, update, comment on, and transition issues
- **Unity Integration**: Complete Unity MonoBehaviour component for easy integration
- **Progress Reporting**: Generate reports on project progress and task completion
- **Async Operations**: All API operations run asynchronously to avoid blocking the main thread

## Setup

### Prerequisites

- Jira account with API token access
- Unity 2020.3 or later
- .NET Standard 2.0 compatible project

### Installation

1. Add the JiraManager API to your project:
   - Copy the `JiraManager` folder to your Unity project's `Assets/Plugins/DevTools` directory
   - OR add this repository as a git submodule to your project

2. Create a Jira API token:
   - Go to [https://id.atlassian.com/manage-profile/security/api-tokens](https://id.atlassian.com/manage-profile/security/api-tokens)
   - Click "Create API token" and save it securely

3. Configure the JiraManagerUnity component:
   - Create an empty GameObject in your scene
   - Add the `JiraManagerUnity` component
   - Fill in your Jira email, API token, Jira URL, and project key

## Usage

### Basic Setup in Unity

```csharp
// You can access the JiraManager from anywhere in your code using the singleton:
JiraManagerUnity.Instance.TestConnection();
```

### Creating Issues

```csharp
// Create a new issue
JiraManagerUnity.Instance.CreateIssue(
    "Bug: Player falls through floor", 
    "When the player jumps on the platform, they sometimes fall through it.",
    "Bug",
    (issue) => {
        if (issue != null) {
            Debug.Log($"Created issue: {issue["key"]}");
        }
    }
);
```

### Adding Comments

```csharp
// Add a comment to an existing issue
JiraManagerUnity.Instance.AddComment(
    "BWYD-123", 
    "This issue is reproducible in the latest build.",
    (success) => {
        Debug.Log(success ? "Comment added" : "Failed to add comment");
    }
);
```

### Transitioning Issues

```csharp
// Transition an issue to a new status
JiraManagerUnity.Instance.TransitionIssue(
    "BWYD-123", 
    "31", // Transition ID (use GetTransitions to find available transitions)
    (success) => {
        Debug.Log(success ? "Issue transitioned" : "Failed to transition issue");
    }
);
```

### Generating Reports

```csharp
// Generate a progress report for the project
JiraManagerUnity.Instance.GenerateProgressReport(
    (report) => {
        if (report != null) {
            Debug.Log($"Project is {report["completion_percentage"]}% complete");
            Debug.Log($"Total issues: {report["total_issues"]}");
        }
    }
);
```

## Security Considerations

- **API Token Security**: Do not commit your Jira API token to version control
- **Runtime Usage**: Consider if you need Jira integration in builds or only in the Unity Editor
- **Access Control**: Ensure your Jira API token has appropriate permissions

## Architecture

The JiraManager consists of two main components:

1. **JiraManagerAPI**: Core API class that handles communication with the Jira REST API
2. **JiraManagerUnity**: Unity MonoBehaviour that wraps the API for easy use in Unity

## Build and Deploy

The JiraManager API is built as a .NET Standard 2.0 library, making it compatible with Unity's scripting backend. The build output is automatically copied to your Unity project's `Assets/Plugins/DevTools/JiraManager` directory.

## Contributing

1. Create a feature branch following the naming convention: `feature/your-feature-name`
2. Make your changes following the BetterWYD coding standards
3. Submit a pull request for review

## License

This code is proprietary and for use only within the BetterWYD project.

---

Created by the BetterWYD team on April 21, 2025.