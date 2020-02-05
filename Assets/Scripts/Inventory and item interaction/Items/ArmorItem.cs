using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class ArmorItem : Item {

    public ArmorSlotEnum ArmorEquipSlotEnum
    {
        get
        {
            return armorEquipSlotEnum;
        }

    }

    public ArmorMeshRegion[] CoveredMeshRegionsEnums
    {
        get
        {
            return coveredMeshRegionsEnum;
        }

    }

    public SkinnedMeshRenderer Mesh
    {
        get
        {
            return mesh;
        }
    }

    public float DamagaReduction
    {
        get
        {
            return damageReduction;
        }
    }

    public float MovementSlow
    {
        get
        {
            return movementSlow;
        }
    }

    [SerializeField]
    private ArmorSlotEnum armorEquipSlotEnum;
    [SerializeField]
    private ArmorMeshRegion[] coveredMeshRegionsEnum;

    [SerializeField]
    private SkinnedMeshRenderer mesh;
    [SerializeField]
    private float damageReduction;
    [SerializeField]
    private float movementSlow;

    /// <summary>
    /// Overloading use method with userObject so we can equip armor properly
    /// </summary>
    /// <param name="userObject"></param>
    public void Use(GameObject userObject)
    {
        if(userObject.GetComponent<ArmorAndWeaponEquipper>())
            userObject.GetComponent<ArmorAndWeaponEquipper>().EquipArmor(this);
    }

    /// <summary>
    /// Unequips this armor item from given character
    /// </summary>
    /// <param name="userObject"></param>
    public void Unequip(GameObject userObject)
    {
        if(userObject.GetComponent<ArmorAndWeaponEquipper>())
        userObject.GetComponent<ArmorAndWeaponEquipper>().Unequip(this);
    }


}

public enum ArmorSlotEnum { Head, Chest, Belt, Legs, NumberOfTypes }
public enum ArmorMeshRegion { Legs, Torso, Head } // body Morphes