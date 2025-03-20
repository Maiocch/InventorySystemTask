using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//example class representing the single item shown in the inventory
//By implementing the EventSystem interfaces, the user can interact with items in the inventory by selecting them with the mouse and dragging them
public class ItemUiButton : ItemUIBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    [HideInInspector] public Transform parentAfterDrag; //per far si che mentre sposto l'item, sia graficamente sopra a tutti gli altri slot
    private bool isDragging;

    public override void SetItemStatus()
    {
        itemUIStatus = ItemUIStatus.InTheInventorySlot;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Begin drag {"item.itemName"}");
        isDragging = true;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        //devo disattivare il mouse quando inizio a trascinare l'item col mouse, così il mouse non conta l'item stesso per trovare il nuovo slot puntato dal mouse.
        itemBg.raycastTarget = false;
        itemIcon.raycastTarget = false;
        textAmount.raycastTarget = false;

        itemBg.color = new Color(itemBg.color.r, itemBg.color.g, itemBg.color.b, transparencyWhenDragged);
    }

    public void OnDrag(PointerEventData eventData)
    {       
        // Debug.Log($"On drag {"item.itemName"}");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End drag {"item.itemName"}");
        transform.SetParent(parentAfterDrag);

        itemBg.raycastTarget = true;
        itemIcon.raycastTarget = true;
        textAmount.raycastTarget = true;


        itemBg.color = colorBgStarndard;
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {//show items details
   //     Debug.Log($"indexItem: {indexItem}, itemUIStatus: {itemUIStatus}");
        InventoryActions.ShowItemDetails(indexItem, itemUIStatus);

        //graphic effects...
        if (!isDragging)
        {
            itemBg.color = colorBgWhenItemSelected;
            transform.localScale = scaleSizeItemWhenSelected;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //graphic effects...
        if (!isDragging)
        {
            itemBg.color = colorBgStarndard;
            transform.localScale = Vector3.one;
        }
  
    }
}
