# BetterWYD Architecture Overview

This document provides a high-level overview of the BetterWYD architecture, complementing the detailed [Technical Design Document](../TechnicalDesignDocument.md).

## System Architecture Diagram

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

## Key Components

### Game Core

The Game Core comprises the central systems that manage the overall game state and lifecycle:

- **GameManager**: Singleton managing game state, scene transitions, and high-level game flow
- **SceneManager**: Controls scene loading, unloading, and transitions
- **EventSystem**: Implements the Observer pattern for communication between game systems

### Player

The Player systems handle all player-related functionality:

- **Character Controller**: Manages movement, physics, and character positioning
- **Input System**: Processes player input from various input devices
- **Camera Controller**: Manages camera movement, positioning, and effects
- **Player State**: Handles player state machine (idle, moving, attacking, etc.)

### Game Systems

Game Systems contain the core gameplay mechanics:

- **Inventory System**: Manages items, equipment, and player inventory
- **Combat System**: Handles attacks, skills, damage calculation, and combat states
- **Skills & Abilities**: Manages character skills, cooldowns, and effects
- **Progression System**: Controls character progression, levels, and attributes
- **Quest System**: Manages quest tracking, objectives, and rewards

### World

World systems manage the game environment:

- **World Manager**: Controls world generation, loading, and management
- **Environment System**: Handles environmental effects, day/night cycle, weather
- **NPC System**: Manages non-player characters, AI, and behavior
- **Interaction System**: Controls how players interact with the world and objects

### UI

UI systems handle all user interface elements:

- **UI Manager**: Core singleton managing UI state and navigation
- **HUD**: In-game heads-up display showing player stats and information
- **Menus**: Game menus for inventory, character, settings, etc.
- **Dialog System**: Handles NPC conversations and information presentation

### Networking

Networking systems manage multiplayer functionality:

- **Network Manager**: Core networking singleton handling connections and sessions
- **Server Communication**: Manages client-server communications
- **Player Synchronization**: Synchronizes player positions, actions, and states
- **World State**: Handles synchronization of the game world between clients

## Communication Patterns

Systems communicate primarily through:

1. **Direct References**: For tightly coupled components
2. **Event System**: For loosely coupled systems using the Observer pattern
3. **Scriptable Objects**: For data-driven communication between systems
4. **Command Pattern**: For input handling and action execution

## Data Flow

```
[Input] → [Input System] → [Command] → [Game Systems] → [State Change] → [UI Update]
                                     → [Network Sync]
```

## Dependencies

The system is designed to minimize dependencies between components. Key dependencies include:

- Game systems depend on the Event System for communication
- UI systems depend on Game Systems for data
- Player systems depend on Input and Game Systems
- All systems depend on the Game Manager for high-level flow

## Future Expansion

The architecture is designed to be modular and extensible, allowing for:

- Addition of new game systems without modifying existing ones
- Replacement of specific components with minimal impact on others
- Scaling of the network system for larger player counts
- Integration with additional services and platforms