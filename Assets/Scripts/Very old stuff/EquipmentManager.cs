using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;

public class EquipmentManager : Singleton<EquipmentManager>
{


    List<Transform> bones = new List<Transform>();

    public SkinnedMeshRenderer[] armorReferenceList;

    protected override void Awake()
    {

        // get all armor references so their bone information can be used later
        armorReferenceList = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (Transform item in GameObject.Find("Player").transform.GetComponentsInChildren<Transform>())
        {
            if (item.tag == "Bone")
            {
                bones.Add(item);
            }
        }
        int numSlots = System.Enum.GetNames(typeof(ArmorSlotEnum)).Length;

        int w_numSlots = System.Enum.GetNames(typeof(WeaponSlotEnum)).Length;
        base.Awake();

    }





}




