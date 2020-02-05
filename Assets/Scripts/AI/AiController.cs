using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

    public RarityEnum RarityLevel
    {
        get { return rarityLevel; }
        set { rarityLevel = value; }
    }

    public int MaxCountOfItems { set { maxCountofItems = value; } }

    protected EntityInventory entityInventory;
    protected AiMovementController movementController;

    [SerializeField]
    protected RarityEnum rarityLevel;

    [SerializeField]
    protected int minCountOfItems;

    [SerializeField]
    protected int maxCountofItems;

    protected virtual void Start()
    {
        if (GetComponent<EntityInventory>())
            entityInventory = GetComponent<EntityInventory>();

        else if(GetComponentInParent<EntityInventory>())
            entityInventory = GetComponentInParent<EntityInventory>();
        

        if (GetComponent<AiMovementController>())
            movementController = GetComponent<AiMovementController>();

        else if(GetComponentInParent<AiMovementController>())
            movementController = GetComponentInParent<AiMovementController>();

        StartCoroutine(EquipAI());
    }



    IEnumerator EquipAI()
    {
        yield return new WaitForSeconds(1);
        ItemAiManager.S_INSTANCE.EquipEquipment(entityInventory, rarityLevel, Random.Range(minCountOfItems, maxCountofItems));
    }

}
