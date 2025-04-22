# Inventory System Design Document

**Version:** 1.0
**Date:** 22 Nisan 2025
**Author:** GitHub Copilot

## 1. Overview

This document outlines the design for the inventory system in BetterWYD-1. The inventory system will manage items collected by the player, allowing them to store, view, use, and organize these items.

## 2. Goals

*   Allow players to pick up, store, and manage items found in the game world.
*   Provide a clear and intuitive user interface for inventory management.
*   Support different item types (e.g., consumables, equipment, quest items, materials).
*   Handle item stacking for stackable items.
*   Integrate with other game systems (e.g., equipment system, crafting system, quest system).
*   Ensure persistence of inventory data across game sessions.

## 3. Core Concepts

*   **Inventory:** A container holding items. The player will have a primary inventory. Other entities (e.g., chests) might also have inventories.
*   **Item:** A distinct object that can be held in an inventory. Each item will have properties like name, description, icon, type, stackability, max stack size, effects (if usable/equipable), etc.
*   **Slot:** A space within the inventory grid where an item or a stack of items can be placed.
*   **Stacking:** Certain items (e.g., potions, crafting materials) can be grouped together in a single slot up to a maximum stack size. Unique items (e.g., specific weapons, quest items) will typically not stack.
*   **Item Types:**
    *   **Consumable:** Items that are used up upon activation (e.g., potions, food).
    *   **Equipment:** Items that can be equipped by the player character (e.g., weapons, armor).
    *   **Quest Item:** Items required for completing quests, often non-droppable or non-usable outside the quest context.
    *   **Material:** Items used for crafting or other systems.
    *   **Currency:** Special items representing game money.
    *   **Junk/Misc:** Items with no specific use other than potentially being sold.

## 4. Data Structures

*   **`InventoryData`:**
    *   `slots`: List/Array of `InventorySlotData`
    *   `capacity`: Integer (Maximum number of slots)
*   **`InventorySlotData`:**
    *   `itemId`: String/Integer (Reference to the Item Definition)
    *   `quantity`: Integer
*   **`ItemDefinition` (ScriptableObject or Database Entry):**
    *   `id`: String/Integer (Unique identifier)
    *   `itemName`: String
    *   `description`: String
    *   `icon`: Sprite/Texture reference
    *   `itemType`: Enum (Consumable, Equipment, Quest, Material, Currency, Junk)
    *   `isStackable`: Boolean
    *   `maxStackSize`: Integer (Relevant if `isStackable` is true)
    *   `useEffect`: Reference to script/data defining usage behavior (if applicable)
    *   `equipSlot`: Enum (Head, Chest, Weapon, etc., if `itemType` is Equipment)
    *   `statsModifiers`: List/Dictionary (if `itemType` is Equipment)
    *   `value`: Integer (Sell/buy price)

## 5. Functionality

*   **Adding Items:**
    *   When an item is picked up, the system checks for existing stacks of the same item with space available.
    *   If found, increment the quantity of the existing stack.
    *   If no existing stack is found or existing stacks are full, find the first empty slot.
    *   If an empty slot is found, add the item (or start a new stack) there.
    *   Handle overflow if the inventory is full. (Drop item, send to stash, notify player).
*   **Removing Items:**
    *   Dropping items from the inventory.
    *   Using consumable items (decrement quantity, remove if quantity reaches zero).
    *   Selling items to vendors.
    *   Using items for crafting.
*   **Moving Items:**
    *   Dragging and dropping items between slots within the inventory.
    *   Splitting stacks.
    *   Merging stacks.
*   **Using Items:**
    *   Triggering the `useEffect` associated with the item (e.g., applying health recovery, equipping armor).
*   **Sorting:**
    *   Provide options to sort the inventory (e.g., by type, name, value).
*   **Filtering:**
    *   (Optional) Allow filtering the view by item type.

## 6. User Interface (UI)

*   **Grid Layout:** Display inventory slots in a grid.
*   **Visual Representation:** Each occupied slot shows the item's icon and stack quantity (if > 1).
*   **Tooltips:** Display item details (name, description, stats, value) when hovering over an item.
*   **Drag and Drop:** Intuitive interaction for moving items.
*   **Context Menu:** Right-click options (Use, Equip, Drop, Split Stack, Examine).
*   **Capacity Indicator:** Show current occupancy vs. total capacity.
*   **Sorting/Filtering Controls:** Buttons or dropdowns for organization.

## 7. Persistence

*   Inventory state (`InventoryData`) needs to be saved when the game saves and loaded when the game loads.
*   Use a suitable serialization method (e.g., JSON, binary serialization) to store the data.

## 8. Integration Points

*   **Player Controller:** Handles item pickup interactions.
*   **Equipment System:** Equipping items removes them from the main inventory grid and places them in dedicated equipment slots. Unequipping returns them.
*   **Crafting System:** Consumes materials from the inventory.
*   **Quest System:** Adds/removes quest items, checks for required items.
*   **Vendor/Shop System:** Handles buying (adding items) and selling (removing items).
*   **Loot System:** Generates items to be added upon defeating enemies or opening containers.

## 9. Technical Considerations

*   **Platform:** Unity Engine.
*   **Data Management:** Consider using ScriptableObjects for `ItemDefinition` for easy editing and management within the Unity Editor.
*   **UI Implementation:** Unity UI (UGUI) or UI Toolkit.
*   **Performance:** Optimize inventory updates and UI refreshes, especially for large inventories. Avoid unnecessary updates.
*   **Modularity:** Design components (e.g., `InventoryManager`, `ItemDatabase`, `InventoryUI`) with clear responsibilities and interfaces.

## 10. Open Questions / Future Enhancements

*   Weight system?
*   Inventory tabs/categories?
*   Shared stash/bank system?
*   Item durability?
*   Item comparison UI?
*   Controller support for UI navigation?

### 10.2 Development Approach

1. Implement core inventory data structure and management functions
2. Develop basic UI representation without detailed styling
3. Integrate with player character for testing item pickup and usage
4. Implement persistence layer
5. Connect with other systems (equipment, crafting, etc.)
6. Polish UI and interaction experience
7. Optimize performance for large inventories

### 10.3 Testing Strategy

- Unit tests for core inventory functions
- Integration tests with related systems
- Performance testing with large item counts
- UI usability testing

## 11. Future Enhancements

- Weight-based inventory system
- Equipment comparison tooltips
- Inventory categories/tabs
- Shared stash/bank system
- Item durability/degradation
- Inventory loadouts/presets
- Search functionality
- Item locking to prevent accidental deletion
- Custom player organization options
- Item crafting within inventory interface
- Mobile-friendly interaction pattern
