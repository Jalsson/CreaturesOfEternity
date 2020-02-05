using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClass : BaseWeaponClass {

    CharacterStats stats;
    private CharacterStats ownerCharcter;
    private Animator animator;

    /// <summary>
    /// Sets the ownerCharacter that class dosent kill it's owner.
    /// Gets Animator so that class can only damage when attack animation is playing.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if (GetComponentInParent<CharacterStats>())
        {
            ownerCharcter = GetComponentInParent<CharacterStats>();
            if (GetComponentInParent<Animator>())
            {
                animator = GetComponentInParent<Animator>();
                animator.SetFloat("AttackSpeedMultiplier", weaponItem.AttackSpeedOrDuration);
            }
        }

    }

    /// <summary>
    /// Damages the character when attacking.
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter(Collider col)
    {
        if (animator.GetBool("AttackAnimationPlaying"))
        {
            if (col.gameObject.GetComponent<CharacterStats>())
            {
                if (!col.gameObject.GetComponent<CharacterStats>().tookDamageRecently)
                {
                    if (col.gameObject != ownerCharcter.gameObject)
                    {
                        stats = col.gameObject.GetComponent<CharacterStats>();
                        stats.TakeDamage(weaponItem.WeaponDamage * weaponDamageModfier);
                    }
                }
            }
        }
    }

    public override void Unequip()
    {
        animator.SetFloat("AttackSpeedMultiplier", 1);
    }
    
}
