using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour {

    [SerializeField]
    private Item[] items;

    private void Start()
    {
        foreach (Item item in items)
        {
            Debug.Log(item.GetHashCode());
        }
    }
}
