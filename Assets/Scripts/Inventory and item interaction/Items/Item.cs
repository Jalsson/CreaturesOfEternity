using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    public string Name
    {
        get { return name; }
    }

    public Sprite Icon
    {
        get { return icon; }
    }

    public string Description
    {
        get { return description; }
    }

    public int Price
    {
        get { return price; }
    }

    public RarityEnum RarityEnum
    {
        get { return rarityEnum; }
    }

    [SerializeField]
    new protected string name = "New Item";

    [SerializeField]
    protected Sprite icon = null;

    [SerializeField]
    protected string description = "infoText";

    [SerializeField]
    protected int price;

    [SerializeField]
    protected RarityEnum rarityEnum;

    /// <summary>
    /// Storage that this item is currently in
    /// </summary>
    protected Storage currentStorage;

    public virtual void Use()
    {
        Debug.Log("Using" + name);
    }


    public void RemoveFromInventory()
    {
        Debug.Log("trying to remove" + name + "from inventory");
    }

    public void SetItemInStorage (Storage storageSetIn)
    {
        //Stuff to do to hide the gameobject
        currentStorage = storageSetIn;
    }


}

public enum RarityEnum { Common, Uncommon, Rare, Legendary}