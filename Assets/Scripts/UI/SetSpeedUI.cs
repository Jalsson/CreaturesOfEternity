using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetSpeedUI : MonoBehaviour {

    PlayerStats playerStats;


    private void Start()
    {
        if (PlayerManager.S_INSTANCE.player)
        {

            playerStats = PlayerManager.S_INSTANCE.player.GetComponent<PlayerStats>();
            playerStats.UpdateStatus += SetSpeedValue;
        }
        
    }

    public void SetSpeedValue()
    {
        GetComponent<TextMeshProUGUI>().text = Mathf.Round((100 * playerStats.MovementModfiers.GetValue())).ToString();
    }
}
