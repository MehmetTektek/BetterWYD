# BetterWYD Technical Design Document

## Introduction

This technical design document outlines the architecture, systems, and implementation details for BetterWYD, a modern remake of the classic MMORPG "With Your Destiny" (WYD) using Unity.

## Table of Contents

1. [System Architecture](#system-architecture)
2. [Core Gameplay Systems](#core-gameplay-systems)
3. [User Interface](#user-interface)
4. [Networking](#networking)
5. [Data Management](#data-management)
6. [Technical Requirements](#technical-requirements)

## System Architecture

### Overview

BetterWYD will use a modular, component-based architecture following SOLID principles to ensure maintainability and extensibility. The game will be built using Unity 2022.3 LTS or newer.

### Core Architectural Components

```
[Client Application]
    ├── [Game Core]
    │   ├── GameManager
    │   ├── SceneManager
    │   └── EventSystem
    │
    ├── [Player]
    │   ├── Character Controller
    │   ├── Input System
    │   ├── Camera Controller
    │   └── Player State
    │
    ├── [Game Systems]
    │   ├── Inventory System
    │   ├── Combat System
    │   ├── Skills & Abilities
    │   ├── Progression System
    │   └── Quest System
    │
    ├── [World]
    │   ├── World Manager
    │   ├── Environment System
    │   ├── NPC System
    │   └── Interaction System
    │
    ├── [UI]
    │   ├── UI Manager
    │   ├── HUD
    │   ├── Menus
    │   └── Dialog System
    │
    └── [Networking]
        ├── Network Manager
        ├── Server Communication
        ├── Player Synchronization
        └── World State
```

### Design Patterns

The following design patterns will be utilized:

- **Singleton**: For manager classes (GameManager, UIManager, etc.)
- **Observer Pattern**: For event handling and communication between systems
- **State Pattern**: For character states, game states, and UI states
- **Factory Pattern**: For object creation (items, enemies, etc.)
- **Command Pattern**: For input handling and action execution
- **Dependency Injection**: For loose coupling between systems

## Core Gameplay Systems

### Character System

#### Character Classes

Each character class will inherit from a base `CharacterClass` class:

```csharp
public abstract class CharacterClass : MonoBehaviour
{
    public string ClassName { get; protected set; }
    public List<Skill> ClassSkills { get; protected set; }
    public Dictionary<AttributeType, float> AttributeModifiers { get; protected set; }
    
    public abstract void InitializeClass();
    public abstract void ApplyClassBonuses(Character character);
    public abstract void PerformSpecialAbility();
}
```

The four classes (Transknight, Hunter, Foema, Beastmaster) will extend this base class and implement their unique abilities and attributes.

#### Character Stats and Attributes

```csharp
public enum AttributeType
{
    Strength,
    Dexterity,
    Intelligence,
    Constitution
}

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    [SerializeField] private int _experiencePoints = 0;
    [SerializeField] private int _experienceToNextLevel = 100;
    
    [SerializeField] private Dictionary<AttributeType, int> _baseAttributes;
    [SerializeField] private Dictionary<AttributeType, int> _bonusAttributes;
    
    // Derived stats
    [SerializeField] private int _health;
    [SerializeField] private int _mana;
    [SerializeField] private int _attackPower;
    [SerializeField] private int _defensePower;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _movementSpeed;
    
    // Methods for attribute manipulation and stat calculation
}
```

### Inventory System

The inventory system will manage items, equipment, and player inventory:

```csharp
public enum ItemType
{
    Weapon,
    Armor,
    Accessory,
    Consumable,
    Material,
    Quest
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Item : ScriptableObject
{
    public string ItemId;
    public string Name;
    public string Description;
    public ItemType Type;
    public ItemRarity Rarity;
    public Sprite Icon;
    public GameObject Model;
    public int MaxStackSize;
    public bool IsStackable;
    public int SellValue;
    
    public virtual void Use(Character character);
}

public class Inventory : MonoBehaviour
{
    public int MaxSlots = 30;
    public List<ItemSlot> Slots = new List<ItemSlot>();
    
    // Methods for adding, removing, using items
}
```

### Combat System

The combat system will handle attacks, skills, damage calculation, and combat states:

```csharp
public enum DamageType
{
    Physical,
    Magical,
    True
}

public class CombatSystem : MonoBehaviour
{
    public float AttackRange = 2f;
    public float AttackCooldown = 1.5f;
    
    private Character _character;
    private bool _isAttacking;
    private float _lastAttackTime;
    
    // Methods for initiating attacks, calculating damage, etc.
    
    public void Attack(Character target)
    {
        // Attack logic
    }
    
    public void UseSkill(Skill skill, Vector3 targetPosition, Character targetCharacter = null)
    {
        // Skill usage logic
    }
    
    public void TakeDamage(int amount, DamageType damageType, Character attacker)
    {
        // Damage receiving logic
    }
}
```

### Skill System

Skills will be implemented as ScriptableObjects for easy creation and modification:

```csharp
public enum SkillType
{
    Active,
    Passive,
    Ultimate
}

public enum SkillTargetType
{
    Self,
    SingleTarget,
    AOE,
    ProjectileLinear,
    ProjectileArc
}

public class Skill : ScriptableObject
{
    public string SkillId;
    public string Name;
    public string Description;
    public SkillType Type;
    public SkillTargetType TargetType;
    public Sprite Icon;
    public GameObject VisualEffect;
    public float Cooldown;
    public int ManaCost;
    public int BaseDamage;
    public float Range;
    public float AreaOfEffect;
    
    public virtual void Execute(Character caster, Vector3 targetPosition, Character targetCharacter = null);
}
```

## User Interface

### UI Architecture

The UI will be built using Unity's Canvas system with a modular approach:

```
[UI System]
    ├── UI Manager (handles showing/hiding UI elements, UI navigation)
    ├── HUD
    │   ├── Health/Mana bars
    │   ├── Mini-map
    │   ├── Action bar
    │   ├── Target information
    │   └── Status effects
    │
    ├── Menus
    │   ├── Main Menu
    │   ├── Character Screen
    │   ├── Inventory
    │   ├── Skills
    │   ├── Map
    │   ├── Quest Log
    │   └── Settings
    │
    └── Dialog System
        ├── NPC Dialog
        ├── System Messages
        ├── Chat System
        └── Tooltips
```

### UI Implementation

The UI system will use a base UI panel class:

```csharp
public abstract class UIPanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    
    public bool IsVisible { get; protected set; }
    
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
    }
    
    public virtual void Show()
    {
        // Show panel logic
    }
    
    public virtual void Hide()
    {
        // Hide panel logic
    }
    
    public virtual void Toggle()
    {
        if (IsVisible)
            Hide();
        else
            Show();
    }
}
```

Individual UI panels will inherit from this base class and implement their specific functionality.

## Networking

### Overview

The networking system will use a client-server architecture with Unity's Netcode for GameObjects as the primary networking solution.

### Key Components

1. **NetworkManager**: Handles connections, player spawning, and network state
2. **Player Synchronization**: Syncs player movements, actions, and states
3. **World State Synchronization**: Syncs NPCs, enemies, and interactive objects
4. **Network Data Structure**: Defines network messages and state representations

### Implementation Approach

For the initial prototype, we'll implement a simplified networking system focusing on:

1. Player movement synchronization
2. Basic combat interactions
3. Chat functionality
4. Player presence in the world

As the project progresses, we'll expand the networking capabilities to include:

1. Guild systems
2. Trading
3. Group/party functionality
4. PvP and large-scale battles
5. Instanced dungeons

## Data Management

### Data Storage

Player and game data will be stored using:

1. **Local Storage**: PlayerPrefs for settings and preferences
2. **Serialized Data**: JSON for local game state
3. **Server Database**: SQL database for persistent player data (future implementation)

### Data Classes

```csharp
[System.Serializable]
public class PlayerData
{
    public string PlayerId;
    public string PlayerName;
    public int Level;
    public int Experience;
    public string CharacterClass;
    public Dictionary<string, int> Attributes;
    public List<EquippedItem> Equipment;
    public List<InventoryItem> Inventory;
    public List<string> UnlockedSkills;
    public Vector3 LastPosition;
    // Additional player data
}

[System.Serializable]
public class GameSettings
{
    public float MusicVolume;
    public float SfxVolume;
    public bool FullScreen;
    public int ResolutionIndex;
    public int QualityLevel;
    // Additional settings
}
```

### Data Management System

```csharp
public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance => _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SavePlayerData(PlayerData data)
    {
        // Serialize and save player data
    }
    
    public PlayerData LoadPlayerData(string playerId)
    {
        // Load and deserialize player data
        return null;
    }
    
    public void SaveGameSettings(GameSettings settings)
    {
        // Save game settings
    }
    
    public GameSettings LoadGameSettings()
    {
        // Load game settings
        return null;
    }
}
```

## Technical Requirements

### Minimum System Requirements

- **OS**: Windows 10, macOS 10.15, or Linux
- **CPU**: Intel Core i5-4460 or AMD FX-6300
- **RAM**: 8 GB
- **GPU**: NVIDIA GeForce GTX 760 or AMD Radeon R7 260x
- **Storage**: 10 GB available space
- **DirectX**: Version 11
- **Network**: Broadband Internet connection

### Target Performance

- **Frame Rate**: 60 FPS minimum on recommended hardware
- **Resolution**: 1920 x 1080 (Full HD)
- **Network Latency**: < 100ms for smooth gameplay

### Development Environment

- **Engine**: Unity 2022.3 LTS or newer
- **Version Control**: Git with GitHub
- **Asset Pipeline**: Standard Unity Asset Pipeline with addressables
- **Build Pipeline**: Unity Cloud Build (future implementation)
- **Testing Framework**: Unity Test Framework for automated testing

## Conclusion

This technical design document provides a foundation for the development of BetterWYD. As development progresses, this document will be updated to reflect changes and additions to the game's systems and architecture.