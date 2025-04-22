using UnityEngine;

namespace BetterWYD.Inventory
{
    /// <summary>
    /// Defines item types available in the game
    /// </summary>
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Consumable,
        Material,
        Quest
    }

    /// <summary>
    /// Defines possible rarity levels for items
    /// </summary>
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// ScriptableObject that defines the base properties of an item in the game.
    /// Used as a template for creating new items in the Unity Editor.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "BetterWYD/Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Tooltip("Unique identifier for the item")]
        public string itemId;

        [Tooltip("Name displayed in the UI")]
        public string displayName;

        [TextArea(3, 5)]
        [Tooltip("Detailed description of the item")]
        public string description;

        [Tooltip("Category of the item")]
        public ItemType type;

        [Tooltip("Rarity level of the item")]
        public ItemRarity rarity;

        [Tooltip("Icon displayed in UI elements")]
        public Sprite icon;

        [Tooltip("3D model prefab for the item in the game world")]
        public GameObject prefab;

        [Tooltip("Maximum number of items that can be stacked")]
        public int maxStackSize = 1;

        [Tooltip("Whether this item can be stacked")]
        public bool isStackable;

        [Tooltip("Base economic value of the item")]
        public int baseValue;
    }
}