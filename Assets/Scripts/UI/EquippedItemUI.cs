using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedItemUI : ItemUIBase, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {//show items details
        InventoryActions.ShowItemDetails(indexItem, itemUIStatus);

        //graphic effects...
        itemBg.color = colorBgWhenItemSelected;
        transform.localScale = scaleSizeItemWhenSelected;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //graphic effects...
        itemBg.color = colorBgStarndard;
        transform.localScale = Vector3.one;
    }

    public override void SetItemStatus()
    {
        itemUIStatus = ItemUIStatus.InTheEquippedWeaponSlot;
    }
}
