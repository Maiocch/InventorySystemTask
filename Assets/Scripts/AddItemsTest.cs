using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemsTest : MonoBehaviour
{
    //method test...
    //the items to add for testing
    [Header("Mockup test")]
    [SerializeField] private List<ItemAndQuantityToAdd> itembBaseListTest;

    //test method
    public void CreateMockupInventory()
    {
        Debug.Log("CreateMockupInventory");
        foreach (ItemAndQuantityToAdd var in itembBaseListTest)
        {
            if(var != null && var.item && var.quantityToAdd > 0){
                AddItems( var.item, var.quantityToAdd);
            }
        }
    }

    public void AddSingleItem(int index)
    {
        Debug.Log($"Add Mockup Item {index}");
        try
        {
            if(index >=0 && index< itembBaseListTest.Count && itembBaseListTest[index]!=null)
                AddItems(itembBaseListTest[index].item, 1);
        }
        catch (Exception e) { 
        Debug.LogError($"Impossible to add the item {index}. Error:\n{e.Message}");
        }

    }


    private int AddItems(ItemBase itemBase, int quantityToAdd)
    {
        int nItemsAdded = 0;
        for (int i = 0; i < quantityToAdd; ++i)
        {
            bool isAdded = InventoryActions.AddItem(itemBase);
            if (isAdded)
                nItemsAdded++;
            else
                break; //It is no longer possible to add more items
        }

        return nItemsAdded;
    }
}

[Serializable]
public class ItemAndQuantityToAdd
{
    public ItemBase item;
    public int quantityToAdd;
}
