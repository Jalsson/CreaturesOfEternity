using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlotUI : InventorySlotUI {

    [SerializeField]
    private ArmorSlotEnum armorSlotEnum;

    /// <summary>
    /// uses armor item
    /// </summary>
    public override void UseItem()
    {
        if (Item != null)
            print("unequip armor");
    }

    public ArmorSlotEnum GetArmorSlotEnum()
    {
        return armorSlotEnum;
    }
}
