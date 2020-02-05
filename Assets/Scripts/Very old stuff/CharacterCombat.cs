using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour {

    CharacterStats MyStats;
    HitBoxCollider hitBoxCollider;
    public Transform currentHitBox;

    public void Start()
    {
        MyStats = GetComponent<CharacterStats>();
    }
    
    public void TakeDamage(CharacterStats targetStats)
    {
        hitBoxCollider = currentHitBox.GetComponent<HitBoxCollider>();
        MyStats.TakeDamage(hitBoxCollider.damageMultiplayer);
    }
    
}
