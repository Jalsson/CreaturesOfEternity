using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStats : CharacterStats {

    ItemPickup[] rocks;

    private void Start()
    {
        rocks = GetComponentsInChildren<ItemPickup>();

    }

    public override void TakeDamage(float damageAmount)
    {
        AudioManager.S_INSTANCE.Play("crystal");
        tookDamageRecently = true;
        StartCoroutine(ResetDamageTick(nextDamageTickTime));

        damaged = true;
        damageAmount = damageAmount * ArmorModifiers.GetValue();
        damageAmount = Mathf.Clamp(damageAmount, 0, int.MaxValue);

        currentHealth -= damageAmount;
        Debug.Log(transform.name + " takes " + damageAmount + " damage.");


        if (currentHealth <= 0 && !dead)
        {
            characterDying();

        }
      
    }



        

    public override void SpawnLoot()
    {

        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i].transform.parent = null;
            rocks[i].enabled = true;
        }
        Destroy(this.gameObject);
    }
}
