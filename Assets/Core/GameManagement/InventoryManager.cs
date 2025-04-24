/**
 * InventoryManager.cs
 * 
 * Manages the player's inventory, handling item operations such as adding,
 * removing, stacking, and optimizing items in inventory slots.
 * 
 * This class is a core component of the BetterWYD inventory system and integrates
 * with the ItemDatabase for item definitions.
 * 
 * Created: April 22, 2025
 * Last Modified: April 23, 2025
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterWYD.Inventory
{
    /// <summary>
    /// Manages the player's inventory, allowing for adding, removing, and querying items.
    /// Implements stacking logic and slot management for efficient inventory operations.
    /// For full API reference, see: /Documentation/Implementation/InventorySystem/InventoryManagerReference.md
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        [Tooltip("Maximum number of slots in the inventory")]
        [SerializeField] private int _maxSlots = 30;
        
        /// <summary>
        /// Maximum number of inventory slots.
        /// </summary>
        public int MaxSlots => _maxSlots;
        
        [Tooltip("List of item slots in the inventory")]
        [SerializeField] private List<ItemSlot> _slots = new List<ItemSlot>();
        
        /// <summary>
        /// Read-only access to inventory slots.
        /// </summary>
        public IReadOnlyList<ItemSlot> Slots => _slots;
        
        /// <summary>
        /// Number of slots in the inventory.
        /// </summary>
        public int SlotCount => _slots?.Count ?? 0;

        /// <summary>
        /// Event triggered when an inventory slot changes.
        /// UI elements should subscribe to this event to stay updated.
        /// 
        /// Example usage:
        /// <code>
        /// inventoryManager.OnSlotChanged += (slot) => {
        ///     UpdateSlotUI(slot);
        /// };
        /// </code>
        /// </summary>
        public event Action<ItemSlot> OnSlotChanged;
        
        /// <summary>
        /// Event triggered when the entire inventory layout changes.
        /// </summary>
        public event Action OnInventoryChanged;

        private void Start()
        {
            // Initialize the inventory with empty slots if none exist
            // This ensures the inventory has the correct number of slots on startup
            if (_slots == null || _slots.Count == 0)
            {
                InitializeInventory(_maxSlots);
            }
        }
        
        /// <summary>
        /// Initializes the inventory with a specific number of empty slots.
        /// </summary>
        /// <param name="slotCount">Number of slots to initialize</param>
        public void InitializeInventory(int slotCount)
        {
            _slots = new List<ItemSlot>(slotCount);
            
            for (int i = 0; i < slotCount; i++)
            {
                _slots.Add(new ItemSlot());
            }
            
            OnInventoryChanged?.Invoke();
        }
        
        /// <summary>
        /// Clears all items from the inventory.
        /// </summary>
        public void ClearInventory()
        {
            foreach (var slot in _slots)
            {
                slot.item = null;
                slot.quantity = 0;
            }
            
            OnInventoryChanged?.Invoke();
        }
        
        /// <summary>
        /// Retrieves a specific inventory slot.
        /// </summary>
        /// <param name="slotIndex">Index of the slot to retrieve</param>
        /// <returns>The requested ItemSlot or null if index is invalid</returns>
        public ItemSlot GetSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < _slots.Count)
            {
                return _slots[slotIndex];
            }
            
            return null;
        }
        
        /// <summary>
        /// Sets the contents of a specific inventory slot.
        /// </summary>
        /// <param name="slotIndex">Index of the slot to modify</param>
        /// <param name="item">Item to place in the slot</param>
        /// <param name="quantity">Quantity of the item</param>
        /// <returns>True if successful, false if index is invalid</returns>
        public bool SetSlot(int slotIndex, ItemDefinition item, int quantity)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
            {
                return false;
            }
            
            var slot = _slots[slotIndex];
            slot.item = item;
            slot.quantity = quantity;
            
            OnSlotChanged?.Invoke(slot);
            return true;
        }

        /// <summary>
        /// Finds a slot that contains the specified item and has room for more.
        /// Used for stacking items in existing slots before creating new stacks.
        /// </summary>
        /// <param name="item">The item to find a slot for</param>
        /// <param name="amount">The amount we want to add</param>
        /// <returns>A suitable slot or null if none found</returns>
        private ItemSlot FindStackableSlot(ItemDefinition item, int amount)
        {
            // Find slots that:
            // 1. Contain the same item
            // 2. Are not at max stack size
            // 3. Have enough space for the amount we want to add
            return _slots.Find(slot => 
                slot.item == item && 
                slot.quantity < item.maxStackSize && 
                slot.quantity + amount <= item.maxStackSize);
        }

        /// <summary>
        /// Finds the first empty slot in the inventory.
        /// Used when creating new stacks of items.
        /// </summary>
        /// <returns>First empty slot or null if inventory is full</returns>
        private ItemSlot FindEmptySlot()
        {
            return _slots.Find(slot => slot.IsEmpty);
        }

        /// <summary>
        /// Calculates how many items can be added to a specific slot based on stack limits.
        /// Ensures we don't exceed the maximum stack size for an item.
        /// </summary>
        /// <param name="slot">The slot to check</param>
        /// <param name="requestedAmount">Amount we want to add</param>
        /// <returns>The actual amount that can be added to this slot</returns>
        private int CalculateStackAddAmount(ItemSlot slot, int requestedAmount)
        {
            // Calculate available space in the slot based on item's max stack size
            int spaceAvailable = slot.item.maxStackSize - slot.quantity;
            return Mathf.Min(spaceAvailable, requestedAmount);
        }

        /// <summary>
        /// Attempts to add an item to the inventory.
        /// First tries to stack with existing items, then uses empty slots if needed.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="quantity">The quantity of the item to add</param>
        /// <returns>True if the item was successfully added, false if inventory is full</returns>
        public bool AddItem(ItemDefinition item, int quantity = 1)
        {
            // Validate inputs to prevent errors
            if (item == null || quantity <= 0)
                return false;
            
            int remainingQuantity = quantity;

            // STEP 1: Try to stack with existing items if the item is stackable
            while (remainingQuantity > 0 && item.isStackable)
            {
                // Find a slot that can accommodate some or all of our items
                ItemSlot stackSlot = FindStackableSlot(item, remainingQuantity);
                if (stackSlot == null)
                    break; // No suitable stacking slots found

                // Calculate how many items we can add to this slot
                int addAmount = CalculateStackAddAmount(stackSlot, remainingQuantity);
                stackSlot.quantity += addAmount;
                remainingQuantity -= addAmount;

                // Notify listeners that the slot has changed
                OnSlotChanged?.Invoke(stackSlot);
            }
            
            // STEP 2: If we have remaining items, try to use empty slots
            while (remainingQuantity > 0)
            {
                // Find an empty slot for a new stack
                ItemSlot emptySlot = FindEmptySlot();
                if (emptySlot == null)
                    return false; // Inventory is full, can't add more items

                // Create a new stack in the empty slot
                emptySlot.item = item;
                // Respect the item's max stack size for new stacks
                int addAmount = Mathf.Min(remainingQuantity, item.maxStackSize);
                emptySlot.quantity = addAmount;
                remainingQuantity -= addAmount;

                // Notify listeners that the slot has changed
                OnSlotChanged?.Invoke(emptySlot);
            }
            
            // All items were successfully added
            return true;
        }

        /// <summary>
        /// Attempts to remove an item from the inventory by item ID.
        /// Removes from smallest stacks first to consolidate remaining items.
        /// </summary>
        /// <param name="itemId">ID of the item to remove</param>
        /// <param name="quantity">The quantity to remove</param>
        /// <returns>True if the specified quantity was successfully removed</returns>
        public bool RemoveItem(string itemId, int quantity = 1)
        {
            // Validate inputs to prevent errors
            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
                return false;

            int remainingToRemove = quantity;

            // Find all slots with the specified item, ordered from smallest to largest quantity
            // This helps consolidate items by removing from smallest stacks first
            var slotsWithItem = _slots
                .Where(slot => slot.item?.itemId == itemId)
                .OrderBy(slot => slot.quantity)
                .ToList();

            foreach (var slot in slotsWithItem)
            {
                if (remainingToRemove <= 0)
                    break; // We've removed all we needed
                
                if (slot.quantity <= remainingToRemove)
                {
                    // Remove the entire stack when the slot has fewer items than we need
                    remainingToRemove -= slot.quantity;
                    slot.quantity = 0;
                    slot.item = null; // Clear the reference to mark slot as empty
                }
                else
                {
                    // Remove part of the stack when the slot has more items than we need
                    slot.quantity -= remainingToRemove;
                    remainingToRemove = 0; // We've removed all needed items
                }

                // Notify listeners that the slot has changed
                OnSlotChanged?.Invoke(slot);
            }
            
            // Return true if we were able to remove all items requested
            return remainingToRemove <= 0;
        }
        
        /// <summary>
        /// Removes a specified quantity of items from a specific slot.
        /// </summary>
        /// <param name="slotIndex">Index of the slot to remove from</param>
        /// <param name="quantity">Quantity to remove</param>
        /// <returns>True if successful</returns>
        public bool RemoveItem(int slotIndex, int quantity = 1)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count || quantity <= 0)
            {
                return false;
            }
            
            var slot = _slots[slotIndex];
            if (slot.IsEmpty)
            {
                return false;
            }
            
            if (slot.quantity <= quantity)
            {
                // Remove the entire stack
                slot.quantity = 0;
                slot.item = null;
            }
            else
            {
                // Remove part of the stack
                slot.quantity -= quantity;
            }
            
            OnSlotChanged?.Invoke(slot);
            return true;
        }
        
        /// <summary>
        /// Combines similar item stacks to optimize inventory space.
        /// Useful after removing items or when inventory is fragmented.
        /// </summary>
        public void OptimizeStacks()
        {
            // Process only non-empty slots with stackable items
            foreach (var slot in _slots.Where(s => !s.IsEmpty && s.item.isStackable).ToList())
            {
                // Continue filling this slot until it's at max capacity
                while (slot.quantity < slot.item.maxStackSize)
                {
                    // Find another slot with the same item that's not full
                    var otherSlot = _slots.FirstOrDefault(s => 
                        s != slot && // Not the same slot
                        !s.IsEmpty && // Has items
                        s.item == slot.item && // Same item
                        s.quantity < s.item.maxStackSize); // Not at max capacity
                    
                    if (otherSlot == null)
                        break; // No more slots with this item found
                    
                    // Calculate how many items we can transfer
                    int transferAmount = Mathf.Min(
                        slot.item.maxStackSize - slot.quantity, // Space available in the target slot
                        otherSlot.quantity); // Quantity in the source slot

                    // Move items between slots
                    slot.quantity += transferAmount;
                    otherSlot.quantity -= transferAmount;

                    // Clear the source slot if it's now empty
                    if (otherSlot.quantity <= 0)
                    {
                        otherSlot.item = null;
                        otherSlot.quantity = 0;
                    }

                    // Notify listeners that both slots have changed
                    OnSlotChanged?.Invoke(slot);
                    OnSlotChanged?.Invoke(otherSlot);
                }
            }
        }
        
        /// <summary>
        /// Moves an item from one slot to another.
        /// If the destination slot is empty, the item is moved completely.
        /// If the destination contains the same item, stacking will be attempted.
        /// </summary>
        /// <param name="fromSlotIndex">Source slot index</param>
        /// <param name="toSlotIndex">Destination slot index</param>
        /// <returns>True if successful</returns>
        public bool MoveItem(int fromSlotIndex, int toSlotIndex)
        {
            // Validate indices
            if (fromSlotIndex < 0 || fromSlotIndex >= _slots.Count ||
                toSlotIndex < 0 || toSlotIndex >= _slots.Count ||
                fromSlotIndex == toSlotIndex)
            {
                return false;
            }
            
            // Get references to the slots
            ItemSlot fromSlot = _slots[fromSlotIndex];
            ItemSlot toSlot = _slots[toSlotIndex];
            
            // Can't move from an empty slot
            if (fromSlot.IsEmpty)
            {
                return false;
            }
            
            // Case 1: Destination slot is empty - simple move
            if (toSlot.IsEmpty)
            {
                toSlot.item = fromSlot.item;
                toSlot.quantity = fromSlot.quantity;
                
                fromSlot.item = null;
                fromSlot.quantity = 0;
                
                OnSlotChanged?.Invoke(fromSlot);
                OnSlotChanged?.Invoke(toSlot);
                return true;
            }
            
            // Case 2: Same item type - try to stack
            if (fromSlot.item == toSlot.item && toSlot.item.isStackable)
            {
                int spaceInToSlot = toSlot.item.maxStackSize - toSlot.quantity;
                
                if (spaceInToSlot <= 0)
                {
                    return false; // Destination slot is full
                }
                
                int amountToMove = Mathf.Min(spaceInToSlot, fromSlot.quantity);
                
                toSlot.quantity += amountToMove;
                fromSlot.quantity -= amountToMove;
                
                // Clear the from slot if it's now empty
                if (fromSlot.quantity <= 0)
                {
                    fromSlot.item = null;
                    fromSlot.quantity = 0;
                }
                
                OnSlotChanged?.Invoke(fromSlot);
                OnSlotChanged?.Invoke(toSlot);
                return true;
            }
            
            // Case 3: Different items - swap positions
            ItemDefinition tempItem = toSlot.item;
            int tempQuantity = toSlot.quantity;
            
            toSlot.item = fromSlot.item;
            toSlot.quantity = fromSlot.quantity;
            
            fromSlot.item = tempItem;
            fromSlot.quantity = tempQuantity;
            
            OnSlotChanged?.Invoke(fromSlot);
            OnSlotChanged?.Invoke(toSlot);
            return true;
        }
        
        /// <summary>
        /// Splits a stack of items into two separate stacks.
        /// Useful for dividing resources or preparing items for trading.
        /// </summary>
        /// <param name="slotIndex">Index of the slot containing the stack to split</param>
        /// <param name="splitAmount">Amount to move to a new stack</param>
        /// <returns>True if the stack was successfully split</returns>
        public bool SplitStack(int slotIndex, int splitAmount)
        {
            // Validate the slot index
            if (slotIndex < 0 || slotIndex >= _slots.Count)
                return false;
            
            // Get the source slot and validate it contains enough items
            var sourceSlot = _slots[slotIndex];
            if (sourceSlot.IsEmpty || splitAmount >= sourceSlot.quantity)
                return false; // Can't split an empty slot or take all items (use move instead)
            
            // Find an empty slot for the new stack
            var emptySlot = FindEmptySlot();
            if (emptySlot == null)
                return false; // No empty slot available

            // Create the new stack in the empty slot
            emptySlot.item = sourceSlot.item;
            emptySlot.quantity = splitAmount;
            sourceSlot.quantity -= splitAmount;

            // Notify listeners that both slots have changed
            OnSlotChanged?.Invoke(sourceSlot);
            OnSlotChanged?.Invoke(emptySlot);
            return true;
        }
    }
}
