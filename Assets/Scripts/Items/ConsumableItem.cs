using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Game/Item/Consumable")]
public class ConsumableItem : ItemBase
{
    [Header("Consumable stats:")]
    public int health;


    //using the constructor to set the default itemType to Consumable.
    //the itemType can still be changed manually after the scriptable object is created.
    public ConsumableItem() : base(ItemType.Consumable)
    {

    }


}
