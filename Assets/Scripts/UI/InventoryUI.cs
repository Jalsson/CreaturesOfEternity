using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Venus.Delegates;

public class InventoryUI : MonoBehaviour
{

    
    private GameObject selectionCircle;
    private GameObject infoPanel;
    

    public InventorySlotUI InventorySlotInUse { private set; get ; }

    #region UI gameobject and parent references

    private GameObject[] playerInventoryUI = new GameObject[3];    // for opening the Ui elements
    private Transform StorageItemsParent;
    private GameObject storageUI;

    private Transform armorParent;
    private Transform itemsParent;
    private Transform weaponsParent;

    ArmorSlotUI[] armorSlots;
    WeaponSlotUI[] weaponSlots;
    InventorySlotUI[] inventorySlots;
    StorageInventorySlotUI[] storageSlots;

    #endregion

    public Storage currentStorageRef { private set; get; }
    EntityInventory playerInventory;
    UiStateUpdateDelegate uiStateUpdateDelegate;

    /// <summary>
    /// Getting all inventory related references that they can later be enabled
    /// </summary>
    void Start()
    {
        playerInventoryUI[0] = transform.Find("Inventory").gameObject;
        playerInventoryUI[1] = transform.Find("Equipment").gameObject;

        armorParent = playerInventoryUI[1].transform.Find("ArmorParent");
        itemsParent = playerInventoryUI[0].transform.Find("ItemsParent");
        weaponsParent = playerInventoryUI[1].transform.Find("WeaponsParent");

        storageUI = transform.Find("storageUI").gameObject;
        playerInventoryUI[2] = storageUI;
        StorageItemsParent = storageUI.transform.Find("StorageItemsParent");

        selectionCircle = transform.Find("SelectCircle").gameObject;
        infoPanel = transform.Find("InfoPanel").gameObject;

        weaponSlots = weaponsParent.GetComponentsInChildren<WeaponSlotUI>();
        armorSlots = armorParent.GetComponentsInChildren<ArmorSlotUI>();
        inventorySlots = itemsParent.GetComponentsInChildren<InventorySlotUI>();
        storageSlots = StorageItemsParent.GetComponentsInChildren<StorageInventorySlotUI>();

        playerInventory = PlayerManager.S_INSTANCE.player.GetComponent<EntityInventory>();
    }

    public void InventoryState(bool state)
    {
        for (int i = 0; i < playerInventoryUI.Length; i++)
        {
            playerInventoryUI[i].SetActive(state);
            selectionCircle.SetActive(false);
        }
        UpdateInventoryUI();
    }

    public void StorageState(bool state)
    {
        storageUI.SetActive(state);
    }

    public void PointerEnterItem()
    {

    }

    /// <summary>
    /// Activates items Use() function. If item is equipment, will just try to swap it to armor slots that it can be equiped.
    /// Also if we are trying to use armor or weapon slot, tries to unequip the item.
    /// </summary>
    public void UseItem()
    {
        selectionCircle.SetActive(false);
        if (InventorySlotInUse is WeaponSlotUI)
        {
            playerInventory.TryUnequipItem(InventorySlotInUse.Item);
        }
        else if (InventorySlotInUse is ArmorSlotUI)
        {
            playerInventory.TryUnequipItem(InventorySlotInUse.Item);
        }
        else if (InventorySlotInUse)
        {
            if (InventorySlotInUse.Item is WeaponItem)
            {
                playerInventory.SwapItem(InventorySlotInUse.Item);
            }
            else if (InventorySlotInUse.Item is ArmorItem)
            {
                playerInventory.SwapItem(InventorySlotInUse.Item);
            }
            else
            {
                InventorySlotInUse.UseItem();
            }
        }
        UpdateInventoryUI();

    }

    /// <summary>
    /// Activates the infopanel where player can read the description of the item.
    /// Loops through the infopanels and sets apropriate values for each value
    /// </summary>
    public void InfoPanelON()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
        foreach (Transform childTransform in infoPanel.transform.GetComponentsInChildren<Transform>())
        {
            if (childTransform.name == "Info_Name")
                childTransform.GetComponent<TextMeshProUGUI>().text = InventorySlotInUse.Item.Name;

            if (childTransform.name == "Info_Description")
                childTransform.GetComponent<TextMeshProUGUI>().text = InventorySlotInUse.Item.Description;

            if (childTransform.name == "Info_Price")
                childTransform.GetComponent<TextMeshProUGUI>().text = "Price: " + InventorySlotInUse.Item.Price.ToString();

            if (childTransform.name == "Info_Icon")
                childTransform.GetComponent<Image>().sprite = InventorySlotInUse.Item.Icon;
        }
    }

    public void DropItem()
    {
        playerInventory.DropItem(InventorySlotInUse.Item);
        UpdateInventoryUI();
        selectionCircle.SetActive(false);
    }

    /// <summary>
    /// Used to transfer item between other storages like chests and lootable bodys.
    /// </summary>
    public void TransferItem()
    {
        if (currentStorageRef)
        {
            if (currentStorageRef.TryStoreItem(InventorySlotInUse.Item))
            {
                if (playerInventory.TryTakeItem(InventorySlotInUse.Item))
                {
                    InventorySlotInUse.ClearSlot();
                    UpdateInventoryUI();
                    UpdateStorageUI(currentStorageRef);
                }
                else
                    print("cannot take item");

            }
            else
                print("Cannot store Item");
        }
    }

    /// <summary>
    /// Called when mouse is no longer on top of the UI element and deactivates the selection circle
    /// </summary>
    public void PointerExitItem()
    {
        selectionCircle.SetActive(false);
    }

    /// <summary>
    /// Called when users clicks item on the UI and moves the interaction menu on top of the given item slot.
    /// </summary>
    /// <param name="inventorySlot"></param>
    public void OnRightClickItem(InventorySlotUI  inventorySlot)
    {
        InventorySlotInUse = inventorySlot;

        selectionCircle.transform.position = Input.mousePosition;
        selectionCircle.SetActive(true);
    }

    /// <summary>
    /// called when pointer exits panel
    /// </summary>
    public void PointerExitPanel()
    {
        
    }

    /// <summary>
    /// Loops through Player Entity inventory and gives UI slots their given invenotory items.
    ///  Where Inventory slots then take their information for item icon and name.
    /// </summary>
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < playerInventory.Items.Count)
                inventorySlots[i].AddItem(playerInventory.Items[i]);

            else
                inventorySlots[i].ClearSlot();
            
        }

        for (int i = 0; i < armorSlots.Length; i++)
        {
                armorSlots[i].ClearSlot();

                for (int a = 0; a < playerInventory.EntityArmor.Count; a++)
                {
                    if (armorSlots[i].GetArmorSlotEnum() == playerInventory.EntityArmor[a].ArmorEquipSlotEnum)
                    {
                        armorSlots[i].AddItem(playerInventory.EntityArmor[a]);
                    }
                    
                }

        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].ClearSlot();

            for (int a = 0; a < playerInventory.EntityWeapons.Count; a++)
            {
                if (weaponSlots[i].GetWeaponSlotEnum() == playerInventory.EntityWeapons[a].WeaponSlotEnum)
                {
                    weaponSlots[i].AddItem(playerInventory.EntityWeapons[a]);
                }

            }
        }
    }

    /// <summary>
    /// Loops through Storage inventory that player is interacting with and gives UI slots their given invenotory items.
    ///  Where Inventory slots then take their information for item icon and name.
    /// </summary>
    /// <param name="storageRef"></param>
    public void UpdateStorageUI(Storage storageRef)
    {
        currentStorageRef = storageRef;
        if (currentStorageRef)
        {
            for (int i = 0; i < storageSlots.Length; i++)
            {
                storageSlots[i].SetStorageReference(storageRef);

                if (i < storageRef.Items.Count)
                {
                    storageSlots[i].AddItem(storageRef.Items[i]);
                }
                else
                {
                    storageSlots[i].ClearSlot();
                }
            }
        }
    }
}