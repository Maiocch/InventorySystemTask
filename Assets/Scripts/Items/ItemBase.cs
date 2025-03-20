using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract class containing all the generic data of each item
//must be implemented by other classes in order to represent a specific type of item
public abstract class ItemBase : ScriptableObject
{
    [Header("Generic item stats:")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public bool useUniqueSlot; //if true, this Item will occupy an entire slot by itself, otherwise it is possible that a slot contains multiple items of the same type

    //each item has its own weight. Consequently, each slot can contain a limited number of items based on the maximum weight of the slot.
    public int itemWeight;

    //the constructor forces derived classes to choose a default ItemType when they are constructed
    protected ItemBase(ItemType _itemType)
    {
        itemType = _itemType;
    }

}
