using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetArmorUI : MonoBehaviour {

    PlayerStats playerStats;


    private void Start()
    {
        if (PlayerManager.S_INSTANCE.player)
        {
            playerStats = PlayerManager.S_INSTANCE.player.GetComponent<PlayerStats>();
            playerStats.UpdateStatus += SetArmorValue;
        }
    }

    public void SetArmorValue()
    {
        float tempNumber = 100 + playerStats.ArmorModifiers.GetOriginalValue();
        GetComponent<TextMeshProUGUI>().text = Mathf.Round(tempNumber).ToString();
    }
}
