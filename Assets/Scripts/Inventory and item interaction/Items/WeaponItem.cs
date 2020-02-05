using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponItem : Item {



    public float WeaponDamage
    {
        get
        {
            return weaponDamage;
        }

    }

    public float AttackSpeedOrDuration
    {
        get
        {

            return attackSpeedOrDuration;
        }
    }

    public float StatEffectOrRadius
    {
        get
        {
            return statEffectOrRadius;
        }
    }

    public CrystalTypeEnum CrystalTypeEnum
    {
        get { return crystalTypeEnum; }
    }

    public WeaponSlotEnum WeaponSlotEnum
    {
        get { return weaponSlotEnum; }
    }

    public GameObject WeaponObject{ get { return weaponObject; } }
    
    [SerializeField]
    [Tooltip("If your weapon is sword it's a primary type, Crystals can be equipped in primary and secondary slots")]
    private WeaponSlotEnum weaponSlotEnum;


    [SerializeField]
    [Tooltip("This defines the behavior of the crystal. If your weapon is sword, leave this to none")]
    private CrystalTypeEnum crystalTypeEnum;

    [SerializeField]
    [Tooltip("This is only used with swords and explosive type crystal. Defines the damage that it does to chracter when it hits")]
    private float weaponDamage = 10;

    [SerializeField]
    [Tooltip("If this weapon is sword, this defines the speed of animations, only use values between 0.8-1.5. If you are creating crystal. This defines the projectile speed of Explosive crystal and duration for buff duration")]
    private float attackSpeedOrDuration = 1;

    [SerializeField]
    [Tooltip("Only used with crystal weapons, if your crystal type is either buff or nerf this amount of that status effect. If type is explosive this is radius of that explosion")]
    private float statEffectOrRadius = 0;

    [SerializeField]
    [Tooltip("Object that is spawned in characters hand when equipped")]
    private GameObject weaponObject;


    /// <summary>
    /// Equips the armor to given gameObject if the object is humanoid chracter
    /// </summary>
    /// <param name="userObject">game object that armor will be equipped, needs armorAndWeaponEquipper class to work</param>
    public void Use(GameObject userObject)
    {
        if (userObject.GetComponent<ArmorAndWeaponEquipper>())
            userObject.GetComponent<ArmorAndWeaponEquipper>().EquipWeapon(this);
    }

    /// <summary>
    /// Unequips the given item from player
    /// </summary>
    /// <param name="itemUserObject"></param>
    public void Unequip(GameObject itemUserObject)
    {
        if (itemUserObject.GetComponent<ArmorAndWeaponEquipper>())
            itemUserObject.GetComponent<ArmorAndWeaponEquipper>().Unequip(this);
    }


}

public enum WeaponSlotEnum {Primary, Secondary, NumberOfTypes }
public enum CrystalTypeEnum {None, Explosive, ArmorBuff, SpeedBuff, HealthBuff, NumberOfTypes }