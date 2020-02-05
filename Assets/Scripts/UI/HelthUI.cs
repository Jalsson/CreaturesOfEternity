using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthUI : MonoBehaviour {

    Slider slider;
    PlayerStats playerStats;
    private void Start()
    {
        if (PlayerManager.S_INSTANCE.player)
        {
            slider = GetComponent<Slider>();
            playerStats = PlayerManager.S_INSTANCE.player.GetComponent<PlayerStats>();
            playerStats.TookDamage += SetValue;
            playerStats.UpdateStatus += SetValue;
        }

    }

    public void SetValue()
    {
        slider.value = playerStats.currentHealth;
    }

}
