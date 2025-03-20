using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class ItemUIBase : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected TMP_Text textAmount;
    public Image itemBg; //background icon
    public Image itemIcon; //icon

    [Header("Effects")]
    [SerializeField] protected Color colorBgStarndard;
    [SerializeField] protected Color colorBgWhenItemSelected;
    [Range(0f, 1f)]
    [SerializeField] protected float transparencyWhenDragged;
    [SerializeField] protected Vector3 scaleSizeItemWhenSelected;

    [HideInInspector] public int indexItem;
    [HideInInspector] public ItemUIStatus itemUIStatus;
    public ItemBase item { get; protected set; }


    public void InitItemBase(int _indexItem, ItemBase _item, int amount)
    {
        indexItem = _indexItem;
        item = _item;
        textAmount.text = amount.ToString();

        if (item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

        SetItemStatus();
    }

    public abstract void SetItemStatus();



}
