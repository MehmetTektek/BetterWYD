# BetterWYD Project Instructions

## Project Overview
This is a modern remake of the classic MMORPG "With Your Destiny" (WYD) using Unity. The game features four character classes, extensive character progression, rich PvE content, and competitive PvP elements.

## Development Guidelines

### Cross-Platform Compatibility
- Use macOS and Windows together for development
- Consider compatibility when creating new implementations

## Documentation and Organization

- Follow existing documentation
- Keep the project organized
- Create new folders for new implementations
- Always create or update documentation with up-to-date status of project

## Code Quality

- Add comments to scripts to make them easier to understand and maintain
- Create headers for scripts with a brief summary and explanation of how they work
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
