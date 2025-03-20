Unity Inventory System

This project implements an inventory system in Unity, allowing the management, visualization, and interaction with the player's inventory items.

Main Classes:

- InventorySystem
Manages the internal logic of the inventory.
Contains the list of slot-items.
Exposes the main functions, including:
	-Adding/Removing an item.
	-Sorting items as preferred by the user.
	-Using an item (e.g., equipping a weapon, consuming an object, etc.).

- InventoryUI
Handles user inputs.
Keeps the inventory UI updated at all times.
Directly interacts with InventorySystem to synchronize data and visualization.

- InventoryActions
A static class containing all Actions related to the inventory.
Allows easy communication between different objects without direct connections.


Customization:

-Item Interaction
Users can drag, move, and swap item positions in-game using the ItemUiButton class.
This class implements Unity's EventSystem interfaces to handle user interactions seamlessly.

-Inventory Types
It's possible to create different types of inventories using InventoryBase (a ScriptableObject).
This allows specifying the size and characteristics of the inventory to be used.

-Item Creation
New items can be created using ItemBase (a ScriptableObject).
Each item can have unique characteristics, including different statistics based on the selected item type.
