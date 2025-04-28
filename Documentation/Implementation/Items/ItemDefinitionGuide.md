# ItemDefinition Guide

## Overview

The `ItemDefinition` ScriptableObject is the foundation of BetterWYD's item system. It defines the properties and characteristics of all items in the game, serving as a template for item creation in the Unity Editor.

## Purpose

This guide explains how to:
- Create new item definitions
- Configure item properties
- Extend the item system
- Best practices for item design

## Location

`ItemDefinition` is located at: `/Assets/ScriptableObjects/Items/ItemDefinition.cs`

## Core Components

### Item Types

The `ItemType` enum defines the categories of items available in the game:

```csharp
public enum ItemType
{
    Weapon,
    Armor,
    Accessory,
    Consumable,
    Material,
    Quest
}
```

### Item Rarity

The `ItemRarity` enum defines the rarity levels for items, which can impact item stats, visual effects, and value:

```csharp
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
```

### Item Properties

The `ItemDefinition` class contains the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `itemId` | string | Unique identifier for the item |
| `displayName` | string | Name displayed in the UI |
| `description` | string | Detailed description of the item |
| `type` | ItemType | Category of the item |
| `rarity` | ItemRarity | Rarity level of the item |
| `icon` | Sprite | Icon displayed in UI elements |
| `prefab` | GameObject | 3D model prefab for the item in the game world |
| `maxStackSize` | int | Maximum number of items that can be stacked |
| `isStackable` | bool | Whether this item can be stacked |
| `baseValue` | int | Base economic value of the item |

## Creating New Item Definitions

### Step-by-Step Guide

1. **Open Project View** in Unity
2. **Navigate** to a suitable folder (e.g., `Assets/ScriptableObjects/Items/Definitions/`)
3. **Right-click** in the Project window
4. Select **Create > BetterWYD > Items > Item Definition**
5. **Name** your new item definition appropriately (e.g., "Item_Sword_Steel")

### Configuration Guidelines

When configuring your new item:

1. **Assign a unique itemId**
   - Use a consistent format (e.g., "weapon_sword_steel")
   - Ensure IDs are unique across all items

2. **Set display name and description**
   - Keep names concise but descriptive
   - Descriptions should provide gameplay-relevant information

3. **Choose appropriate item type and rarity**
   - These affect how the item functions in-game
   - Rarity can impact the item's stats and value

4. **Add visual assets**
   - Assign an icon sprite for UI representation
   - Link a prefab if the item has a physical representation in the game world

5. **Configure stacking behavior**
   - Set `isStackable` to true for consumables, materials, etc.
   - Define an appropriate `maxStackSize`
   - Non-stackable items should have `maxStackSize = 1`

6. **Set economic value**
   - Consider the item's rarity, usefulness, and game balance
   - This value will influence shop prices and NPC interactions

## Extending the Item System

### Creating Specialized Item Types

To create specialized item types (e.g., weapons with damage stats):

1. Create a new class that inherits from `ItemDefinition`
2. Add specialized properties and functionality

Example for a weapon item:

```csharp
[CreateAssetMenu(fileName = "New Weapon", menuName = "BetterWYD/Items/Weapon Definition")]
public class WeaponDefinition : ItemDefinition
{
    [Tooltip("Base damage dealt by this weapon")]
    public float baseDamage;
    
    [Tooltip("Attack speed in attacks per second")]
    public float attackSpeed;
    
    [Tooltip("Range of the weapon in units")]
    public float range;
    
    [Tooltip("Type of damage dealt by this weapon")]
    public DamageType damageType;
    
    public WeaponDefinition()
    {
        // Default to weapon type
        type = ItemType.Weapon;
        isStackable = false;
        maxStackSize = 1;
    }
}
```

### Adding New Item Types

To add new item types:

1. Extend the `ItemType` enum in the original file
2. Update any UI or system code that uses the item types

```csharp
public enum ItemType
{
    Weapon,
    Armor,
    Accessory,
    Consumable,
    Material,
    Quest,
    Tool,  // New item type
    Collectible  // New item type
}
```

### Adding New Rarity Levels

To add new rarity levels:

1. Extend the `ItemRarity` enum
2. Update color schemes or visual indicators for the new rarity
3. Adjust drop rates and value calculations

```csharp
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic,  // New rarity level
    Artifact  // New rarity level
}
```

## Integration with Inventory System

The `ItemDefinition` class works directly with the inventory system:

1. Items in inventory slots reference the item definition
2. UI elements display information from the item definition
3. Item interactions use properties defined in the item definition

Example of a simple inventory slot implementation:

```csharp
[System.Serializable]
public class ItemSlot
{
    public ItemDefinition item;
    public int quantity;
    
    public bool IsEmpty => item == null;
    
    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
```

## Best Practices

1. **Maintain Unique IDs**
   - Use a systematic approach to naming and ID creation
   - Consider using prefixes for different item categories

2. **Balance Item Properties**
   - Consider game progression when setting rarity and value
   - Ensure consistent scaling between item tiers

3. **Asset Management**
   - Keep icons and prefabs organized in dedicated folders
   - Maintain consistent sprite sizes for UI elements

4. **Documentation**
   - Add comments to complex properties
   - Document any special behaviors or interactions

5. **Extensibility**
   - Design with future requirements in mind
   - Use inheritance for specialized item types

## Common Pitfalls

1. **Inconsistent IDs**: Ensure each item has a unique ID to prevent conflicts
2. **Missing Visual Assets**: Always assign proper icons and prefabs
3. **Unbalanced Values**: Test item value and stats against game progression
4. **Improper Stacking**: Configure `isStackable` and `maxStackSize` correctly
5. **Overlooking Tooltips**: Use tooltips to explain properties in the Unity Editor

## Related Systems

- Inventory System (`InventoryManager.cs`)
- Loot System
- Shop System
- Character Equipment System

## Changelog

| Date | Change | Author |
|------|--------|--------|
| 25 Nisan 2025 | Initial document creation | GitHub Copilot |