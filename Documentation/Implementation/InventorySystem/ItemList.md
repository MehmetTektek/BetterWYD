# BetterWYD Item List

**Version:** 1.0  
**Date:** 22 Nisan 2025  
**Author:** BetterWYD Development Team  
**Last Updated By:** GitHub Copilot  

## Table of Contents

1. [Item ID Naming Convention](#item-id-naming-convention)
2. [Consumables](#1-consumables)
3. [Equipment](#2-equipment)
   - [Weapons](#21-weapons)
   - [Armor](#22-armor)
   - [Accessories](#23-accessories)
4. [Quest Items](#3-quest-items)
5. [Materials](#4-materials)
6. [Currency](#5-currency)
7. [Miscellaneous](#6-junk--miscellaneous)

## Item ID Naming Convention

Each item in BetterWYD follows a consistent naming convention to ensure clarity and organization:

`[TYPE]_[Subtype]_[Name]_[Tier/Variant]`

- **TYPE**: Primary category (CON=Consumable, EQP=Equipment, QST=Quest, MAT=Material, CUR=Currency, JNK=Junk)
- **Subtype**: Further categorization (e.g., WPN=Weapon, ARM=Armor, ACC=Accessory for Equipment items)
- **Name**: Brief identifier for the specific item
- **Tier/Variant**: Numerical or letter indicating quality/version (1, 2, 3 or A, B, C)

## 1. Consumables

Items that are used up upon activation.

| Item ID          | Name             | Description                       | Max Stack | Effect                     |
| :--------------- | :--------------- | :-------------------------------- | :-------- | :------------------------- |
| `CON_Potion_HP1` | Minor Health Potion | Restores a small amount of health. | 20        | Restore 50 HP            |
| `CON_Potion_MP1` | Minor Mana Potion   | Restores a small amount of mana.  | 20        | Restore 30 MP            |
| `CON_Food_Bread` | Hardtack Bread   | Basic sustenance.                 | 10        | Restore 10 HP over 5s    |
| `CON_Potion_HP2` | Health Potion    | Restores a moderate amount of health. | 20    | Restore 100 HP           |
| `CON_Potion_MP2` | Mana Potion      | Restores a moderate amount of mana.   | 20    | Restore 70 MP            |
| `CON_Scroll_TP1` | Scroll of Return | Teleports player to nearest town.     | 5     | Teleport to Town         |
| `CON_Buff_STR1`  | Might Elixir     | Temporarily increases strength.       | 5     | +10 STR for 5 minutes    |
| `CON_Buff_INT1`  | Wisdom Elixir    | Temporarily increases intelligence.   | 5     | +10 INT for 5 minutes    |

## 2. Equipment

Items that can be equipped by the player character.

### 2.1. Weapons

| Item ID             | Name            | Description                     | Type   | Equip Slot | Stats Modifiers        | Class Restriction | Rarity   |
| :------------------ | :-------------- | :------------------------------ | :----- | :--------- | :--------------------- | :---------------- | :------- |
| `EQP_WPN_Sword1H_1` | Rusty Shortsword | A basic, worn-out shortsword. | Sword  | Main Hand  | +5 Attack Power        | Transknight       | Common   |
| `EQP_WPN_Bow_1`     | Simple Shortbow | A crudely made shortbow.        | Bow    | Main Hand  | +4 Attack Power, +2 Dex | Hunter            | Common   |
| `EQP_WPN_Staff_1`   | Apprentice Staff| A simple wooden staff.          | Staff  | Main Hand  | +5 Magic Power         | Foema             | Common   |
| `EQP_WPN_Axe1H_1`   | Crude Handaxe   | A basic axe for combat.         | Axe    | Main Hand  | +6 Attack Power        | Beastmaster       | Common   |
| `EQP_WPN_Sword2H_1` | Heavy Iron Blade| A two-handed iron sword.        | Sword  | Two Hands  | +10 Attack Power       | Transknight       | Common   |
| `EQP_WPN_Dagger_1`  | Rusty Dagger    | A small, rusty blade.           | Dagger | Main Hand  | +3 Attack Power, +1 Dex | Hunter            | Common   |
| `EQP_WPN_Wand_1`    | Novice Wand     | A basic magical implement.      | Wand   | Main Hand  | +4 Magic Power, +1 Int  | Foema             | Common   |

### 2.2. Armor

| Item ID           | Name            | Description                     | Type  | Equip Slot | Stats Modifiers       | Class Restriction | Rarity   |
| :---------------- | :-------------- | :------------------------------ | :---- | :--------- | :-------------------- | :---------------- | :------- |
| `EQP_ARM_Chest_L1`| Leather Vest    | Simple leather protection.      | Light | Chest      | +10 Defense, +1 Dex   | Hunter, Foema     | Common   |
| `EQP_ARM_Chest_H1`| Iron Chainmail  | Basic chainmail armor.          | Heavy | Chest      | +20 Defense, +2 Str   | Transknight       | Common   |
| `EQP_ARM_Head_L1` | Leather Cap     | A simple leather cap.           | Light | Head       | +5 Defense            | All               | Common   |
| `EQP_ARM_Head_H1` | Iron Helm       | Basic protective headgear.      | Heavy | Head       | +10 Defense           | Transknight       | Common   |
| `EQP_ARM_Legs_L1` | Leather Pants   | Simple leather leg protection.  | Light | Legs       | +8 Defense, +1 Dex    | Hunter, Foema     | Common   |
| `EQP_ARM_Legs_H1` | Iron Greaves    | Heavy leg protection.           | Heavy | Legs       | +15 Defense           | Transknight       | Common   |
| `EQP_ARM_Hands_L1`| Leather Gloves  | Simple hand protection.         | Light | Hands      | +3 Defense, +1 Dex    | Hunter, Foema     | Common   |
| `EQP_ARM_Hands_H1`| Iron Gauntlets  | Heavy hand protection.          | Heavy | Hands      | +8 Defense            | Transknight       | Common   |
| `EQP_ARM_Feet_L1` | Leather Boots   | Simple footwear.                | Light | Feet       | +5 Defense, +1 Spd    | Hunter, Foema     | Common   |
| `EQP_ARM_Feet_H1` | Iron Boots      | Heavy foot protection.          | Heavy | Feet       | +10 Defense           | Transknight       | Common   |

### 2.3. Accessories

| Item ID             | Name            | Description                     | Type    | Equip Slot | Stats Modifiers       | Class Restriction | Rarity   |
| :------------------ | :-------------- | :------------------------------ | :------ | :--------- | :-------------------- | :---------------- | :------- |
| `EQP_ACC_Ring_1`    | Simple Iron Ring| A plain iron ring.              | Ring    | Finger     | +1 Constitution       | All               | Common   |
| `EQP_ACC_Amulet_1`  | Faded Amulet    | An old amulet, its power weak.  | Amulet  | Neck       | +1 Intelligence       | All               | Common   |
| `EQP_ACC_Ring_2`    | Silver Band     | A simple silver ring.           | Ring    | Finger     | +2 Intelligence       | All               | Uncommon |
| `EQP_ACC_Amulet_2`  | Amber Pendant   | A pendant with amber stone.     | Amulet  | Neck       | +2 Dexterity          | All               | Uncommon |
| `EQP_ACC_Earring_1` | Copper Stud     | A simple earring.               | Earring | Ear        | +1 Dexterity          | All               | Common   |
| `EQP_ACC_Belt_1`    | Leather Belt    | A sturdy leather belt.          | Belt    | Waist      | +2 Constitution       | All               | Common   |

## 3. Quest Items

Items required for completing quests. Usually non-droppable/non-sellable.

| Item ID           | Name                | Description                             | Related Quest(s) | Location Found |
| :---------------- | :------------------ | :-------------------------------------- | :--------------- | :------------- |
| `QST_Key_Jail`    | Rusty Jail Key      | Opens the cell in the starting dungeon. | "Escape!"        | Abandoned Jail |
| `QST_Herb_Moon`   | Moonpetal Herb      | A rare herb needed by the alchemist.    | "Alchemist's Aid"| Moonlight Grove|
| `QST_Emblem_Fire` | Fire Temple Emblem  | Proof of completing the Fire Temple.    | "Temple Trials"  | Fire Temple    |
| `QST_Letter_Mayor`| Mayor's Letter      | Important document for the castle guard.| "Urgent Message" | Town Hall      |
| `QST_Relic_Ancient`| Ancient Relic      | A mysterious artifact of unknown power. | "Lost Relics"    | Ancient Ruins  |
| `QST_Map_Treasure`| Torn Treasure Map   | A map showing a hidden treasure location.| "Buried Secrets"| Pirate Cove    |

## 4. Materials

Items used for crafting or other systems.

| Item ID           | Name          | Description                     | Max Stack | Source(s)        | Used In Crafting |
| :---------------- | :------------ | :------------------------------ | :-------- | :--------------- | :--------------- |
| `MAT_Ore_Iron`    | Iron Ore      | Raw iron, needs smelting.       | 100       | Mining Nodes     | Weapons, Armor   |
| `MAT_Hide_Wolf`   | Wolf Pelt     | Hide from a common wolf.        | 50        | Wolves           | Light Armor      |
| `MAT_Herb_Verdia` | Verdia Leaf   | A common medicinal herb.        | 100       | Herbalism Nodes  | Health Potions   |
| `MAT_Wood_Oak`    | Oak Wood      | Standard sturdy wood.           | 100       | Trees, Woodcutting | Bows, Staves    |
| `MAT_Gem_Ruby`    | Ruby          | A small red gemstone.           | 50        | Mining, Monsters | Jewelry, Enchanting |
| `MAT_Cloth_Linen` | Linen Cloth   | Basic fabric material.          | 100       | Humanoid enemies | Light Armor, Bags |
| `MAT_Metal_Steel` | Steel Ingot   | Refined metal alloy.            | 50        | Smelting Iron Ore | Weapons, Heavy Armor |
| `MAT_Leather_Fine`| Fine Leather  | High-quality treated leather.   | 50        | Crafting from hides | Light Armor, Accessories |

## 5. Currency

Special items representing game money.

| Item ID       | Name        | Description                                | Max Stack | Conversion Rate |
| :------------ | :---------- | :----------------------------------------- | :-------- | :-------------- |
| `CUR_Gold`    | Gold        | The primary currency.                      | Unlimited | 1 Gold = 1 Gold |
| `CUR_Premium` | Gems        | Premium currency.                          | Unlimited | 1 Gem = 100 Gold|
| `CUR_Token_Arena` | Arena Token | Special currency for Arena vendors.    | 999       | 1 Token = 10 Gold|
| `CUR_Token_Faction` | Faction Merit | Used for faction-specific rewards. | 999       | 1 Merit = 50 Gold|

## 6. Junk / Miscellaneous

Items with no specific use other than potentially being sold.

| Item ID         | Name            | Description                     | Max Stack | Sell Value | Special Notes |
| :-------------- | :-------------- | :------------------------------ | :-------- | :--------- | :------------ |
| `JNK_Bone_1`    | Cracked Bone    | A fragment of bone.             | 50        | 1 Gold     | Common monster drop |
| `JNK_Bottle_1`  | Empty Bottle    | Just an empty glass bottle.     | 20        | 1 Gold     | Found in settlements |
| `JNK_Scrap_1`   | Metal Scrap     | A piece of scrap metal.         | 50        | 2 Gold     | From destroyed mechanisms |
| `JNK_Trash_1`   | Soggy Paper     | Wet, illegible paper.           | 50        | 1 Gold     | Found in chests and barrels |
| `JNK_Tooth_1`   | Monster Fang    | A tooth from a monster.         | 30        | 3 Gold     | Drop from carnivorous monsters |
| `JNK_Trinket_1` | Broken Amulet   | A non-magical broken amulet.    | 10        | 5 Gold     | From humanoid enemies |

