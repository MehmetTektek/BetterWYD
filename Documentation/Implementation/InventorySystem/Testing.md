# Inventory System Testing Guide

## Test Scene Setup

1. Create a new test scene in Assets/Development/Scenes
2. Add these GameObjects to the scene:
   - GameManager (empty GameObject)
     - Add ItemDatabase component
   - Player (empty GameObject)
     - Add InventoryManager component

## Test Cases

### Item Database Testing

1. **Item Loading**
   - Create several test ItemDefinition assets in Resources/Items
   - Run the scene and verify ItemDatabase loads all items
   - Check console for any loading errors

2. **Item Retrieval**
   ```csharp
   // Test getting valid and invalid items
   ItemDefinition validItem = ItemDatabase.Instance.GetItem("test_item_1");
   ItemDefinition invalidItem = ItemDatabase.Instance.GetItem("nonexistent_item");
   
   Debug.Assert(validItem != null, "Valid item should be found");
   Debug.Assert(invalidItem == null, "Invalid item should return null");
   ```

### Inventory Operations

1. **Adding Items**
   - Test adding stackable items
   - Test adding non-stackable items
   - Test adding items to a full inventory
   - Verify OnSlotChanged events are fired correctly

2. **Removing Items**
   - Test removing entire stacks
   - Test removing partial stacks
   - Test removing non-existent items
   - Verify slot states after removal

3. **Stack Management**
   - Test stack size limits
   - Test combining partial stacks
   - Test splitting stacks

## Common Issues

1. **Items Not Loading**
   - Verify items are in the correct Resources folder
   - Check ItemDefinition asset configurations
   - Ensure unique itemIds

2. **Stack Operations Failing**
   - Verify maxStackSize settings
   - Check isStackable flag
   - Debug quantity calculations

## Performance Testing

1. Run profiler while:
   - Loading large numbers of items
   - Performing rapid inventory operations
   - Managing multiple inventories

2. Memory considerations:
   - Monitor garbage collection
   - Check for memory leaks
   - Verify proper cleanup on scene changes