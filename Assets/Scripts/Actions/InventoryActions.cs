using System;
using System.Collections;
using System.Collections.Generic;

//using Actions to easily call one or more functions subscribed to the actions, without needing a direct reference.
public static class InventoryActions
{
    //using a Func as it returns true if the item is added successfully.
    public static Func<ItemBase, bool> OnAddItem;
    public static bool AddItem(ItemBase itemBaseToAdd)
    {
        return OnAddItem?.Invoke(itemBaseToAdd) ?? false;
    }

    //Use this action to update the inventory shown to the user
    public static Action OnUpdateInventoryUI;
    public static void UpdateInventoryUI()
    {
        OnUpdateInventoryUI?.Invoke();
    }

    public static Action OnUpdateEquipWeaponUI;
    public static void UpdateEquipWeaponUI()
    {
        OnUpdateEquipWeaponUI?.Invoke();
    }

    public static Action<int, int> OnChangeItemPosition;
    public static void ChangeItemPosition(int oldIndex, int newIndex)
    {
        OnChangeItemPosition?.Invoke(oldIndex, newIndex);
    }

    //to be used when a single item needs to be displayed to the user
    public static Action<int, ItemUIStatus> OnShowItemDetails;
    public static void ShowItemDetails(int indexSelectedItem, ItemUIStatus selectedItemStatus)
    {
        OnShowItemDetails?.Invoke(indexSelectedItem, selectedItemStatus);
    }




}
