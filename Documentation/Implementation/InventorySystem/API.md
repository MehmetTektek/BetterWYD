# Inventory System API Documentation

## Core Classes

### ItemDefinition

ScriptableObject that defines item properties.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| itemId | string | Unique identifier for the item |
| displayName | string | Name shown in UI |
| description | string | Item description |
| type | ItemType | Category of the item |
| rarity | ItemRarity | Item rarity level |
| icon | Sprite | UI icon |
| prefab | GameObject | 3D model prefab |
| maxStackSize | int | Maximum stack size |
| isStackable | bool | Whether item can stack |
| baseValue | int | Base economic value |

#### Creating Items in Editor

```csharp
// Right-click in Project window
// Select Create > BetterWYD/Items/Item Definition
// Configure properties in Inspector
```

### ItemDatabase

Singleton that manages item definitions.

#### Methods

| Method | Parameters | Returns | Description |
|--------|------------|---------|-------------|
| GetItem | string itemId | ItemDefinition | Gets item by ID, returns null if not found |

#### Usage Example

```csharp
// Get item definition
ItemDefinition potion = ItemDatabase.Instance.GetItem("health_potion");
if (potion != null)
{
    Debug.Log($"Found item: {potion.displayName}");
}
```

### InventoryManager

Manages inventory operations.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| maxSlots | int | Maximum inventory capacity |
| slots | List<ItemSlot> | List of inventory slots |

#### Events

| Event | Parameters | Description |
|-------|------------|-------------|
| OnSlotChanged | ItemSlot | Fired when slot contents change |

#### Methods

| Method | Parameters | Returns | Description |
|--------|------------|---------|-------------|
| AddItem | ItemDefinition item, int quantity = 1 | bool | Adds items to inventory |
| RemoveItem | string itemId, int quantity = 1 | bool | Removes items from inventory |

#### Usage Example

```csharp
public class InventoryExample : MonoBehaviour
{
    private InventoryManager inventory;

    private void Start()
    {
        // Get reference to inventory
        inventory = GetComponent<InventoryManager>();
        
        // Subscribe to changes
        inventory.OnSlotChanged += HandleSlotChanged;
        
        // Add items
        ItemDefinition sword = ItemDatabase.Instance.GetItem("iron_sword");
        if (inventory.AddItem(sword))
        {
            Debug.Log("Added sword to inventory");
        }
    }

    private void HandleSlotChanged(ItemSlot slot)
    {
        // Update UI or perform other actions
        if (!slot.isEmpty)
        {
            Debug.Log($"Slot now contains {slot.quantity}x {slot.item.displayName}");
        }
    }
}
```

## Integration Examples

### Player Equipment System

```csharp
public class PlayerEquipment : MonoBehaviour
{
    private InventoryManager inventory;

    public bool EquipItem(string itemId)
    {
        ItemDefinition item = ItemDatabase.Instance.GetItem(itemId);
        if (item?.type == ItemType.Weapon || item?.type == ItemType.Armor)
        {
            if (inventory.RemoveItem(itemId, 1))
            {
                // Equipment logic here
                return true;
            }
        }
        return false;
    }
}
```

### Shop System

```csharp
public class ShopSystem : MonoBehaviour
{
    private InventoryManager playerInventory;

    public bool PurchaseItem(string itemId, int quantity)
    {
        ItemDefinition item = ItemDatabase.Instance.GetItem(itemId);
        if (item != null)
        {
            int totalCost = item.baseValue * quantity;
            if (CanAfford(totalCost))
            {
                if (playerInventory.AddItem(item, quantity))
                {
                    SpendCurrency(totalCost);
                    return true;
                }
            }
        }
        return false;
    }
}
```

## Best Practices

1. **Item IDs**
   - Use consistent naming convention (e.g., "category_name_variant")
   - Keep IDs lowercase with underscores
   - Include item type in ID when relevant

2. **Event Handling**
   - Always unsubscribe from OnSlotChanged when destroying objects
   - Handle null cases in event callbacks
   - Keep event handlers lightweight

3. **Performance**
   - Cache ItemDatabase.Instance when making multiple calls
   - Use item IDs instead of definitions for lookups
   - Batch inventory operations when possible