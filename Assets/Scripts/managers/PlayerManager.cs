using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;


    public class PlayerManager : Singleton<PlayerManager> {

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    

    public GameObject player { get; private set; }

    private void Start()
    {
        ItemObject newItem = new ItemObject();
        LoadPlayer();
    }
    public void SavePlayer()
    {
        List<string> itemsToSave = new List<string>();
        List<string> armorToSave = new List<string>();
        List<string> weaponsToSave = new List<string>();
        EntityInventory playerInventory = player.GetComponent<EntityInventory>();

        for (int i = 0; i < playerInventory.Items.Count; i++)
        {
            itemsToSave.Add(playerInventory.Items[i].name);
        }

        for (int i = 0; i < playerInventory.EntityArmor.Count; i++)
        {
            armorToSave.Add(playerInventory.EntityArmor[i].name);
        }
        for (int i = 0; i < playerInventory.EntityWeapons.Count; i++)
        {
            weaponsToSave.Add(playerInventory.EntityWeapons[i].name);
        }
        if(playerInventory.Items.Count > 0)
        {
            GetComponent<SaveData>().Save(itemsToSave.ToArray(), "PlayerItems", "Arraysize1");
        }
        if (playerInventory.EntityArmor.Count > 0)
        {
            GetComponent<SaveData>().Save(armorToSave.ToArray(), "PlayerArmor", "Arraysize2");
        }
        if(playerInventory.EntityWeapons.Count > 0)
        {
            GetComponent<SaveData>().Save(weaponsToSave.ToArray(), "PlayerWeapons", "Arraysize3");
        }
        

    }

    public void LoadPlayer()
    {
        string[] itemsToLoad = GetComponent<SaveData>().Load("PlayerItems", "Arraysize1");
        string[] armorToLoad = GetComponent<SaveData>().Load("PlayerArmor", "Arraysize2");
        string[] weaponsToLoad = GetComponent<SaveData>().Load("PlayerWeapons", "Arraysize3");

        ItemAiManager itemManager = GetComponent<ItemAiManager>();
        EntityInventory playerInventory = player.GetComponent<EntityInventory>();
        List<Item> items = new List<Item>();

        playerInventory.Items.Clear();
        playerInventory.EntityWeapons.Clear();
        playerInventory.EntityArmor.Clear();

        for (int i = 0; i < GetComponent<ItemAiManager>().allItems.Length; i++)
        {
            items.Add((Item)itemManager.allItems[i]);
        }
        for (int i = 0; i < armorToLoad.Length; i++)
        {
            Item itemToAdd = null;

            for (int z = 0; z < items.Count; z++)
            {
                if (items[z].name == armorToLoad[i])
                {
                    itemToAdd = items[z];
                }
            }
            if (itemToAdd != null)
            {
                if (playerInventory.TryEquipItem(itemToAdd))
                {

                }
                else
                    Debug.LogWarning("warning: did not find the " + items[i] + " item");
            }
        }

        for (int i = 0; i < weaponsToLoad.Length; i++)
        {
            Item itemToAdd = null;

            for (int z = 0; z < items.Count; z++)
            {
                if (items[z].name == weaponsToLoad[i])
                {
                    itemToAdd = items[z];
                }
            }
            if (itemToAdd != null)
            {
                if (playerInventory.TryEquipItem(itemToAdd))
                {

                }
                else
                    Debug.LogWarning("warning: did not find the " + weaponsToLoad[i] + " item");
            }
        }  

        for (int i = 0; i < itemsToLoad.Length; i++)
        {
            Item itemToAdd = null;

            for (int z = 0; z < items.Count; z++)
            {
                if (items[z].name == itemsToLoad[i])
                {
                    itemToAdd = items[z];
                }
            }
            if (itemToAdd != null)
            {
                if (playerInventory.TryStoreItem(itemToAdd))
                {
                    print("itemi lisätty :3");
                }
            }
        }


    }
}

