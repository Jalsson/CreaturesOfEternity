using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;

public class Storage : MonoBehaviour
{
    /// <summary>
    /// Items in storage.
    /// </summary>
    public List<Item> Items
    {
        get
        {
            return items;
        }
    }

    /// <summary>
    /// The maximum amount of items that can be stored in this storage.
    /// </summary>
    protected int maxItemAmount = 16;

    /// <summary>
    /// Items currently in this storage.
    /// </summary>
    [SerializeField]
    private List<Item> items = new List<Item>();

    /// <summary>
    /// Tries to store the given item to this storage.
    /// </summary>
    /// <param name="item">Item to try to store in the storage.</param>
    /// <returns>True if storing the item was successful.</returns>
    public virtual bool TryStoreItem (Item item)
    {
        if (items.Count < maxItemAmount)
        {
            //Add item to the list
            items.Add(item);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the amount of free spaces left.
    /// </summary>
    /// <returns></returns>
    public int FreeSlotsLeft ()
    {
        return maxItemAmount - items.Count;
    }

    /// <summary>
    /// Checks if the item is in this storage.
    /// </summary>
    /// <param name="item">Item to check for.</param>
    /// <returns>True if the item was found.</returns>
    public bool ContainsItem (Item item)
    {
        return items.Contains(item);
    }

    /// <summary>
    /// Tries to take the given item from the list by removing it and returns a bool indicating if it was successful.
    /// </summary>
    /// <param name="item">Item to drop.</param>
    /// <returns>True if item was found from the storage and was successfully removed.</returns>
    public virtual bool TryTakeItem (Item item)
    {
        return items.Remove(item);
    }

    /// <summary>
    /// Sets the max item amount to the given value.
    /// </summary>
    /// <param name="value">New max item amount.</param>
    public void SetMaxItemAmount (int value)
    {
        maxItemAmount = value;
    }
}
