using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {
    /// <summary>
    /// Storage that this item is currently in
    /// </summary>
    protected Storage currentStorage;

    //Components
    protected new Renderer renderer;
    protected new Collider collider;
    protected new Rigidbody rigidbody;

    public string itemName { get; protected set; }
    public Sprite icon { get; protected set; }
    public string InfoText { get; protected set; }

protected void Start()
    {
        //Get references
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called when the item is set into a storage. Disables its physical and visible traits.
    /// </summary>
    /// <param name="storageSetIn">Storage that this item was set in.</param>
    public void SetItemInStorage(Storage storageSetIn)
    {
        renderer.enabled = false; //Disable graphics
        collider.enabled = false; //Disable collision
        rigidbody.isKinematic = true; //Freeze physics

        //Reference torage
        currentStorage = storageSetIn;

        //Set as a child of the storage
        transform.parent = storageSetIn.transform;

        //Set pos to center of the storage
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Called when the item is set into world instead of a storage. Enables its physical and visible traits.
    /// </summary>
    public void SetItemInWorld()
    {
        renderer.enabled = true; //Enable graphics
        collider.enabled = true; //Enable collision
        rigidbody.isKinematic = false; //Enable physics

        //Dereference storage
        currentStorage = null;

        //Deparent
        transform.parent = null;

        //Make sure no previous velocities will affect this
        rigidbody.velocity = Vector3.zero;
    }
}