# BetterWYD Style Guide

This document outlines the coding conventions, style guidelines, and project structure standards for the BetterWYD project.

## Table of Contents

1. [C# Coding Conventions](#c-coding-conventions)
2. [Project Structure](#project-structure)
3. [Unity-Specific Guidelines](#unity-specific-guidelines)
4. [Asset Naming Conventions](#asset-naming-conventions)
5. [Prefab Structure](#prefab-structure)
6. [Scene Organization](#scene-organization)
7. [Documentation Standards](#documentation-standards)
8. [Markdown Formatting](#markdown-formatting)
9. [Version Control Practices](#version-control-practices)

## C# Coding Conventions

### Naming Conventions

- **PascalCase** for:
  - Class names
  - Method names
  - Properties
  - Public fields
  - Namespaces
  - Enums

- **camelCase** for:
  - Local variables
  - Method parameters

- **_camelCase** for:
  - Private fields (prefix with underscore)

- **UPPER_CASE** for:
  - Constants

```csharp
// Example
public class PlayerController : MonoBehaviour
{
    // Constants
    public const float MAX_HEALTH = 100f;
    
    // Public properties
    public float MovementSpeed { get; private set; }
    
    // Private fields
    private Vector3 _moveDirection;
    private Animator _animator;
    
    // Method example
    public void CalculateDamage(float baseDamage, float multiplier)
    {
        float finalDamage = baseDamage * multiplier;
        ApplyDamage(finalDamage);
    }
}
```

### Code Organization

1. **Namespace Usage**
   - Organize code into namespaces that match the folder structure
   - Example: `namespace BetterWYD.Characters.Player`

2. **Class Structure Order**
   - Constants
   - Public fields
   - Private fields
   - Properties
   - Unity methods (Awake, Start, Update, etc.)
   - Public methods
   - Private methods

3. **File Organization**
   - One class per file (with exceptions for small, related classes)
   - Filename should match the primary class name

### Formatting

- Use 4 spaces for indentation (not tabs)
- Place braces on new lines
- Keep lines under 120 characters when possible
- Use blank lines to separate logical sections of code

### Comments

- Use XML documentation comments for public methods and classes
- Add inline comments for complex logic
- Comment non-obvious code

```csharp
/// <summary>
/// Calculates damage based on character stats and weapon properties
/// </summary>
/// <param name="baseDamage">The base damage value</param>
/// <param name="isCritical">Whether this is a critical hit</param>
/// <returns>The final calculated damage</returns>
public float CalculateDamage(float baseDamage, bool isCritical)
{
    // Apply critical hit multiplier if applicable
    float multiplier = isCritical ? _criticalHitMultiplier : 1.0f;
    
    // Calculate damage based on strength and weapon stats
    return (baseDamage + (Strength * _strengthDamageRatio)) * multiplier;
}
```

## Project Structure

The BetterWYD project follows this folder structure within the Assets folder:

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

For detailed information about the project structure, folder management guidelines, and organization principles, see the [Folder Structure Guidelines](/Documentation/Guidelines/FolderStructure.md).

### Scripts Organization

Scripts should be organized into subdirectories based on functionality:

- **Core**: Essential managers and systems
- **Characters**: Player and NPC-related scripts
- **UI**: User interface components
- **Gameplay**: Game mechanics and systems
- **Network**: Networking and multiplayer functionality
- **Utils**: Helper classes and extensions

## Unity-Specific Guidelines

### MonoBehaviour Practices

1. **Component-Based Design**
   - Prefer composition over inheritance
   - Create small, focused components with single responsibilities
   - Use interfaces to define behavior contracts

2. **Unity Methods**
   - Use `Awake()` for initialization that doesn't depend on other components
   - Use `Start()` for initialization that depends on other components
   - Use `OnEnable()`/`OnDisable()` for event subscription

3. **Performance Considerations**
   - Cache component references in `Awake()` or `Start()`
   - Avoid using `GetComponent<>()` in `Update()` methods
   - Use coroutines for time-delayed operations

```csharp
// Good practice example
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    private void Awake()
    {
        // Cache component references
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
}
```

### Serialized Fields

- Use `[SerializeField]` for private fields that need to be exposed in the Inspector
- Use appropriate ranges and tooltips
- Group related fields with headers

```csharp
[Header("Movement Settings")]
[SerializeField] 
[Tooltip("Base movement speed in units per second")]
[Range(1f, 10f)] 
private float _movementSpeed = 5f;

[SerializeField] 
[Tooltip("Maximum running speed multiplier")]
private float _runSpeedMultiplier = 2f;

[Header("Combat Settings")]
[SerializeField] private float _attackRate = 1f;
```

### Prefab Usage

- Prefer prefabs for reusable game objects
- Use nested prefabs for complex objects
- Create prefab variants for similar objects with minor differences

## Asset Naming Conventions

Use consistent naming patterns for all assets:

### General Naming Pattern

`TypePrefix_Category_Name_Variant`

### Type Prefixes

- **Animations**: Anim_
- **Materials**: Mat_
- **Models**: Mdl_
- **Textures**: Tex_
- **Sprites**: Spr_
- **Prefabs**: Pfb_
- **Scenes**: Scn_
- **Scripts**: No prefix (use PascalCase)
- **Sound Effects**: Sfx_
- **Music**: Mus_

### Examples

- `Anim_Character_Knight_Attack`
- `Mat_Environment_Stone_Mossy`
- `Mdl_Character_Warrior_Female`
- `Tex_Item_Weapon_Sword`
- `Pfb_UI_Button_Red`
- `Sfx_Combat_Sword_Hit`

## Prefab Structure

### Character Prefabs

```
Character (Empty GameObject)
├── Model
│   ├── Mesh
│   └── Armature
├── Effects
├── Audio
└── Colliders
```

### UI Prefabs

```
UIPanel (Canvas or Panel)
├── Header
│   ├── Title
│   └── CloseButton
├── Content
└── Footer
    └── Buttons
```

## Scene Organization

Organize scenes with a clear hierarchy using empty GameObjects as containers:

```
Scene
├── _Managers
│   ├── GameManager
│   ├── UIManager
│   └── AudioManager
├── Environment
│   ├── Terrain
│   ├── Structures
│   └── Props
├── Lighting
│   ├── Directional Light
│   └── Point Lights
├── Characters
│   ├── Player
│   └── NPCs
└── UI
    ├── HUD
    └── Menus
```

### Tagging and Layers

- Use consistent tags for common object types
- Set up layers for collision management
- Document the purpose of custom tags and layers

## Documentation Standards

### Code Documentation

- Document all public classes and methods with XML comments
- Document complex algorithms and non-obvious implementations
- Keep comments up-to-date when code changes

### Design Documentation

- Update the Technical Design Document when implementing new systems
- Document architectural decisions and patterns used
- Create diagrams for complex systems

### Markdown Formatting

All documentation files should follow our markdown standards:

- Follow the [Markdownlint Rules Guide](/Documentation/Guidelines/MarkdownlintRulesGuide.md) for consistent formatting
- Use the project's `.markdownlint.json` configuration
- Write clear, concise, and well-structured documentation
- Include a single top-level heading as the document title
- Use proper heading hierarchy without skipping levels
- Use dash (-) for unordered lists
- Use incremental numbers for ordered lists
- Specify language for code blocks
- Keep line length under 100 characters where possible

## Version Control Practices

See the [Git Documentation](GitDocumentation.md) for specific version control guidelines, including:

- Branch naming conventions
- Commit message formats
- Pull request procedures
- LFS usage for large assets

## Conclusion

Following these guidelines will ensure consistency across the BetterWYD project, improve code readability, and facilitate efficient development. If you have questions or suggestions for improving these guidelines, note them for future reference.

## References

- [BetterWYD Folder Structure Guidelines](/Documentation/Guidelines/FolderStructure.md)
- [BetterWYD Markdownlint Rules Guide](/Documentation/Guidelines/MarkdownlintRulesGuide.md)
- [BetterWYD Documentation Standards](/Documentation/Guidelines/DocumentationStandards.md)
- [Unity Best Practices](https://docs.unity3d.com/Manual/MobileOptimizationPracticalGuide.html)
- [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)