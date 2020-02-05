using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTestScript : MonoBehaviour
{
    protected void Start()
    {
        TradeManager.S_INSTANCE.StartTrade(GetComponent<EntityInventory>());
    }
}
