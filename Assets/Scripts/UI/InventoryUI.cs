using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [Header("References for equipped items")]
    [SerializeField] private GameObject equippedWeaponContainer;
    [SerializeField] private EquippedItemUI equippedUIPrefab;
    [SerializeField] private GameObject equippedSlotUIPrefab;

    [Header("References for the visual items list")]
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private ItemUiButton itemUIPrefab;
    [SerializeField] private InventorySlotUI slotUIPrefab;
    [SerializeField] private GameObject itemUIContiainer;

    [Header("References for the item details view")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text useEquipText; //the text changes depending on the type of object selected.
    [SerializeField] private TMP_Text discardText; //the text changes depending on the type of object selected.

    private int indexSelectedItem;
    private ItemUIStatus selectedItemStatus;

    private void OnEnable()
    {
        InventoryActions.OnUpdateInventoryUI += UpdateItemsToShow;
        InventoryActions.OnUpdateEquipWeaponUI += UpdateEquipWeaponToShow;
        InventoryActions.OnShowItemDetails += ShowItem;
    }

    private void OnDisable()
    {
        InventoryActions.OnUpdateInventoryUI -= UpdateItemsToShow;
        InventoryActions.OnUpdateEquipWeaponUI -= UpdateEquipWeaponToShow;
        InventoryActions.OnShowItemDetails -= ShowItem;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseItem(indexSelectedItem);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            DiscardItem(indexSelectedItem);
        }
    }

 

    //update the items to show to the user
    public void UpdateItemsToShow()
    {
        if (itemUIContiainer == null)
        {
            Debug.LogError("Items container is null!");
            return;
        }

        //Destroy all children (the grid re-starts from zero elements)
        foreach (Transform child in itemUIContiainer.transform)
        {
            Destroy(child.gameObject);
        }

        //recreate slots and items to show to the user 
        for (int i = 0; i < inventorySystem.inventorySlotList.Length; i++) {

            //slot creation
            GameObject slotUiGameObject = Instantiate(slotUIPrefab.gameObject);
            slotUiGameObject.transform.SetParent(itemUIContiainer.transform, false);
            slotUiGameObject.GetComponent<InventorySlotUI>().indexSlot = i;

            //Creating the item in the slot
            if (inventorySystem.GetItem(i) != null)
            {
                GameObject ItemUiGameObject = Instantiate(itemUIPrefab.gameObject);
                ItemUiButton newItemButton = ItemUiGameObject.GetComponent<ItemUiButton>();
                newItemButton.transform.SetParent(slotUiGameObject.transform, false);
                newItemButton.InitItemBase(i, inventorySystem.GetItem(i), inventorySystem.GetAmountItemsInSlot(i));
            }
        }
    }

    private void UpdateEquipWeaponToShow()
    {
        EquippedItemUI newItemToEquip;
        if (equippedWeaponContainer.transform.childCount > 0)
        {
            //destroy slots
            foreach (Transform child in equippedWeaponContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //slot creation
        GameObject slotUiGameObject = Instantiate(equippedSlotUIPrefab.gameObject);
        slotUiGameObject.transform.SetParent(equippedWeaponContainer.transform, false);

        if (inventorySystem.equippedWeapon != null)
        {
            //Creating the weapon in the slot
            newItemToEquip = Instantiate(equippedUIPrefab);
            newItemToEquip.transform.SetParent(slotUiGameObject.transform, false);
            newItemToEquip.InitItemBase(0, inventorySystem.equippedWeapon, 1);
        }
    }

    //example method to show all item information to the user
    //It takes the data of the last selected item (logic in ItemUiButton)
    public void ShowItem(int _indexItemToShow, ItemUIStatus _selectedItemStatus)
    {

        /* private ItemBase selectedItemBase;
           private ItemUIStatus selectedItemStatus; */
        selectedItemStatus = _selectedItemStatus;
        indexSelectedItem = _indexItemToShow;

        ItemBase itemBase=null;
        if (selectedItemStatus == ItemUIStatus.InTheInventorySlot)
        {
            Debug.Log($"show InTheInventorySlot");
            itemBase = inventorySystem.GetItem(_indexItemToShow);
            if (itemBase.itemType == ItemType.Weapon)
            {
                useEquipText.text = "E: Equip";
                discardText.text = "F: Discard";
            }
            else
            {
                useEquipText.text = "E: Use";
                discardText.text = "F: Discard";
            }
        }
        else if(selectedItemStatus == ItemUIStatus.InTheEquippedWeaponSlot)
        {
            Debug.Log($"show InTheEquippedWeaponSlot");
            itemBase = inventorySystem.equippedWeapon;
            useEquipText.text = "";
            discardText.text = "F: Unequip";
        }
        else
        {
            Debug.LogWarning("Status item not valid");
            return;
        }

        if (itemNameText == null)
        {
            Debug.Log($"itemNameText is null");
        }
        if (itemBase == null)
        {
            Debug.Log($"itemBase is null");
        }

        itemNameText.text = itemBase.itemName;
        itemDescriptionText.text = itemBase.itemDescription;
        itemIcon.sprite = itemBase.itemIcon;
    }

    private void UseItem(int indexItemToShow)
    {
        if(selectedItemStatus == ItemUIStatus.InTheInventorySlot)
        {
            inventorySystem.UseItem(indexItemToShow);
        }

        //sugli item equipaggiati non possono eseguire azioni...

    }

    private void DiscardItem(int indexItemToShow)
    {
        Debug.Log($"DiscardItem");

        if (selectedItemStatus == ItemUIStatus.InTheInventorySlot)
        {
            inventorySystem.RemoveItem(indexItemToShow, 1);
        }
        else if (selectedItemStatus == ItemUIStatus.InTheEquippedWeaponSlot)
        {
            inventorySystem.UnequipItem(selectedItemStatus);
        }
    }

}
