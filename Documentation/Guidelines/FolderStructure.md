# BetterWYD Project Folder Structure

## Overview

This document defines the standard folder structure for the BetterWYD project, ensuring consistency and organization across all development environments. Proper folder structure is critical for cross-platform compatibility between macOS and Windows development environments and for maintaining an efficient workflow as the project scales.

## Unity Assets Structure

All game assets must be placed within the Unity `Assets` folder to be recognized by the engine. The following structure outlines our standardized organization:

```
Assets/
├── Core/                  # Core game systems and framework
│   ├── GameManagement/    # Game managers and core controllers
│   ├── Input/             # Input system components
│   ├── Events/            # Event system and messaging
│   └── Utils/             # Utility scripts and helpers
│
├── Characters/            # Character-related assets
│   ├── Player/            # Player character models, animations, controllers
│   ├── Classes/           # Class-specific implementations and assets
│   ├── NPCs/              # Non-player character assets
│   └── Enemies/           # Enemy character assets
│
├── UI/                    # User interface assets
│   ├── HUD/               # Heads-up display elements
│   ├── Menus/             # Main menu and submenu screens
│   ├── Common/            # Reusable UI components
│   └── Fonts/             # Typography assets
│
├── Environments/          # World and environment assets
│   ├── Kersef/            # Kersef continent assets
│   ├── Dungeons/          # Dungeon environments
│   ├── Props/             # Environmental props and decorations
│   └── Effects/           # Environmental effects (fog, particles, etc.)
│
├── Audio/                 # Audio assets
│   ├── Music/             # Background music tracks
│   ├── SFX/               # Sound effects
│   └── Ambient/           # Ambient sound loops
│
├── VFX/                   # Visual effects
│   ├── Combat/            # Combat-related effects
│   ├── Skills/            # Skill and ability effects
│   └── Environment/       # Environmental visual effects
│
├── Networking/            # Networking assets and components
│   ├── Prefabs/           # Network-specific prefabs
│   └── Scripts/           # Network scripts and components
│
├── Resources/             # Assets loaded at runtime
│
├── ScriptableObjects/     # Scriptable object assets
│   ├── Items/             # Item definitions
│   ├── Skills/            # Skill definitions
│   ├── Quests/            # Quest definitions
│   └── Character/         # Character data definitions
│
└── Development/           # Development and testing assets
    ├── Scenes/            # Test scenes
    │   └── TestScene.unity # Main test scene for development
    ├── Prefabs/           # Test prefabs
    ├── Scripts/           # Experimental scripts
    └── Materials/         # Test materials
```

## Development Workspace

The `Development` folder is specifically for work-in-progress features, testing, and prototyping. Assets in this folder should be considered temporary and not for production use. When features are completed and tested, they should be moved to their appropriate location in the production structure.

### Key Development Folders

- `Development/Scenes`: Contains test scenes for feature development
- `Development/Prefabs`: Contains prototype prefabs for testing
- `Development/Scripts`: Contains experimental scripts not yet ready for production
- `Development/Materials`: Contains temporary materials for testing

## Cross-Platform Considerations

To ensure compatibility between macOS and Windows:

1. **Case Sensitivity**: Maintain consistent casing in file and folder names (macOS file systems can be case-sensitive)
2. **Path Separators**: Use forward slashes (`/`) in paths rather than backslashes (`\`)
3. **Special Characters**: Avoid using special characters or spaces in file and folder names
4. **Path Length**: Keep paths reasonably short to avoid issues with Windows path length limitations

## Version Control Guidelines

When working with Git:

1. **Git LFS**: Use Git LFS for large binary files (models, textures, etc.)
2. **Meta Files**: Never delete `.meta` files
3. **Ignored Files**: Respect the `.gitignore` file to avoid committing unnecessary files
4. **Committing Changes**: Commit related changes together with descriptive commit messages

## Best Practices

1. **Asset Naming**: Follow the established naming convention for all assets
   - Prefix assets with their type (e.g., `UI_MainMenu`, `Char_Player`, `Env_Tree`)
   - Use PascalCase for asset names
   - Be descriptive but concise

2. **Avoid Empty Folders**: Unity does not track empty folders in version control
   - Place a `.gitkeep` file in folders that need to be preserved but are temporarily empty

3. **Organization First**: Always place new assets in their appropriate folders immediately
   - Do not leave assets in the root of the Assets folder

4. **Dependency Management**: Keep related assets together to reduce dependencies between folders

## Conclusion

Following this folder structure will ensure consistency across the BetterWYD project and facilitate efficient collaboration among team members. As the project evolves, this structure may be refined to better accommodate new features and workflows.

## References

- Unity Best Practices Documentation
- BetterWYD Technical Design Document
- Cross-Platform Development Guidelines