# Inventory System Documentation

## Overview

The inventory system manages item storage, stacking, and manipulation in BetterWYD. It uses a modular, component-based architecture with ScriptableObjects for item definitions and supports cross-platform compatibility between macOS and Windows.

## Core Components

### ItemDefinition (ScriptableObject)

Defines the base properties of items in the game.

```csharp
public class ItemDefinition : ScriptableObject
{
    public string itemId;         // Unique identifier for the item
    public string displayName;    // Name shown in UI
    public string description;    // Item description
    public ItemType type;        // Category of the item
    public ItemRarity rarity;    // Item rarity level
    public Sprite icon;          // UI icon
    public GameObject prefab;    // 3D model prefab
    public int maxStackSize;     // Maximum stack size (default: 1)
    public bool isStackable;     // Whether the item can stack
    public int baseValue;        // Base economic value
}
```

### ItemSlot

Represents a single slot in an inventory, containing an item and its quantity.

```csharp
public class ItemSlot
{
    public ItemDefinition item;   // Reference to the item definition
    public int quantity;         // Current stack size
    public bool isEmpty;         // Convenience property for empty slots
}
```

### ItemDatabase

Manages the loading and retrieval of item definitions.

```csharp
public class ItemDatabase : MonoBehaviour
{
    // Singleton instance
    private static ItemDatabase instance;
    
    // Dictionary of all loaded items
    private Dictionary<string, ItemDefinition> itemDictionary;
    
    // Loads items from Resources folder
    private void LoadItems();
}
```

### InventoryManager

Handles inventory operations and maintains inventory state.

```csharp
public class InventoryManager : MonoBehaviour
{
    public int maxSlots;                     // Maximum inventory capacity
    public List<ItemSlot> slots;            // Inventory slots
    public event Action<ItemSlot> OnSlotChanged; // Event for UI updates
    
    public bool AddItem(ItemDefinition item, int quantity);
    public bool RemoveItem(string itemId, int quantity);
}
```

## Usage

### Creating New Items

1. Right-click in the Project window
2. Select Create > BetterWYD/Items/Item Definition
3. Fill in the item properties in the Inspector
4. Place the asset in Resources/Items folder

### Adding Items to Inventory

```csharp
// Get reference to inventory manager
InventoryManager inventory = GetComponent<InventoryManager>();

// Add items
ItemDefinition potion = ItemDatabase.Instance.GetItem("health_potion");
bool success = inventory.AddItem(potion, 5);
```

### Removing Items

```csharp
// Remove items by ID
bool success = inventory.RemoveItem("health_potion", 2);
```

## Cross-Platform Considerations

- All file paths use forward slashes for cross-platform compatibility
- Item assets are loaded from Resources folder for consistent runtime access
- UI elements scale properly on different screen resolutions

## Performance Optimization

- Item definitions are loaded once at startup
- Dictionary lookup for fast item retrieval
- Events for efficient UI updates
- Minimal garbage collection impact

## Future Enhancements

- Item categories and filtering
- Advanced stacking rules
- Item tooltips
- Drag and drop interface
- Item crafting system
- Item durability

## References

- [Technical Design Document](/Documentation/TechnicalDesignDocument.md)
- [UI Architecture](/Documentation/Architecture/UISystem.md)
- [Asset Management Guidelines](/Documentation/Guidelines/AssetManagement.md)