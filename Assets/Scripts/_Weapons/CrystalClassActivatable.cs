using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalClassActivatable : BaseWeaponClass {


    /// <summary>
    /// Activates the buff inside character stats and destroys this class.
    /// </summary>
    public override void ActivateWeapon()
    {
        if (GetComponentInParent<CharacterStats>())
        {
            GetComponentInParent<CharacterStats>().StartTempStatEffect(weaponItem);

            if (GetComponentInParent<EntityInventory>())
            {
                if (GetComponentInParent<EntityInventory>().TryUnequipItem(weaponItem))
                        GetComponentInParent<EntityInventory>().TryTakeItem(weaponItem);
            }
        }
    }

}
