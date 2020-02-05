using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimScript : BaseAnimationScript{


    /// Set attack trigger to false if player wants to continue combo
    override public  void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<PlayerAnimContoller>().TriggerAttack = false;
        animator.SetBool("AttackAnimationPlaying", true);
        animator.SetInteger("AttackStyle", 0);
        animator.applyRootMotion = true;
        animator.SetFloat("AttackAnimation", Random.Range(0, 2));
    }

    /// If attack trigger is false, stop playing animation and return to normal movement state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1 > 0.4f && !animator.GetBool("AttackTrigger"))
        {
            animator.SetBool("AttackAnimationPlaying", false);
        }
        if (stateInfo.normalizedTime % 1 > 0.66f && !animator.GetBool("AttackTrigger"))
        {
            animator.SetBool("AttackAnimationPlaying", false);
        }
        else if (stateInfo.normalizedTime % 1 > 0.95f)
        {
            animator.SetBool("AttackAnimationPlaying", false);
        }
    }
    
    // Set attack trigger to false when exiting the attack animation state.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<PlayerAnimContoller>().TriggerAttack = false;
        animator.SetBool("AttackAnimationPlaying", false);
        animator.applyRootMotion = false;
    }
}
