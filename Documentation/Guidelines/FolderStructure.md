# Folder Structure Guidelines

## Current Project Structure
The BetterWYD project follows this updated organizational structure:

```
Assets/
├── Core/               # Core game systems and essential utilities
│   ├── GameManagement/ # Game managers and global controllers
│   ├── Input/          # Input processing systems including player input handlers
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
├── Settings/           # Project configurations and settings
├── Scenes/             # Game scenes and levels
├── Resources/          # Runtime-loaded assets
├── Networking/         # Multiplayer functionality
├── TextMesh Pro/       # Text rendering system
├── VFX/                # Visual effects
└── Development/        # Development-only assets and test implementations
    ├── Scenes/         # Test scenes
    ├── Scripts/        # Development scripts
    └── Prefabs/        # Test prefabs
```

## Recent Project Structure Updates
As of April 24, 2025, we've made the following updates to our project structure:

1. Added missing folders to the documentation that exist in the actual project (Networking, Settings, Scenes, Resources, TextMesh Pro, VFX)
2. Added reference to the new markdownlint rules documentation
3. Reorganized implementation documentation to follow the standard template

Previous updates (April 22, 2025):

1. Removed unused empty folders (Classes, Enemies, Common, Fonts, Environments and subdirectories, Skills, Quests)
2. Moved core input implementations from Development to Core/Input
3. Moved PlayerCharacterController from Development to Characters/Player
4. Removed Materials folder from Development since it's currently unused

Empty folders will be added back as needed when implementations begin for those systems.

## Guiding Principles

1. **Functional Separation**: Assets are organized by their functional role in the game
2. **Clear Hierarchy**: Establish a clear parent/child relationship between folders
3. **Consistency**: Follow the same naming conventions and organizational patterns throughout
4. **Discoverability**: Make assets easy to find based on their purpose
5. **Cross-platform Compatibility**: Ensure folder names and paths work on both macOS and Windows

## Implementation Files

Implementation-specific scripts should be placed in their respective functional directories:
- Core game systems go in appropriate subfolders of `Assets/Core/`
- Character controllers go in `Assets/Characters/[CharacterType]/Scripts/`
- UI scripts go in `Assets/UI/[UISection]/Scripts/`

Development and test scripts should remain in the `Assets/Development/Scripts/` folder until they are finalized. Once a feature is complete and tested, scripts should be moved to their appropriate production location.

### Migration Guidelines

When migrating scripts from Development to production folders:

1. Create appropriate Scripts folder in the destination if it doesn't exist
2. Update any namespace declarations to match the new location
3. Update any references in other scripts or prefabs
4. Test thoroughly after migration
5. Document the migration in the ProjectStructureChanges.md file

## Naming Conventions

- Use PascalCase for folder names
- Avoid special characters in folder names
- Keep folder names concise but descriptive
- Prefer singular nouns for category folders (e.g., "Character" not "Characters")

## Version Control Considerations

- Add `.gitkeep` files to empty folders that should be preserved in version control
- Follow `.gitignore` patterns to exclude appropriate files and folders

## Development Workspace

The `Development` folder is specifically for work-in-progress features, testing, and prototyping. Assets in this folder should be considered temporary and not for production use. When features are completed and tested, they should be moved to their appropriate location in the production structure.

### Key Development Folders

- `Development/Scenes`: Contains test scenes for feature development
- `Development/Prefabs`: Contains prototype prefabs for testing
- `Development/Scripts`: Contains experimental scripts not yet ready for production

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

## Directory Structure Management

### When to Add New Folders

Create new folders when:
- Implementing a new major system
- Adding a new category of assets
- Logical grouping would improve organization
- The number of files in a folder exceeds 15-20

### When to Remove Folders

Remove folders when:
- They remain empty with no planned use in the next 2 development phases
- Their purpose has been consolidated with another folder
- The system they were intended for has been removed from the project scope

Always document folder removals in the ProjectStructureChanges.md file.

## Conclusion

This folder structure is a living standard that evolves with the project. Refer to the ProjectStructureChanges.md file for a history of structural updates. Following these guidelines will maintain consistency across the BetterWYD project and facilitate efficient development.

## References

- Unity Best Practices Documentation
- BetterWYD Technical Design Document
- Cross-Platform Development Guidelines
- [BetterWYD Project Structure Changes](./ProjectStructureChanges.md)
- [BetterWYD Markdownlint Rules Guide](./MarkdownlintRulesGuide.md)
- [BetterWYD Markdown Style Guide](./MarkdownStyleGuide.md)