using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.QuestSystem;

public class QuestItemPickup : ItemPickup
{
    [Space]
    /// <summary>
    /// The string id of the quest that gets completed when this gets picked up.
    /// </summary>
    [SerializeField] [Tooltip("The string id of the quest that gets completed when this gets picked up.")]
    private string questStringID;

    protected override void PickUp(EntityInventory entityInventory)
    {
        //Check if entity
        if (entityInventory.gameObject == player && QuestManager.S_INSTANCE.CurrentMission.QuestStringID == questStringID)
        {
            //Quest done
            QuestManager.S_INSTANCE.CurrentMission.EndMission();

            base.PickUp(entityInventory);
        }
    }
}
