using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : Interactible {

#region Singleton
    public static ChestInventory instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int space = 12;
    public Item[] itemArray;

    public delegate void OnItemChanged();
    public OnItemChanged onChestItemChangedCallback;

    public List<Item> items = new List<Item>();


     void Start()
    {

        Add(itemArray[0]);
    }

    public override void Interact(Collider other)
    {
        instance = this;
    }
    public override void OnExitInteract(Collider other)
    {
        instance = null;
    }
    public bool Add (Item item)
    {
        for (int i = 0; i < space; i++)
        {   
            if(items.Count >= space)
                {
                    Debug.Log("Not enough room.");
                    return false;
                }
            if(itemArray[i] != null) 
            items.Add(itemArray[i]);

            if(onChestItemChangedCallback != null)
            onChestItemChangedCallback.Invoke();
          
            
        }
        return true;
    }

    public void Transfer(Item item)
    {
        if (onChestItemChangedCallback != null)
        {
         Inventory.instance.Add(item);
        }
       
    }

    public void Remove(Item item)
    {
           
        if (onChestItemChangedCallback != null)
        {
            onChestItemChangedCallback.Invoke();
            items.Remove(item);
        }
            
    }



}
