using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : Interactible
{

    protected override void Start()
    {
        sphereColliderRadius = 0.3f;
        sphereColliderRadius = 0.3f;
        GetComponent<Rigidbody>().useGravity = true;
        if (GetComponent<CapsuleCollider>() != null)
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
        base.Start();
    }
    public Item Item
    {
        get
        {
            return item;
        }
    }

    [SerializeField]
    private Item item;

    public void AddItem (Item item)
    {
        this.item = item;
    }


    private void OnTriggerEnter(Collider other)
    {
        PickUp(other);
    }

    protected virtual void PickUp(Collider other)
    {
        EntityInventory entityInventory = other.GetComponent<EntityInventory>();

        if (entityInventory != null)
        {
            PickUp(entityInventory);
        }
    }

    protected virtual void PickUp (EntityInventory entityInventory)
    {

        if (entityInventory.TryEquipItem(Item))
        {
            Destroy(gameObject);
        }
        else if (entityInventory.TryStoreItem(Item))
        {
            Destroy(gameObject);
        }
    }

}

