using UnityEngine;

namespace BetterWYD.Inventory
{
    /// <summary>
    /// Represents a single slot in the inventory, containing an item and its quantity.
    /// </summary>
    [System.Serializable]
    public class ItemSlot
    {
        /// <summary>
        /// Reference to item definition.
        /// </summary>
        public ItemDefinition item;

        /// <summary>
        /// Current quantity of the item in this slot.
        /// </summary>
        public int quantity;

        /// <summary>
        /// Checks if the slot is empty (i.e., contains no item).
        /// </summary>
        public bool IsEmpty => item == null || quantity <= 0;
    }
    
}