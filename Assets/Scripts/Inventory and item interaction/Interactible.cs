using UnityEngine;
using Venus.Utilities;

[RequireComponent(typeof(SphereCollider))]
public class Interactible : MonoBehaviour
{
     public GameObject player;

    [SerializeField]
    protected float sphereColliderRadius = 0.8f;

    protected virtual void Start()
    {
        player = PlayerManager.S_INSTANCE.player;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<SphereCollider>().radius = sphereColliderRadius;
    }

    public virtual void Interact (Collider other)
    {

    }

    public virtual void OnStayInteract(Collider other)
    {
        
    }

    public virtual void OnExitInteract(Collider other)
    {
       
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
            Interact(other);
    }

     void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        OnStayInteract(other);
    }

     void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        OnExitInteract(other);
        
    }


}


