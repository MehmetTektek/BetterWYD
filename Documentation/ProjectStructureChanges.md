# Project Structure Changes

This document tracks significant changes to the BetterWYD project structure and organization.

## Table of Contents

1. [Current Structure](#current-structure)
2. [April 22, 2025 Updates](#april-22-2025-updates)

## Current Structure

The current project structure is organized as follows:

```
Assets/
├── Core/               # Core game systems and essential utilities
│   ├── GameManagement/ # Game managers and global controllers
│   ├── Input/          # Input processing systems
│   ├── Events/         # Event system implementation
│   └── Utils/          # Utility classes and helper functions
├── Characters/         # All character-related assets
│   ├── Player/         # Player character models, animations and controllers
│   └── NPCs/           # Non-player characters
├── UI/                 # User interface elements
│   ├── HUD/            # Heads-up display elements
│   └── Menus/          # Menu screens and components
├── ScriptableObjects/  # Game data defined as ScriptableObjects
│   ├── Items/          # Item definitions
│   └── Character/      # Character data
└── Development/        # Development-only assets and test implementations
    ├── Scenes/         # Test scenes
    ├── Scripts/        # Development scripts
    └── Prefabs/        # Test prefabs
```

## April 22, 2025 Updates

### Changes Made

1. **Removed Empty Folders:**
   - Removed unused empty folders to streamline the project structure
   - Kept only folders that are currently in active use

2. **Script Reorganization:**
   - Moved core implementations from Development folder to their respective functional folders
   - PlayerCharacterController.cs moved from Development/Scripts to Characters/Player/Scripts
   - Input handling scripts moved from Development/Scripts to Core/Input
   - Maintained Development folder for work-in-progress implementations only

3. **Documentation Updates:**
   - Updated all implementation documentation to reflect new file paths
   - Revised folder structure guidelines
   - Created this change log to track structural modifications

### Rationale

These changes were made to:

1. Reduce clutter in the project hierarchy
2. Improve organization of finalized implementations 
3. Provide a clearer separation between in-development work and production-ready systems
4. Better align the folder structure with the modular architecture described in the Technical Design Document

### Empty Folder Management

To maintain folders in version control while keeping the project clean:

1. Add `.gitkeep` files to important empty folders
2. Document planned folder usage in this file
3. Remove completely unnecessary empty folders

### Next Steps

- Update Unity project settings to reflect the new folder organization
- Review all script references to ensure they point to the correct locations
- Notify all team members of the new structure
