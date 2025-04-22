# BetterWYD Documentation Standards

## Overview

This document serves as the central reference for all documentation standards used in the BetterWYD project. It establishes consistent guidelines for documentation across all aspects of the project, ensuring clarity, maintainability, and cross-platform compatibility.

## Table of Contents

1. [Documentation Types](#documentation-types)
2. [Markdown Guidelines](#markdown-guidelines)
3. [Code Documentation](#code-documentation)
4. [Technical Design Documentation](#technical-design-documentation)
5. [User Documentation](#user-documentation)
6. [Version Control Documentation](#version-control-documentation)
7. [Cross-Platform Considerations](#cross-platform-considerations)
8. [Review Process](#review-process)
9. [References](#references)

## Documentation Types

The BetterWYD project maintains several types of documentation:

| Type | Purpose | Location |
|------|---------|----------|
| Code Documentation | API, class, and method documentation | Inline comments and `/Documentation/Coding/` |
| Technical Design | System architecture and implementation details | `/Documentation/TechnicalDesignDocument.md` and `/Documentation/Architecture/` |
| Development Guidelines | Standards and best practices | `/Documentation/Guidelines/` |
| Development Roadmap | Project timeline and milestones | `/Documentation/DevelopmentRoadmap.md` |
| User Documentation | End-user instructions and help | Future: `/Documentation/UserGuide/` |

## Markdown Guidelines

All markdown files in the project should follow our established guidelines:

- Follow the [Markdown Style Guide](/Documentation/Guidelines/MarkdownStyleGuide.md)
- Use the project's `.markdownlint.json` configuration for formatting validation
- Line length should not exceed 100 characters
- Use ATX-style headers with a space after the hash marks (`# Header` not `#Header`)
- Include a single top-level heading as the document title
- Use reference-style links for better readability
- Specify the language for code blocks (e.g., ```csharp)

## Code Documentation

### C# Documentation

- Use XML documentation comments for public APIs:
  ```csharp
  /// <summary>
  /// Provides a brief description of the class or method.
  /// </summary>
  /// <param name="paramName">Description of the parameter.</param>
  /// <returns>Description of the return value.</returns>
  ```
- Include documentation for:
  - Classes and interfaces
  - Public and protected methods
  - Public properties and fields
  - Enum values
  - Complex algorithms or non-obvious implementations

### Comment Style

- Use single-line comments (`//`) for brief explanations
- Use multi-line comments (`/* */`) for longer descriptions within methods
- Group related parameters under a single header comment when appropriate
- Document performance considerations and potential pitfalls

### Examples

See the [PlayerCharacterController documentation](/Documentation/Coding/PlayerCharacterController.md) for a comprehensive example of proper code documentation.

## Technical Design Documentation

Technical design documents should:

- Clearly define the system architecture
- Include component diagrams where appropriate
- Specify interactions between systems
- Define data flow and state management
- Document technical requirements and constraints
- Provide rationale for architectural decisions

Our main Technical Design Document is structured as follows:

1. Introduction
2. System Architecture
3. Core Gameplay Systems
4. User Interface
5. Networking
6. Data Management
7. Technical Requirements

## User Documentation

User documentation should be:

- Task-oriented, focusing on how to accomplish specific goals
- Written in clear, non-technical language
- Illustrated with screenshots and diagrams where helpful
- Organized by feature or common workflow
- Versioned to match software releases

## Version Control Documentation

All significant commits should include descriptive messages following this format:

```
[Component] Brief summary of changes

- Detailed bullet points of significant changes
- Reasoning behind complex changes
- References to related issues or tickets
```

Git branch naming conventions:
- `feature/feature-name` for new features
- `bugfix/issue-description` for bug fixes
- `docs/documentation-topic` for documentation updates

## Cross-Platform Considerations

Documentation must account for both macOS and Windows environments:

- Use forward slashes (`/`) in all file paths
- Note platform-specific differences when relevant
- Provide command-line instructions for both platforms when needed
- Document shortcut keys for both platforms (e.g., "Ctrl+C (Windows) or Cmd+C (macOS)")

## Review Process

All documentation should undergo review:

1. **Technical accuracy**: Verified by the responsible developer or technical lead
2. **Style and consistency**: Checked against these documentation standards
3. **Clarity and completeness**: Reviewed for comprehension by someone unfamiliar with the feature

## References

- [Unity Documentation Style Guide](https://unity.com/documentation)
- [Microsoft Docs Style Guide](https://learn.microsoft.com/style-guide)
- [Google Developer Documentation Style Guide](https://developers.google.com/style)
- [BetterWYD Markdown Style Guide](/Documentation/Guidelines/MarkdownStyleGuide.md)
- [BetterWYD Folder Structure Guidelines](/Documentation/Guidelines/FolderStructure.md)