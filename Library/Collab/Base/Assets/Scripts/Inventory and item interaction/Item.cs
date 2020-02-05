using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public bool IsDefaultItem = false;
    public string InfoText = "infoText";

    /// <summary>
    /// Storage that this item is currently in
    /// </summary>
    protected Storage currentStorage;

    public virtual void Use()
    {
        Debug.Log("Using" + name);

    }
    public virtual void Unequip()
    {

    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

    public void SetItemInStorage (Storage storageSetIn)
    {
        //Stuff to do to hide the gameobject
        currentStorage = storageSetIn;

        
    }
}
