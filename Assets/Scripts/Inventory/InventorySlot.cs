using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents an inventory slot.
//It implements the IComparable interface to create its sorting system, giving each type of item a different value
public class InventorySlot
{
    public ItemBase item;
    public int amount;
    public int currentWeight;
}
