using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour {

    CharacterCombat characterCombat;
    CharacterStats targetStats;

    public int damageMultiplayer;

    public void Start()
    {
        characterCombat = GetComponentInParent<CharacterCombat>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            characterCombat.currentHitBox = this.transform;
            targetStats = other.GetComponentInParent<CharacterStats>();
            characterCombat.TakeDamage(targetStats);
        }
    }

}
