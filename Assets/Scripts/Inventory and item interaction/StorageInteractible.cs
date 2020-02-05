using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInteractible : Interactible
{
    public Storage LocalStorage
    {
        set
        {
            localStorage = value;
        }
    }
    private InventoryUI inventoryUI;
    private GameObject storageInventoryUI;

    private Storage localStorage;

    protected override void Start()
    {
        sphereColliderRadius = 1.4f;
        inventoryUI = GameObject.Find("HUDCanvas").GetComponent<InventoryUI>();
        storageInventoryUI = inventoryUI.transform.Find("storageUI").gameObject;
        localStorage = GetComponent<Storage>();
        base.Start();
    }

    public override void Interact(Collider other)
    {
        if (other.tag == "Player")
        {
            OpenStorage();
        }
    }

    public override void OnExitInteract(Collider other)
    {
        if (other.tag == "Player")
        {
            OnExitZone();
        }
    }

    /// <summary>
    /// Opens the inventory UI and sets the open Storage to this.
    /// </summary>
    void OpenStorage()
    {
        if(!localStorage)
            localStorage = GetComponent<Storage>();

        if (InputManager.S_INSTANCE.UiState == UiStateEnum.Inventory)
        {
            inventoryUI.StorageState(true);
        }
        inventoryUI.UpdateStorageUI(localStorage);
        print("enterzone");
    }

    /// <summary>
    /// shuts down the storage viewer UI element
    /// </summary>
    void OnExitZone()
    {
        print("exitZone");
        inventoryUI.UpdateStorageUI(null);
        inventoryUI.StorageState(false);
    }
	
}
