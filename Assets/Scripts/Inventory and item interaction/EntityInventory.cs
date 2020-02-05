using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;

public class EntityInventory : Storage
{
    /// <summary>
    /// Money amount. Get only.
    /// </summary>
    public int Money
    {
        get
        {
            return money;
        }
    }

    /// <summary>
    /// All the weapons stored in this entity's storage.
    /// </summary>
    public List<WeaponItem> EntityWeapons
    {
        get
        {
            return weapons;
        }
    }

    /// <summary>
    /// All the armor stored in this entity's storage.
    /// </summary>
    public List<ArmorItem> EntityArmor
    {
        get
        {
            return armor;
        }
    }

    /// <summary>
    /// Money amount.
    /// </summary>
    [SerializeField]
    protected int money;

    [Space]

    /// <summary>
    /// Max amount of weapons that can be equipped
    /// </summary>
    [SerializeField]
    protected int maxWeapons = 2;

    /// <summary>
    /// Max amount of armors that can be equipped
    /// </summary>
    [SerializeField]
    protected int maxArmors = 4;

    /// <summary>
    /// List of all the weapon items in this entity's storage.
    /// </summary>
    protected LimitedList<WeaponItem> weapons = new LimitedList<WeaponItem>();

    /// <summary>
    /// List of all the armor items in this entity's storage.
    /// </summary>
    protected LimitedList<ArmorItem> armor = new LimitedList<ArmorItem>();

    protected void Start()
    {
        //Set max slots
        weapons.SetMaxSlots(maxWeapons);
        armor.SetMaxSlots(maxArmors);
    }

    /// <summary>
    /// Checks if the person can afford the given cost.
    /// </summary>
    /// <param name="cost">The cost amount.</param>
    /// <returns>True if can afford.</returns>
    public bool CanAfford (int cost)
    {
        if (money >= cost)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Receives the given amount of money.
    /// </summary>
    /// <param name="amount">Money amount</param>
    public void ReceiveMoney (int amount)
    {
        money += amount;
    }

    /// <summary>
    /// Trades money from this to the receiver and returns a bool indicating if it was successful.
    /// </summary>
    /// <param name="amount">Amount to trade from this.</param>
    /// <param name="receiver">EntityInventory to receive the amount of money.</param>
    /// <returns>True if transaction was successful</returns>
    public bool TradeMoney (int amount, EntityInventory receiver)
    {
        //Make sure that the amount is valid, this EntityInventory has enough to pay and the reiceiver is not null
        if (money <= 0 || !CanAfford(amount) || receiver == null)
        {
            return false;
        }

        //Remove money
        money -= amount;

        //Give money
        receiver.ReceiveMoney(amount);

        //Went all gud very nais
        return true;
    }

    /// <summary>
    /// Tries to swap the given item to the equipped weapons or armor.
    /// </summary>
    /// <param name="item">Item to swap to equipped items.</param>
    /// <returns>True if successful.</returns>
    public bool SwapItem (Item item)
    {
        bool result = false;

        if (item is WeaponItem)
        {
            WeaponItem weaponCast = (WeaponItem)item;

            result = SwapItem(weaponCast);
        }
        else if (item is ArmorItem)
        {
            ArmorItem armorCast = (ArmorItem)item;

            result = SwapItem(armorCast);
        }

        return result;
    }

    /// <summary>
    /// Tries to swap the given weapon to the equipped weapons.
    /// </summary>
    /// <param name="weapon">Weapon to swap to equipped weapons.</param>
    /// <returns>True if successful.</returns>
    public bool SwapItem (WeaponItem weapon)
    {
        WeaponItem weaponEquipped = GetWeaponFromSlot(weapon.WeaponSlotEnum);

        if (weaponEquipped != null) //Unequip if something is equipped
        {
            TryUnequipItem(weaponEquipped);
        }

        return TryEquipItem(weapon);
    }

    /// <summary>
    /// Tries to swap the given armor to the equipped armor.
    /// </summary>
    /// <param name="armorItem">Armor to swap to equipped armor.</param>
    /// <returns>True if successful.</returns>
    public bool SwapItem(ArmorItem armorItem)
    {
        ArmorItem armorEquipped = GetArmorFromSlot(armorItem.ArmorEquipSlotEnum);

        if (armorEquipped != null) //Unequip if something is equipped
        {
            TryUnequipItem(armorEquipped);
        }

        return TryEquipItem(armorItem);
    }

    /// <summary>
    /// Tries to unequip the given item from the equipment slots and to add it back to items.
    /// </summary>
    /// <param name="item">Item to unequip.</param>
    /// <returns>True if successful.</returns>
    public bool TryUnequipItem (Item item)
    {
        if (item is WeaponItem)
        {
            weapons.Remove((WeaponItem)item);

            Items.Add(item);

            WeaponItem weaponItem = item as WeaponItem;
            weaponItem.Unequip(gameObject);

            return true;
        }
        else if (item is ArmorItem)
        {
            armor.Remove((ArmorItem)item);

            Items.Add(item);

            ArmorItem armorItem = item as ArmorItem;
            armorItem.Unequip(gameObject);

            return true;
        }

        return false;
    }

    #region Try Equip Item

    /// <summary>
    /// Tries to equip the given item.
    /// </summary>
    /// <param name="item">Item to equip.</param>
    /// <returns>True if equipped successfully.</returns>
    public bool TryEquipItem (Item item)
    {
        bool result = false;
        if (item is WeaponItem)
        {
            //Cast items
            WeaponItem weaponCast = (WeaponItem)item;

            result = TryEquipItem(weaponCast);
        }
        else if (item is ArmorItem)
        {
            //Cast item
            ArmorItem armorCast = (ArmorItem)item;

            result = TryEquipItem(armorCast);
        }

        return result;
    }

    /// <summary>
    /// Tries to equip the given weapon.
    /// </summary>
    /// <param name="weapon">Weapon to equip.</param>
    /// <returns>True if successful.</returns>
    public bool TryEquipItem(WeaponItem weapon)
    {
        if (GetWeaponFromSlot(weapon.WeaponSlotEnum) == null) //Try to equip item
        {
            if (weapons.TryAddItem(weapon))
            {
                Items.Remove(weapon);

                weapon.Use(gameObject);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Tries to equip the given armor.
    /// </summary>
    /// <param name="armorItem">Armor to equip.</param>
    /// <returns>True if successful.</returns>
    public bool TryEquipItem(ArmorItem armorItem)
    {

        if (GetArmorFromSlot(armorItem.ArmorEquipSlotEnum) == null) //Try to equip
        {
            if (armor.TryAddItem(armorItem))
            {
                Items.Remove(armorItem);
                armorItem.Use(gameObject);

                return true;
            }
        }

        return false;
    }

    #endregion


    /// <summary>
    /// Tries to take the given item from the list by removing it and returns a bool indicating if it was successful.
    /// In EntityInventory it will also remove it from the weapons and armor list if it's in either of them.
    /// </summary>
    /// <param name="item">Item to drop.</param>
    /// <returns>True if item was found from the storage and was successfully removed.</returns>
    public override bool TryTakeItem(Item item)
    {
        bool result = false;

        if (base.TryTakeItem(item))
        {
            result = true;
        }

        if (item is WeaponItem)
        {
            //Cast
            WeaponItem weaponCast = (WeaponItem)item;

            if (weaponCast != null && weapons.Remove(weaponCast)) //Try to remove the casted weapon from the list
            {
                result = true;
            }
        }
        else if (item is ArmorItem)
        {
            //Cast
            ArmorItem armorCast = (ArmorItem)item;

            if (armorCast != null && armor.Remove(armorCast)) //Try to remove the casted armor from the list
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Tries to get a weapon from the currently equipped weapons that has the specific slot assigned on it.
    /// </summary>
    /// <param name="weaponSlot">Weapon slot to check for.</param>
    /// <returns>The weapon if found. Otherwise returns null.</returns>
    public WeaponItem GetWeaponFromSlot (WeaponSlotEnum weaponSlot)
    {
        foreach (WeaponItem weapon in weapons) //Loop through weapons
        {
            if (weapon.WeaponSlotEnum == weaponSlot) //Check for the right slot
            {
                return weapon;
            }
        }

        return null; //Null if not found
    }

    /// <summary>
    /// Tries to get an armor from the currently equipped ones that has the specific slot on it.
    /// </summary>
    /// <param name="armorSlot">armor slot to check for.</param>
    /// <returns></returns>
    public ArmorItem GetArmorFromSlot (ArmorSlotEnum armorSlot)
    {
        foreach (ArmorItem armor in armor) //Loop through armor
        {
            if (armor.ArmorEquipSlotEnum == armorSlot) //Check for the right slot
            {
                return armor;
            }
        }

        return null; //Null if not found
    }

    /// <summary>
    /// Drops the item in front of the player.
    /// </summary>
    /// <param name="itemToDrop"></param>
    public void DropItem(Item itemToDrop)
    {
        if (TryTakeItem(itemToDrop))
        {
            Vector3 playerPos = transform.position + new Vector3(0,1,0);
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            float spawnDistance = 1.5f;

            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

            var DefaultObject = Resources.Load<GameObject>("Props/DefaultItem");
            GameObject itemObject = Instantiate(DefaultObject, spawnPos, transform.rotation);

            ItemPickup itemPickup = itemObject.AddComponent<ItemPickup>();
            itemPickup.AddItem(itemToDrop);

            if (itemToDrop is WeaponItem)
            {
                WeaponItem weaponItem = (WeaponItem)itemToDrop;

                itemObject.GetComponent<MeshFilter>().sharedMesh = weaponItem.WeaponObject.GetComponent<MeshFilter>().sharedMesh;
                itemObject.GetComponent<MeshRenderer>().sharedMaterials = weaponItem.WeaponObject.GetComponent<MeshRenderer>().sharedMaterials;

            }
            else if (itemToDrop is ArmorItem)
            {
                ArmorItem armorItem = (ArmorItem)itemToDrop;

                itemObject.GetComponent<MeshFilter>().sharedMesh = armorItem.Mesh.sharedMesh;
                itemObject.GetComponent<MeshRenderer>().sharedMaterials = armorItem.Mesh.sharedMaterials;

            }
        }
    }
}