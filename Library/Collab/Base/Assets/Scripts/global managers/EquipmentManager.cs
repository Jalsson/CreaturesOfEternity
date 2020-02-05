using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;

public class EquipmentManager : Singleton<EquipmentManager>
{
    

    public Equipment[] currentEquipment;
    public Weapons[] currentWeapons;
    SkinnedMeshRenderer[] currentMeshes;

    public List<GameObject> HeadArmorList = new List<GameObject>();
    public List<GameObject> TorsoArmorList = new List<GameObject>();
    public List<GameObject> BeltArmorList= new List<GameObject>();
    public List<GameObject> LegArmorList = new List<GameObject>();

    public Equipment OldItem;
    public SkinnedMeshRenderer targetMesh { get; private set; }
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    PlayerManager playerManager;
    GameObject player;
    public delegate void OnWeaponChanged(Weapons newWeapon, Weapons oldWeapon);
    public OnWeaponChanged onWeaponChanged;

    public InventoryUI inventoryUI;
    Inventory inventory;
    

    void Start()
    {
        playerManager = PlayerManager.S_INSTANCE;
        player = playerManager.player;
        inventory = Inventory.instance;
        targetMesh = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>();

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];

        int w_numSlots = System.Enum.GetNames(typeof(WeaponSlot)).Length;
        currentWeapons = new Weapons[w_numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];
        ListAllArmorsOnPlayer();

    }

    void ListAllArmorsOnPlayer()
    {
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("TorsoArmor"))
        {
            if (t.transform.root == this.transform)
            {
                TorsoArmorList.Add(t);
            }
        }

        foreach (GameObject h in GameObject.FindGameObjectsWithTag("HeadHelmet"))
        {
            if (h.transform.root == this.transform)
            {
                HeadArmorList.Add(h);
            }
        }

        foreach (GameObject b in GameObject.FindGameObjectsWithTag("BeltArmor"))
        {
            if (b.transform.root == this.transform)
            {
                BeltArmorList.Add(b);
            }
        }

        foreach (GameObject f in GameObject.FindGameObjectsWithTag("LegArmor"))
        {
            if (f.transform.root == this.transform)
            {
                LegArmorList.Add(f);
            }
        }
    }


    #region Equipments
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);
    

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        SetEquipmentBlendShapes(newItem, 100);
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.Mesh);
        newMesh.transform.parent = targetMesh.transform;
        currentMeshes[slotIndex] = newMesh;
        switch (newItem.equipSlot)
        {
            case EquipmentSlot.Head:
                newMesh.bones = HeadArmorList[newItem.ListIndex].GetComponent<SkinnedMeshRenderer>().bones;
                newMesh.enabled = true;
                break;
            case EquipmentSlot.Chest:
                newMesh.bones =  TorsoArmorList[newItem.ListIndex].GetComponent<SkinnedMeshRenderer>().bones;
                newMesh.enabled = true;
                break;
            case EquipmentSlot.Belt:
                newMesh.bones = BeltArmorList[newItem.ListIndex].GetComponent<SkinnedMeshRenderer>().bones;
                newMesh.enabled = true;
                break;
            case EquipmentSlot.Legs:
                newMesh.bones = LegArmorList[newItem.ListIndex].GetComponent<SkinnedMeshRenderer>().bones;
                newMesh.enabled = true;
                break;
        }
    }

    public void EquipWeapon(Weapons newWeapon)
    {
        int slotIndex = (int)newWeapon.weaponSlot;


        Weapons oldWeapon = null;

        if (currentWeapons[slotIndex] != null)
        {
            oldWeapon = currentWeapons[slotIndex];
            inventory.Add(oldWeapon);
        }

        if (onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newWeapon, oldWeapon);
        }

        currentWeapons[slotIndex] = newWeapon;
    }
        #endregion

    #region Unequipments
    public Equipment Unequip(int slotIndex)

    {
        if (currentEquipment[slotIndex] != null)
        {

            
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            SetEquipmentBlendShapes(oldItem, 0);
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            currentEquipment[slotIndex] = null;
            inventoryUI.UpdateUI();
            return oldItem;
            
        }
        
        return null;
        
    }
    public void WeaponUnequip(int slotIndex)

    {
        if (currentWeapons[slotIndex] != null)
        {
            Weapons oldWeapon = currentWeapons[slotIndex];
            inventory.Add(oldWeapon);

            currentWeapons[slotIndex] = null;

            if (onWeaponChanged != null)
            {
                onWeaponChanged.Invoke(null, oldWeapon);
            }
        }
    }
#endregion


    void SetEquipmentBlendShapes (Equipment item, int weight)
    {
        foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions)
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }

}




