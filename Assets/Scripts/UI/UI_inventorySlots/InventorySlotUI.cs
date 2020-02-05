using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class InventorySlotUI : MonoBehaviour {

    private Image icon;

    protected Button ItemButton;

    public Item Item { get; protected set; }
    protected InventoryUI invenoryUI;


    protected virtual void Awake()
    {
        GetComponents();
    }

    /// <summary>
    ///  Adds Item to referenced slot and updates the icon with picture that is stored in item.
    /// </summary>
    /// <param name="newItem">Item that we want to display inside this slot</param>
    public virtual void AddItem (Item newItem)
    {
        if (icon == null)
            GetComponents();
        
        Item = newItem;

        icon.sprite = Item.Icon;
        icon.enabled = true;
    }

    /// <summary>
    /// Clears the slot and sets it as empty
    /// </summary>
    public virtual void ClearSlot()
    {
        GetComponents();

        Item = null;
        icon.sprite = null;
        icon.enabled = false;   
    }

    /// <summary>
    /// Activates items Use variable
    /// </summary>
    public virtual void UseItem()
    {
        if (Item != null)
        {
            if (Item is ArmorItem)
            {
                ArmorItem equipment = Item as ArmorItem;
                equipment.Use(PlayerManager.S_INSTANCE.player);
            }
            else if(Item is WeaponItem)
            {
                WeaponItem weapon = Item as WeaponItem;
                weapon.Use(PlayerManager.S_INSTANCE.player);
            }
            else
            {
                Item.Use();
            }
        }
    }

    /// <summary>
    /// Gets component references 
    /// </summary>
    public virtual void GetComponents()
    {
        invenoryUI = GetComponentInParent<InventoryUI>();
        icon = GetComponentInChildren<Image>();
        ItemButton = GetComponentInChildren<Button>();

        if (!GetComponent<StorageInventorySlotUI>())
        {
            ItemButton.onClick.AddListener(RightClickItem);
        }
    }


    /// <summary>
    /// Sets the instance of interacted inventory slot in Inventory UI to this.
    /// </summary>
    public virtual void RightClickItem()
    {
        if (invenoryUI != null)
        {
            invenoryUI.OnRightClickItem(this);
        }
    }

}
