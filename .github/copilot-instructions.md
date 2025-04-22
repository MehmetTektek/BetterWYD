# BetterWYD Project Instructions

## Core Rules

- After every action create a log with short summary of action and timestamp
- Use the history file to remember previous interactions. Located in AgentRuntime/CopilotHistory.md
- Read last 5 interaction and if needed read more
- Create a log in the beginning of every prompt and update it end of every prompt
- Create files with Markdownlint format

## Cross-Platform Compatibility

- We are using macOS and Windows together for development
- Consider compatibility when creating new implementations

## Documentation and Organization

- Follow the comprehensive documentation standards defined in Documentation/Guidelines/DocumentationStandards.md
- Keep the project organized according to Documentation/Guidelines/FolderStructure.md
- Create new folders for new implementations
- Always create or update documentation with up-to-date status of project

## Code Quality

- Do not create or code any scripts, guide user with code snippets and explainations.
- Specify name and where scripts needs to be created.
- Add comments to scripts to make them easier to understand and maintain
- Create headers for scripts with a brief summary and explanation of how they work
- Follow C# documentation style guide in DocumentationStandards.md for all code documentation
- Organize documentation with proper categorization via folders

## Architecture Patterns

- Follow the modular, component-based architecture outlined in the Technical Design Document
- Ensure new systems integrate properly with the existing core architectural components

## Version Control

- Create descriptive commit messages that clearly explain changes
- Use feature branches for new implementations (format: feature/your-feature-name)
- Create pull/merge requests for code review before merging to main branches
- Tag important releases with version numbers
- Use Git LFS for large binary files (textures, models, audio, video)
- Avoid committing scene files with conflicts

## Asset Management

- Follow a consistent naming convention for all assets (Prefabs, Materials, Textures, etc.)
- Organize assets in appropriate folders by type and functionality

## UI Development

- Follow the UI architecture outlined in the Technical Design Document

## Project Timeline Awareness

- Prioritize features according to the development roadmap
- Follow the phase-specific focus (Phase 1: Foundation, Phase 2: Gameplay, Phase 3: Polish)
- Document progress and update milestone status
- Focus on completing core features before adding enhancements

## Performance Optimization

- Profile regularly to identify bottlenecks
- Establish performance budgets for different systems (rendering, physics, AI)
- Optimize asset loading and memory usage
- Consider both minimum and recommended hardware specifications
