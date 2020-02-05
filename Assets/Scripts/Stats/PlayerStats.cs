using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Venus.Delegates;

public class PlayerStats : CharacterStats {


    public VoidDelegate UpdateStatus;

    private void Start()
    {
        Dying += onDeath;
    }

    private void onDeath ()
    {
        if (SceneManager.GetActiveScene().name == "Intro") //Intro
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (SceneManager.GetActiveScene().name == "CaveLevel") //Cave level
        {
            EntityInventory inv = PlayerManager.S_INSTANCE.player.GetComponent<EntityInventory>();

            PlayerManager.S_INSTANCE.SavePlayer();

            SceneManager.LoadScene("Hub");
        }
    }

    public override void SpawnLoot()
    {
        
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        UpdateStatus();
        TookDamage();
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        UpdateStatus();
    }
}
