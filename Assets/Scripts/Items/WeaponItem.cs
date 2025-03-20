using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Item/Weapon")]
public class WeaponItem : ItemBase
{
    [Header("Weapon stats:")]
    public int attackDamage;
    public float attackSpeed;
    public float criticalChance;
    public int health;

    //using the constructor to set the default itemType to Weapon.
    //the itemType can still be changed manually after the scriptable object is created.
    public WeaponItem() : base(ItemType.Weapon)
    {
    }



}
