using UnityEngine;
using System.Collections.Generic;

namespace BetterWYD.Inventory
{
    /// <summary>
    /// Manages the loading and access of all item definitions in the game.
    /// Implements the Singleton pattern to ensure only one instance exists.
    /// </summary>
    public class ItemDatabase : MonoBehaviour
    {
        private static ItemDatabase instance;

        /// <summary>
        /// Singleton instance of the ItemDatabase.
        /// </summary>
        public static ItemDatabase Instance => instance;

        // Dictionary to hold item definitions, using item ID as the key.
        private Dictionary<string, ItemDefinition> itemDictionary = new Dictionary<string, ItemDefinition>();

        private void Awake()
        {
            // Ensure only one instance of ItemDatabase exists.
            if (instance == null)
            {
                instance = this;
                LoadItems();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Loads all item definitions from the Resources folder into the dictionary.
        /// </summary>
        private void LoadItems()
        {
            // Load all ItemDefinition assets from the Resources folder.
            ItemDefinition[] loadedItems = Resources.LoadAll<ItemDefinition>("Items");
            foreach (ItemDefinition item in loadedItems)
            {
                itemDictionary[item.itemId] = item;
            }
        }

        /// <summary>
        /// Retrieves an item definition by its ID.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item.</param>
        /// <returns>The ItemDefinition associated with the given ID, or null if not found.</returns>
        public ItemDefinition GetItem(string itemId)
        {
            itemDictionary.TryGetValue(itemId, out ItemDefinition item);
            return item;
        }
    }
}