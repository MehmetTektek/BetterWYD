# InventoryManager Implementation Reference

## Overview

The `InventoryManager` class is a core component of BetterWYD's inventory system that manages the player's inventory operations. It provides functionality for adding, removing, stacking, and organizing items within the player's inventory.

## Location

`/Assets/Core/GameManagement/InventoryManager.cs`

## Namespace

`BetterWYD.Inventory`

## Dependencies

The `InventoryManager` relies on the following libraries and components:

| Dependency | Purpose |
|------------|---------|
| `UnityEngine` | Core Unity functionality including MonoBehaviour |
| `System` | For basic .NET functionality and Action delegates |
| `System.Collections.Generic` | Provides List<T> collection for inventory slots |
| `System.Linq` | Used for querying and filtering inventory slots |
| `ItemDefinition` | ScriptableObject that defines item properties |
| `ItemSlot` | Data structure representing individual inventory slots |

## API Reference

### Properties and Fields

| Name | Type | Description |
|------|------|-------------|
| `maxSlots` | int | Maximum number of slots in the inventory |
| `slots` | List\<ItemSlot\> | Collection of inventory slots |
| `OnSlotChanged` | Action\<ItemSlot\> | Event triggered when a slot's content changes |

### Public Methods

| Method | Parameters | Returns | Description |
|--------|------------|---------|-------------|
| `AddItem` | ItemDefinition item, int quantity = 1 | bool | Adds an item to the inventory, automatically handling stacking |
| `RemoveItem` | string itemId, int quantity = 1 | bool | Removes a specified amount of an item from the inventory |
| `OptimizeStacks` | none | void | Combines similar stacks to optimize inventory space |
| `SplitStack` | int slotIndex, int splitAmount | bool | Splits a stack into two separate stacks |

### Private Methods

| Method | Parameters | Returns | Description |
|--------|------------|---------|-------------|
| `FindStackableSlot` | ItemDefinition item, int amount | ItemSlot | Finds a slot that can stack more of an item |
| `FindEmptySlot` | none | ItemSlot | Finds the first empty inventory slot |
| `CalculateStackAddAmount` | ItemSlot slot, int requestedAmount | int | Calculates how many items can be added to a slot |

## Initialization

The `InventoryManager` initializes the inventory slots in its `Start` method. If no slots exist, it creates a number of empty slots equal to `maxSlots`.

```csharp
private void Start()
{
    // Initialize the inventory with empty slots
    if (slots == null || slots.Count == 0)
    {
        slots = new List<ItemSlot>(maxSlots);
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new ItemSlot());
        }
    }
}
```

## Item Stacking Logic

The stacking system follows these rules:

1. Always try to add items to existing stacks first
2. Only create new stacks when necessary
3. Respect each item's maximum stack size
4. Handle stackable and non-stackable items differently

Here's how the stacking logic works in the `AddItem` method:

1. Validate item and quantity inputs
2. Check for existing stacks with the same item that can accept more
3. Fill those stacks first, up to their maximum capacity
4. If there are remaining items, look for empty slots
5. Create new stacks in empty slots
6. Return false if there's not enough space for all items

## Implementation Details

### C# Features Used

1. **LINQ**: The class uses LINQ's `Where`, `OrderBy`, and `FirstOrDefault` to efficiently query inventory slots.

```csharp
var slotsWithItem = slots
    .Where(slot => slot.item?.itemId == itemId)
    .OrderBy(slot => slot.quantity)
    .ToList();
```

2. **Null Conditional Operator (`?.`)**: Used for safe access to potentially null properties.

```csharp
OnSlotChanged?.Invoke(slot);
```

3. **Lambda Expressions**: Used with LINQ and for slot searching.

```csharp
return slots.Find(slot => slot.isEmpty);
```

4. **Default Parameter Values**: Used in method signatures for convenient overloads.

```csharp
public bool AddItem(ItemDefinition item, int quantity = 1)
```

5. **Expression-bodied Properties**: Used for computed properties like `isEmpty`.

```csharp
public bool isEmpty => item == null || quantity <= 0;
```

### Unity-Specific Features

1. **MonoBehaviour**: The class inherits from MonoBehaviour to integrate with Unity's component system.
2. **Tooltip Attribute**: Used to provide hover descriptions in the Unity inspector.
3. **Mathf**: Unity's mathematics library used for min/max calculations.

## Usage Examples

### Adding Items

```csharp
// Get reference to inventory manager
var inventoryManager = GetComponent<InventoryManager>();

// Get item from database
var potion = ItemDatabase.Instance.GetItem("health_potion");

// Add 5 potions to inventory
if (inventoryManager.AddItem(potion, 5))
{
    Debug.Log("Added 5 potions to inventory");
}
else
{
    Debug.Log("Not enough space in inventory");
}
```

### Removing Items

```csharp
// Remove 2 potions from inventory
if (inventoryManager.RemoveItem("health_potion", 2))
{
    Debug.Log("Removed 2 potions from inventory");
}
else
{
    Debug.Log("Not enough potions in inventory");
}
```

### Stack Management

```csharp
// Optimize inventory by combining similar stacks
inventoryManager.OptimizeStacks();

// Split a stack (divide slot 5's stack, moving half to a new slot)
int slotIndex = 5;
var slot = inventoryManager.slots[slotIndex];
int halfAmount = slot.quantity / 2;
inventoryManager.SplitStack(slotIndex, halfAmount);
```

## Event System

The `OnSlotChanged` event is triggered whenever a slot's contents change. UI elements should subscribe to this event to stay updated.

```csharp
// Subscribe to inventory changes
inventoryManager.OnSlotChanged += OnInventorySlotChanged;

// Handle inventory changes
private void OnInventorySlotChanged(ItemSlot slot)
{
    // Update UI or other systems
    UpdateInventoryUI();
}
```

## Common Issues and Solutions

| Issue | Solution |
|-------|----------|
| Items not stacking properly | Check if `isStackable` is true and `maxStackSize` > 1 |
| Inventory full errors | Consider increasing `maxSlots` or implementing overflow handling |
| Missing item references | Ensure ItemDatabase has loaded properly before accessing items |
| Stack splitting failures | Verify there's at least one empty slot available |

## Performance Considerations

1. **LINQ Efficiency**: LINQ queries should be optimized in performance-critical situations.
2. **Event Handling**: Too many subscribers to `OnSlotChanged` could impact performance.
3. **Stack Optimization**: The `OptimizeStacks` method iterates multiple times and should not be called every frame.

## Cross-Platform Compatibility

The inventory system is designed to work identically on both Windows and macOS platforms. All code uses standard Unity and C# features with no platform-specific dependencies.

## References

- [Inventory System Overview](/Documentation/Implementation/InventorySystem/Overview.md)
- [Item System API Documentation](/Documentation/Implementation/InventorySystem/API.md)
- [Technical Design Document](/Documentation/TechnicalDesignDocument.md)