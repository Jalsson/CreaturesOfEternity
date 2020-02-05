using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimScript : StateMachineBehaviour {

    [SerializeField]
    private float rootMotionMultiplier = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackTrigger", false);
        animator.gameObject.GetComponent<PlayerAnimContoller>().TriggerAttack(false);
        animator.SetBool("AnimationPlaying", true);
        animator.applyRootMotion = true;
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1 > 0.5f)
        {
            
            animator.SetBool("AnimationPlaying", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AnimationPlaying", false);
        //animator.applyRootMotion = false;
        
    }
}
