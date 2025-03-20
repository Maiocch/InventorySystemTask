using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    [HideInInspector] public int indexSlot;

    //when the user drops an item in this position
    public void OnDrop(PointerEventData eventData)
    {
        //Get the dropped item
        GameObject dropped = eventData.pointerDrag;
        //by dropping an item into this new slot, I have to set this slot as the new parent of the item
        ItemUiButton draggableItem = dropped.GetComponent<ItemUiButton>();
        InventoryActions.ChangeItemPosition(draggableItem.indexItem, indexSlot);
        Debug.Log($"On drop item {draggableItem.item.itemName} in the slot n. {indexSlot}");
    }

}
