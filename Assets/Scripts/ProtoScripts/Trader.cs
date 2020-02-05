using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Interaction;

public class Trader : EntityInventory, IInteractable
{
    /// <summary>
    /// Returns the transform of this object.
    /// </summary>
    /// <returns>Gameobject's transform.</returns>
    public Transform GetTransform ()
    {
        return transform;
    }

    /// <summary>
    /// Triggers the trade between the player and this entity
    /// </summary>
    public void Interact ()
    {
        TradeManager.S_INSTANCE.StartTrade(this);
    }
}
