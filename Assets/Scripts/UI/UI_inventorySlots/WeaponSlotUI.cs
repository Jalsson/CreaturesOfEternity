using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotUI : InventorySlotUI {

    [SerializeField]
    private WeaponSlotEnum weaponSlotEnum;

    /// <summary>
    /// holds the weaponSlotEnum which type of weapon this slots holds
    /// </summary>
    /// <returns></returns>
    public WeaponSlotEnum GetWeaponSlotEnum()
    {
        return weaponSlotEnum;
    }
}
