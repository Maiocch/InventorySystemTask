using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this ScriptableObject represents the inventory used by the user.
//It's possible to create multiple inventories with different stats to improve and expand the inventory even at run-time.
[CreateAssetMenu(fileName = "New Inventory", menuName = "Game/Inventory/New Inventory")]
public class InventoryBase : ScriptableObject
{
    public int maxItemCapacity;
    public int maxWeightPerSlot;
    public int maxItemsAmountPerSlot;
}
