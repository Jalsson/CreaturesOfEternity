using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponClass : MonoBehaviour {

    [SerializeField]
    protected Vector3 offSetPosition;
    [SerializeField]
    protected Quaternion offSetRotation;

    // set private weapon damage and attack speed, these are setupped in constructor
    protected WeaponItem weaponItem;

    protected float weaponDamageModfier = 1;
    protected float attackSpeedModifier = 1;


    protected virtual void Start()
    {
        transform.localPosition = offSetPosition;
        transform.localRotation = offSetRotation;
    }

    /// <summary>
    /// Set default values from item
    /// </summary>
    /// <param name="weaponDamage"></param>
    /// <param name="attackSpeed"></param>
    /// <param name="statEffect"></param>
    public virtual void SetWeaponItem(WeaponItem weaponItem)
    {
        this.weaponItem = weaponItem;
    }

    public WeaponItem WeaponItem
    {
        get { return weaponItem; }
       
    }

    public virtual void ActivateWeapon()
    {

    }

    public virtual void Unequip()
    {

    }

    public virtual void SetWeaponModifiers(float weaponDamageModfier, float attackSpeedModifier)
    {
        this.weaponDamageModfier = weaponDamageModfier;
        this.attackSpeedModifier = attackSpeedModifier;
    }

}
