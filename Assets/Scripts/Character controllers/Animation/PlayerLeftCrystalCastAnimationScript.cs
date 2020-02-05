using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftCrystalCastAnimationScript : StateMachineBehaviour {

    bool activated = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.gameObject.GetComponentInParent<PlayerAnimContoller>().TriggerAttack = false;
        animator.SetBool("CrystalAnimationPlaying", true);
        animator.SetInteger("AttackStyle", 0);


    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (stateInfo.normalizedTime % 1 > 0.95f)
        {

            if (!activated)
            {
                animator.SetBool("CrystalAnimationPlaying", false);
                animator.SetInteger("AttackStyle", 4);
                animator.gameObject.GetComponentInParent<PlayerController>().ActivateCrystal(layerIndex-1);
                activated = true;
            }
            animator.SetBool("CrystalAnimationPlaying", false);
            animator.SetInteger("AttackStyle", 4);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CrystalAnimationPlaying", false);
        animator.SetInteger("AttackStyle", 0);
        animator.SetLayerWeight(layerIndex, 0);

        activated = false;
    }
}
