# BetterWYD Coding Standards

This document outlines the coding standards and best practices for the BetterWYD project.

## Script Organization

### Header Comments

All scripts should include a header comment with the following information:

```csharp
/**
 * ClassName.cs
 * 
 * Brief description of what this script does
 * 
 * Created: [Date]
 * Last Modified: [Date]
 * Author: [Author Name]
 */
```

### Script Structure

Scripts should follow this general structure:

1. Namespace declarations
2. Using statements
3. Class declaration
4. Serialized fields/public properties
5. Private variables
6. Unity event methods (Awake, Start, Update, etc.)
7. Public methods
8. Private methods

Example:

```csharp
using System;
using UnityEngine;

namespace BetterWYD.Characters
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        
        private Vector3 _moveDirection;
        
        private void Awake()
        {
            // Initialization code
        }
        
        private void Update()
        {
            // Update logic
        }
        
        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
        
        private void ProcessMovement()
        {
            // Movement processing logic
        }
    }
}
```

## Naming Conventions

### General

- Use **PascalCase** for class names, public methods, and public properties
- Use **camelCase** for local variables and private methods
- Use **_camelCase** (with leading underscore) for private fields
- Use **UPPER_CASE** for constants

### Unity-Specific

- Use descriptive names for GameObjects and Prefabs
- Prefix prefab names with "Prefab_"
- Group-related GameObjects with empty parent GameObjects
- Name UI elements with a UI prefix (e.g., "UI_MainMenu")

## Code Formatting

- Use 4 spaces for indentation (not tabs)
- Place opening braces on the same line as the statement
- Use spaces around operators
- Keep lines shorter than 100 characters when possible

## Comments

- Use comments to explain "why", not "what"
- Comment complex logic that isn't immediately obvious
- Keep comments current when modifying code
- Use `//TODO:` comments for future implementation notes

## Best Practices

### Unity-Specific

- Use `[SerializeField]` instead of making fields public
- Cache component references in Awake when possible
- Use GetComponent calls sparingly
- Use Object Pooling for frequently instantiated/destroyed objects
- Use ScriptableObjects for configuration data

### Performance

- Avoid Find, FindObjectOfType, and GameObject.Find in Update methods
- Use non-allocating physics APIs (e.g., Physics.RaycastNonAlloc)
- Batch operations when possible
- Optimize UI by minimizing Canvas rebuilds

### Architecture

- Follow the SOLID principles
- Use interfaces for loose coupling between systems
- Implement proper error handling and logging
- Follow the component-based architecture outlined in the Technical Design Document

## Unity Editor Extensions

- Keep custom editor scripts in an Editor folder
- Document custom editor functionality
- Ensure editor scripts don't cause performance issues in the build