using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAndWeaponEquipper : MonoBehaviour {

    EquipmentManager equipmentManager;
    SkinnedMeshRenderer targetMesh;
    CharacterStats characterStats;

    Transform[] refBones = null;
    Transform[] characterBones = null;

    public Transform[] weaponParents { private set; get; }

    GameObject[] equipedArmors = new GameObject[(int)ArmorSlotEnum.NumberOfTypes];
    public GameObject[] EquipedWeapons { private set; get; }

    /// <summary>
    /// This start method gets all owner characters bone game objects and stores them in to a array.
    /// </summary>
    private void Start()
    {
        weaponParents = new Transform[2];
        EquipedWeapons = new GameObject[(int)WeaponSlotEnum.NumberOfTypes];
        characterStats = GetComponent<CharacterStats>();



        equipmentManager = EquipmentManager.S_INSTANCE;
        targetMesh = GetComponentInChildren<SkinnedMeshRenderer>();

        List<Transform> tempBoneList = new List<Transform>();
        foreach (Transform childTransform in transform.GetComponentsInChildren<Transform>())
        {
            if (childTransform.tag == "Bone")
                tempBoneList.Add(childTransform);

            if (childTransform.name == "WeaponParent0")
                weaponParents[0] = childTransform;

            if (childTransform.name == "WeaponParent1")
                weaponParents[1] = childTransform;
        }

        characterBones = tempBoneList.ToArray();

    }

    #region Equipments
    /// <summary>
    /// This function equips new skinned mesh renderer to script owners character. This instanciates a new Object and attaches it to player parent.
    /// <p> After this it gets reference bone names from equipment manager and loops through it's own boned and sets correct transforms to correct places
    /// <p>After this mesh should be visible and follow character correctly
    /// <p>Also aplyes the armor and movement speed modifiers of armor to character.
    /// </summary>
    /// <param name="newItem"> scriptable object that holds needed information to spawn a armor mesh.</param>
    public void EquipArmor(ArmorItem newItem)
    {


        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.Mesh);
        
        characterStats.ArmorModifiers.AddModifier(newItem.DamagaReduction);
        characterStats.MovementModfiers.AddModifier(newItem.MovementSlow);


        equipedArmors[(int)newItem.ArmorEquipSlotEnum] = newMesh.gameObject;
        Transform[] tempBoneList = new Transform[newMesh.bones.Length];

        // Shrink the mesh that it dosent clip through armor
        SetEquipmentBlendShapes(newItem, 100);
        
        //set parent under skinnder mesh
        newMesh.transform.parent = targetMesh.transform;

        for (int i = 0; i < equipmentManager.armorReferenceList.Length; i++)
        {
            if (newItem.Mesh.name == equipmentManager.armorReferenceList[i].name)
            {
                refBones = equipmentManager.armorReferenceList[i].bones;
            }
        }

        
        for (int i = 0; i < newMesh.bones.Length; i++)
        {
            string oldBoneName = refBones[i].name;

            for (int o = 0; o < characterBones.Length; o++)
            {
                if (oldBoneName == characterBones[o].name)
                {
                    tempBoneList[i] = characterBones[o];
                }
            }
        }
        newMesh.bones = tempBoneList;
        newMesh.enabled = true;

        if (characterStats is PlayerStats)
        {
            PlayerStats playerStats = (PlayerStats)characterStats;
            playerStats.UpdateStatus();
        }
    }

    /// <summary>
    /// Spawns given gameObject to proper WeaponSlot and stores reference to it for later if it needs to be deleted
    /// </summary>
    /// <param name="newItem"></param>
    public void EquipWeapon(WeaponItem newItem)
    {
        GameObject newWeaponObject = Instantiate(newItem.WeaponObject);

        int weaponSlot = (int)newItem.WeaponSlotEnum;
        newWeaponObject.GetComponentInChildren<BaseWeaponClass>().SetWeaponItem(newItem);

        EquipedWeapons[weaponSlot] = newWeaponObject;

        newWeaponObject.transform.position = weaponParents[weaponSlot].position;
        newWeaponObject.transform.parent = weaponParents[weaponSlot];

        if (newItem.CrystalTypeEnum != CrystalTypeEnum.None)
        {
            if (GetComponent<PlayerAnimContoller>())
            {
                GetComponent<PlayerAnimContoller>().SetAnimatorLayerWeight(weaponSlot + 1, 1);
            }
        }
    }

    #endregion

    #region Unequipments
    /// <summary>
    /// Destroys the equiped game object depending on it's class(weaponItem and ArmorItem)
    /// </summary>
    /// <param name="meshToUnequip">Item that you want to be destroyed</param>
    public void Unequip(Item itemToUnequip)
    {
        if (itemToUnequip is WeaponItem)
        {
            WeaponItem weaponItem = itemToUnequip as WeaponItem;
            int weaponSlotInt = (int)weaponItem.WeaponSlotEnum;

            if (EquipedWeapons[weaponSlotInt])
            {
                if (EquipedWeapons[weaponSlotInt].transform.IsChildOf(transform))
                {
                    
                    EquipedWeapons[weaponSlotInt].GetComponentInChildren<BaseWeaponClass>().Unequip();
                    Destroy(EquipedWeapons[weaponSlotInt]);
                }
                EquipedWeapons[weaponSlotInt] = null;

                if (weaponItem.CrystalTypeEnum != CrystalTypeEnum.None)
                {
                    if (GetComponent<PlayerAnimContoller>())
                    {
                        GetComponent<PlayerAnimContoller>().SetAnimatorLayerWeight(weaponSlotInt + 1, 0);
                    }
                }
            }

            
        }
        else if(itemToUnequip is ArmorItem)
        {
            ArmorItem armorItem = itemToUnequip as ArmorItem;
            int armorSlotInt = (int)armorItem.ArmorEquipSlotEnum;
            if (equipedArmors[armorSlotInt])
            {
                characterStats.ArmorModifiers.RemoveModifier(armorItem.DamagaReduction);
                characterStats.MovementModfiers.RemoveModifier(armorItem.MovementSlow);

                Destroy(equipedArmors[armorSlotInt]);
                equipedArmors[armorSlotInt] = null;
                SetEquipmentBlendShapes(armorItem, 0);
            }
        }
    }
    #endregion

        /// <summary>
        /// shrinks the needed mesh regions down that character mesh dosen't clip through the armor
        /// </summary>
        /// <param name="item">item that is going to be equipped. This will give us the right areas that need shrinking</param>
        /// <param name="weight">amount of shrinking that you want to set. 0 is none and 100 is maxinum amount</param>
    void SetEquipmentBlendShapes(ArmorItem item, int weight)
    {
        foreach (ArmorMeshRegion blendShape in item.CoveredMeshRegionsEnums)
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }
    
}
