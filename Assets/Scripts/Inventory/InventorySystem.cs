using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //the inventory type to use (with maxCapacity and maxWeight)
    [SerializeField] private InventoryBase inventory;

    //the list of items in the inventory
    public InventorySlot[] inventorySlotList { get; private set; }
    public ItemBase equippedWeapon { get; private set; }

    private void Start()
    {
        inventorySlotList = new InventorySlot[inventory.maxItemCapacity];

        for (int i = 0; i < inventorySlotList.Length; i++)
        {
            ClearTheSlot(ref inventorySlotList[i]);
        }
        Debug.Log($"inventory.maxCapacity: {inventory.maxItemCapacity}");
        Debug.Log($"inventorySlotList Length: {inventorySlotList.Length}");

        InventoryActions.UpdateInventoryUI();
        InventoryActions.UpdateEquipWeaponUI();
    }

    private void OnEnable()
    {
        InventoryActions.OnChangeItemPosition += ChangeItemPosition;
        InventoryActions.OnAddItem = AddItem;
    }
    private void OnDisable()
    {
        InventoryActions.OnChangeItemPosition -= ChangeItemPosition;
        InventoryActions.OnAddItem = null;
    }

    //Try moving an item from one slot to another
    public void ChangeItemPosition(int oldIndex, int newIndex)
    {
        ItemBase newItem = inventorySlotList[newIndex].item;
        ItemBase oldItem = inventorySlotList[oldIndex].item;

        //1. se uno dei due slot è vuoto, li scambio.
        if (newItem == null || oldItem == null)
        {
            Debug.Log("Swap with empy slot");
            SwapItems(oldIndex, newIndex);
        }
        //2. se i due slot sono dello stesso tipo: provo a passare più item possibili nel nuovo slot.
        else if (newItem.itemName.Equals(oldItem.itemName))
        {
            MergeSlots(oldIndex, newIndex);
        }
        //3. se due slot sono di un tipo diverso, scambio solo la posizione degli item
        else
        {
            Debug.Log("Swap with full slot");
            SwapItems(oldIndex, newIndex);
        }
        
        //4. aggiorno la lista visualizzata dall'utente
        InventoryActions.UpdateInventoryUI();
    }

    public void AutomaticInventorySorting()
    {
        //sort list using System.Linq
        //1. sort by ItemType
        //2. sort by item name 
        //3 sort by amount in slot
        inventorySlotList = inventorySlotList.OrderBy(i =>  i.item == null? ItemType.SlotNull : i.item.itemType)
            .ThenBy(i => i.item == null ? "" : i.item.itemName)
            .ThenBy(i => i.item == null ? 0 : i.amount)
            .ToArray();

        InventoryActions.UpdateInventoryUI();
        Debug.Log("Automatic Inventory Sorting");
    }

    public ItemBase GetItem(int indexItemToReturn)
    {
        return inventorySlotList[indexItemToReturn].item;
    }

    public int GetAmountItemsInSlot(int indexItemToReturn) {
        return inventorySlotList[indexItemToReturn].amount;
    }

    //Use or Equip an Item. It uses a different logic based on the type of item
    public void UseItem(int indexItemToUse)
    {
        //Check if slot exists
        if (indexItemToUse >= inventorySlotList.Length)
        {
            //out of bounds
            Debug.LogError("indexItemToUse is out of bounds");
            return;
        }

        //Check if slot is empty
        if (IsEmptySlot(inventorySlotList[indexItemToUse])) {
            //out of bounds
            Debug.LogWarning($"Slot n.{indexItemToUse} is empty. Impossible to use.");
            return;
        }

        ItemType itemType = inventorySlotList[indexItemToUse].item.itemType;
        if (itemType == ItemType.Consumable)
        {
            UseConsumableItem(indexItemToUse);
        }
        else if (itemType == ItemType.Weapon)
        {
            EquipWeaponItem(indexItemToUse);
        }
        else
        {
            Debug.LogWarning($"no type detected for item {inventorySlotList[indexItemToUse].item.itemName}");
        }

        InventoryActions.UpdateInventoryUI();
    }

    public bool AddItem(ItemBase itemToAdd)
    {
        //check if there is a slot that contains the item to add. 
        //if there is, I use that slot, otherwise I use an empty slot. 
        //if there are no empty slots, I cannot add the item.

        if (inventorySlotList == null)
        {
            Debug.LogError("InventorySlotList null!");
            return false;
        }

        bool itemHaveBeenAdded=false;
        InventorySlot firstEmpySlot = null;
        foreach (InventorySlot slot in inventorySlotList) {
            if (!IsEmptySlot(slot))
            {
                itemHaveBeenAdded = AddItemToUsedSlot(slot, itemToAdd);
                if(itemHaveBeenAdded)
                    break;
            }
            else
            {
                //the slot is empty
                //if I haven't found any empty slots yet, I can use this slot to add a new item.
                if (firstEmpySlot == null)
                {
                    firstEmpySlot = slot;
                }
            }
        }

        //cannot add item to an existing slot. Try adding to an empty slot
        if (!itemHaveBeenAdded) {

            if (firstEmpySlot != null) {
                //add the item to the empty slot.
                AddItemInEmptySlot(firstEmpySlot, itemToAdd);
                itemHaveBeenAdded=true;
            }
            else
            {
                //unable to add item, no free slots.
                Debug.Log($"Unable to add {itemToAdd.itemName}, no free slots.");
                itemHaveBeenAdded=false;
            }
        }

        Debug.Log($"Item added: {itemHaveBeenAdded.ToString()}");

        InventoryActions.UpdateInventoryUI();
        return itemHaveBeenAdded;
    }

    public ItemBase RemoveItem(int indexItemToDrop, int amountToDelete)
    {
        ItemBase itemToRemove = inventorySlotList[indexItemToDrop].item;
        if (inventorySlotList[indexItemToDrop] != null)
        {
            if (amountToDelete <= inventorySlotList[indexItemToDrop].amount )
            {
                inventorySlotList[indexItemToDrop].amount -= amountToDelete;
                if(inventorySlotList[indexItemToDrop].amount <= 0)
                {
                    Debug.Log($"Item {inventorySlotList[indexItemToDrop].item.itemName} removed successfully");
                    ClearTheSlot(ref inventorySlotList[indexItemToDrop]);
                }
                else
                {
                    //aggiorno peso
                    inventorySlotList[indexItemToDrop].currentWeight -= (amountToDelete * inventorySlotList[indexItemToDrop].item.itemWeight);

                    Debug.Log($"{amountToDelete} {inventorySlotList[indexItemToDrop].item.itemName} have been removed, remaining quantity: {inventorySlotList[indexItemToDrop].amount}");
                }

            }
            else
            {
                Debug.LogWarning("the amount to be removed is greater than the amount in the slot!");
            }
        }
        else
        {
            Debug.LogWarning("no items to remove!");
        }

        InventoryActions.UpdateInventoryUI();

        return itemToRemove;
    }

    public void UnequipItem(ItemUIStatus itemUIStatus)
    {
        Debug.Log($"UnequipItem itemUIStatus: {itemUIStatus}");

        //get the item to unequip
        ItemBase equippedItem = null;
        if(itemUIStatus == ItemUIStatus.InTheEquippedWeaponSlot)
        {
            equippedItem = equippedWeapon;
        }

        //if the item is null, it's impossible to unequip
        if (equippedItem == null)
            return;

        //try to add the item to inventory
        bool added = AddItem(equippedItem);

        if (added)
        {
            if (itemUIStatus == ItemUIStatus.InTheEquippedWeaponSlot)
            {
                equippedWeapon = null;
            }
        }
        InventoryActions.UpdateInventoryUI();
        InventoryActions.UpdateEquipWeaponUI();
    }

    private void UseConsumableItem(int indexItemToUse)
    {
        ConsumableItem consumableItem = (ConsumableItem)inventorySlotList[indexItemToUse].item;
        Debug.Log($"Use item {consumableItem.itemName}...");

        RemoveItem(indexItemToUse, 1);
    }

    private void EquipWeaponItem(int indexItemToUse)
    {
        WeaponItem weaponItem = (WeaponItem)inventorySlotList[indexItemToUse].item;

        if (equippedWeapon == null)
        {
            //the equipped weapon slot is empty
            equippedWeapon = weaponItem;
            RemoveItem(indexItemToUse, 1);
            Debug.Log($"Equip Weapon {weaponItem.itemName}");
        }
        else
        {
            //the equipped weapon slot is alreasy used.
            //Swap the two items.
            ItemBase oldItemEquipped = equippedWeapon;
            equippedWeapon = weaponItem;
            RemoveItem(indexItemToUse, 1);
            bool isAdded = AddItem(oldItemEquipped);

            //if the previously equipped item cannot be moved to the inventory, the new weapon cannot be equipped either
            //returning to the previous situation.
            if (!isAdded)
            {
                AddItem(weaponItem);
                equippedWeapon = oldItemEquipped;
            }
        }

        //update the weapon view
        InventoryActions.UpdateEquipWeaponUI();
    }

    private bool IsEmptySlot(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
            return true;
        else
            return false;
    }

    private bool AddItemToUsedSlot(InventorySlot slot, ItemBase itemToAdd)
    {
        //check if the slot contains the same type of item as the item to add.
        if (slot.item.itemName.Equals(itemToAdd.itemName))
        {
            int newSlotWeight = slot.currentWeight + itemToAdd.itemWeight;
            if (!itemToAdd.useUniqueSlot && newSlotWeight <= inventory.maxWeightPerSlot && slot.amount < inventory.maxItemsAmountPerSlot)
            {
                //update total weight of the slot and number of items contained in it.
                slot.currentWeight = newSlotWeight;
                slot.amount++;
                return true;
            }
            else
            {
                //the total weight of the items in this slot exceeds the maximum weight allowed per slot
                //or the total number of items in the slot exceeds the maximum capacity allowed, I cannot add the item to this slot.
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void AddItemInEmptySlot(InventorySlot empySlot, ItemBase itemToAdd)
    {
        if (empySlot == null)
            Debug.LogError("empySlot is null");
        if (itemToAdd == null)
            Debug.LogError("itemToAdd is null");

        empySlot.amount = 1;
        empySlot.currentWeight = itemToAdd.itemWeight;
        empySlot.item = itemToAdd;
    }

    private void ClearTheSlot(ref InventorySlot slotToClear)
    {
        if(slotToClear != null)
        {
            slotToClear.item = null;
            slotToClear.amount = 0;
            slotToClear.currentWeight = 0;
        }
        else
        {
            slotToClear = new InventorySlot();
        }
    }

    //swap two position items
    private void SwapItems(int oldIndex, int newIndex)
    {
        InventorySlot tmpSlot = inventorySlotList[oldIndex];
        inventorySlotList[oldIndex] = inventorySlotList[newIndex];
        inventorySlotList[newIndex] = tmpSlot;
    }

    private void MergeSlots(int oldIndex, int newIndex)
    {
        //merge all possible items from the old slot into the new slot (There may be limits on the quantity and weight of a single slot.)
        bool addedAtList1 = false;
        int countAddedItems = 0;
        ItemBase newItem = inventorySlotList[newIndex].item;
        for (int i = 0; i < inventorySlotList[oldIndex].amount; i++)
        {
            if (AddItemToUsedSlot(inventorySlotList[newIndex], newItem))
            {
                addedAtList1 = true;
                countAddedItems++;
            }
            else
                break;
        }

        if (addedAtList1)
        {
            Debug.Log($"Added {countAddedItems} to new slot");
            RemoveItem(oldIndex, countAddedItems);
        }
        else
        {
            Debug.Log("0 item swapped");
        }
    }
}



