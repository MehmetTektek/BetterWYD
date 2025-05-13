using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BetterWYD.Systems.Inventory
{
    /// <summary>
    /// This class is used to test the inventory system.
    /// </summary>
    public class InventoryTester : MonoBehaviour
    {
        /// <summary>
        /// Serialized Field for the inventory system.
        /// </summary>
        [Header("System References")]
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private ItemDatabase itemDatabase;

        [Header("Test Items")]
        [SerializeField] private List<ItemDefinition> testItems = new List<ItemDefinition>();

        [Header("UI References")]
        [SerializeField] private Transform slotGrid;
        [SerializeField] private TMP_Text resultsText;

        [Header("Button References")]
        [SerializeField] private Button addItemButton;
        [SerializeField] private Button removeItemButton;
        [SerializeField] private Button optimizeButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button runTestsButton;

        // Track UI objects inventory slots
        private List<GameObject> slotObjects = new List<GameObject>();
        private List<Image> slotIcons = new List<Image>();
        private List<TMP_Text> slotCounts = new List<TMP_Text>();

        /// <summary>
        /// Initializes the inventory tester.
        /// </summary>
        private void Start()
        {
            // Verify we have correct components
            if (!VerifyReferences())
                return;

            // Cache references to slot UI elements
            CacheSlotReferences();

            // Subscribe to inventory events
            SubscribeToEvents();

            // Connect button events
            ConnectButtonEvents();

            // Initialize UI update
            UpdateAllSlots();

            LogResults("Inventory Tester Initialized");
        }

        /// <summary>
        /// Cleanup the inventory tester.
        /// </summary>
        private void OnDestroy()
        {
            // Cleanup event subscriptions
            UnsubscribeFromEvents();

            // Remove button listeners
            RemoveButtonListeners();
        }

        /// <summary>
        /// Verifies that all references are set correctly.
        /// </summary>
        private bool VerifyReferences()
        {
            bool valid = true;

            if (inventoryManager == null)
            {
                Debug.LogError("Inventory Manager is not set.");
                valid = false;
            }

            if (itemDatabase == null)
            {
                Debug.LogError("Item Database is not set.");
                valid = false;
            }

            if (slotGrid == null)
            {
                Debug.LogError("Slot Grid is not set.");
                valid = false;
            }
            if (resultsText == null)
            {
                Debug.LogError("Results Text is not set.");
                valid = false;
            }

            // Return true if all references are valid
            return valid;
        }

        /// <summary>
        /// Caches references to the slot UI elements.
        /// </summary>
        private void CacheSlotReferences()
        {
            slotObjects.Clear();
            slotIcons.Clear();
            slotCounts.Clear();

            // Find all slot objects in the slot grid
            for (int i = 0; i < slotGrid.childCount; i++)
            {
                Transform slotTransform = slotGrid.GetChild(i);
                slotObjects.Add(slotTransform.gameObject);

                // Find the icon and count text components
                Image icon = slotTransform.Find("ItemIcon").GetComponent<Image>();
                slotIcons.Add(icon);

                TMP_Text count = slotTransform.Find("ItemCount").GetComponent<TMP_Text>();
                slotCounts.Add(count);
            }
            // Log the number of slots found
            Debug.Log($"Found {slotObjects.Count} slots in the slot grid.");
        }

        /// <summary>
        /// Subscribes to inventory events.
        /// </summary>
        private void SubscribeToEvents()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnSlotChanged += HandleSlotChanged;
                inventoryManager.OnInventoryChanged += HandleInventoryChanged;
            }
        }

        /// <summary>
        /// Unsubscribes from inventory events.
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnSlotChanged -= HandleSlotChanged;
                inventoryManager.OnInventoryChanged -= HandleInventoryChanged;
            }
        }

        /// <summary>
        /// Connects button events to their respective methods.
        /// </summary>
        private void ConnectButtonEvents()
        {
            if (addItemButton != null)
                addItemButton.onClick.AddListener(AddRandomItem);

            if (removeItemButton != null)
                removeItemButton.onClick.AddListener(RemoveRandomItem);
            
            if (optimizeButton != null)
                optimizeButton.onClick.AddListener(OptimizeInventory);
            
            if (clearButton != null)
                clearButton.onClick.AddListener(ClearInventory);

            if (runTestsButton != null)
                runTestsButton.onClick.AddListener(RunTests); 
        }

        /// <summary>
        /// Removes button event listeners.
        /// </summary>
        private void RemoveButtonListeners()
        {
            if (addItemButton != null)
                addItemButton.onClick.RemoveListener(AddRandomItem);

            if (removeItemButton != null)
                removeItemButton.onClick.RemoveListener(RemoveRandomItem);
            
            if (optimizeButton != null)
                optimizeButton.onClick.RemoveListener(OptimizeInventory);
            
            if (clearButton != null)
                clearButton.onClick.RemoveListener(ClearInventory);

            if (runTestsButton != null)
                runTestsButton.onClick.RemoveListener(RunTests); 
        }

        /// <summary>
        /// Updates all inventory slot UI elements.
        /// </summary>
        private void UpdateAllSlots()
        {
            if (inventoryManager == null || slotIcons.Count == 0)
                return;

            for (int i = 0; i < inventoryManager.SlotCount && i < slotIcons.Count; i++)
            {
                UpdateSlotUI(i, inventoryManager.GetSlotItem(i), inventoryManager.GetSlotCount(i));
            }
        }

        /// <summary>
        /// Updates a single inventory slot UI element.
        /// </summary>
        private void UpdateSlotUI(int slotIndex, ItemDefinition item, int count)
        {
            if (slotIndex < 0 || slotIndex >= slotIcons.Count)
                return;

            if (item == null || count <= 0)
            {
                // Empty slot
                slotIcons[slotIndex].sprite = null;
                slotIcons[slotIndex].enabled = false;
                slotCounts[slotIndex].text = string.Empty;
            }
            else
            {
                // Filled slot
                slotIcons[slotIndex].sprite = item.Icon;
                slotIcons[slotIndex].enabled = true;
                slotCounts[slotIndex].text = count.ToString();
            }
        }

        /// <summary>
        /// Logs results to the UI.
        /// </summary>
        private void LogResults(string message)
        {
            if (resultsText != null)
            {
                resultsText.text = message;
                Debug.Log(message);
            }
        }

        /// <summary>
        /// Handles slot changed events from the inventory system.
        /// </summary>
        private void HandleSlotChanged(int slotIndex, ItemDefinition item, int count)
        {
            UpdateSlotUI(slotIndex, item, count);
        }

        /// <summary>
        /// Handles inventory changed events from the inventory system.
        /// </summary>
        private void HandleInventoryChanged()
        {
            UpdateAllSlots();
            LogResults("Inventory Updated");
        }

        /// <summary>
        /// Adds a random item to the inventory.
        /// </summary>
        private void AddRandomItem()
        {
            if (inventoryManager == null || testItems.Count == 0)
            {
                LogResults("Cannot add item: Missing inventory manager or test items");
                return;
            }

            // Select a random item from the test items
            int randomIndex = Random.Range(0, testItems.Count);
            ItemDefinition randomItem = testItems[randomIndex];
            
            // Add it to the inventory
            bool added = inventoryManager.AddItem(randomItem, 1);
            
            if (added)
                LogResults($"Added: {randomItem.ItemName}");
            else
                LogResults($"Failed to add: {randomItem.ItemName} (Inventory full?)");
        }

        /// <summary>
        /// Removes a random item from the inventory.
        /// </summary>
        private void RemoveRandomItem()
        {
            if (inventoryManager == null)
            {
                LogResults("Cannot remove item: Missing inventory manager");
                return;
            }

            // Find a non-empty slot
            List<int> nonEmptySlots = new List<int>();
            for (int i = 0; i < inventoryManager.SlotCount; i++)
            {
                if (inventoryManager.GetSlotItem(i) != null)
                    nonEmptySlots.Add(i);
            }

            if (nonEmptySlots.Count == 0)
            {
                LogResults("No items to remove");
                return;
            }

            // Select a random non-empty slot
            int randomSlotIndex = nonEmptySlots[Random.Range(0, nonEmptySlots.Count)];
            ItemDefinition item = inventoryManager.GetSlotItem(randomSlotIndex);
            
            // Remove the item
            bool removed = inventoryManager.RemoveItemAt(randomSlotIndex, 1);
            
            if (removed && item != null)
                LogResults($"Removed: {item.ItemName}");
            else
                LogResults("Failed to remove item");
        }

        /// <summary>
        /// Optimizes the inventory by stacking and sorting items.
        /// </summary>
        private void OptimizeInventory()
        {
            if (inventoryManager == null)
            {
                LogResults("Cannot optimize: Missing inventory manager");
                return;
            }

            inventoryManager.OptimizeInventory();
            LogResults("Inventory optimized");
        }

        /// <summary>
        /// Clears all items from the inventory.
        /// </summary>
        private void ClearInventory()
        {
            if (inventoryManager == null)
            {
                LogResults("Cannot clear: Missing inventory manager");
                return;
            }

            inventoryManager.ClearInventory();
            LogResults("Inventory cleared");
        }

        /// <summary>
        /// Runs a series of inventory tests.
        /// </summary>
        private void RunTests()
        {
            if (inventoryManager == null || testItems.Count == 0)
            {
                LogResults("Cannot run tests: Missing inventory manager or test items");
                return;
            }

            StartCoroutine(RunTestSequence());
        }

        /// <summary>
        /// Runs a sequence of inventory tests.
        /// </summary>
        private System.Collections.IEnumerator RunTestSequence()
        {
            LogResults("Starting Test Sequence...");
            yield return new WaitForSeconds(0.5f);

            // Test 1: Clear inventory
            ClearInventory();
            yield return new WaitForSeconds(0.5f);

            // Test 2: Add items until full
            int addCount = 0;
            for (int i = 0; i < 20; i++)
            {
                ItemDefinition testItem = testItems[Random.Range(0, testItems.Count)];
                if (inventoryManager.AddItem(testItem, 1))
                    addCount++;
                
                yield return new WaitForSeconds(0.1f);
            }
            LogResults($"Added {addCount} items until capacity reached");
            yield return new WaitForSeconds(0.5f);

            // Test 3: Optimize
            OptimizeInventory();
            yield return new WaitForSeconds(0.5f);

            // Test 4: Remove half the items
            int removeCount = Mathf.FloorToInt(addCount / 2f);
            for (int i = 0; i < removeCount; i++)
            {
                RemoveRandomItem();
                yield return new WaitForSeconds(0.1f);
            }
            
            LogResults("Test Sequence Completed");
        }
    }
}