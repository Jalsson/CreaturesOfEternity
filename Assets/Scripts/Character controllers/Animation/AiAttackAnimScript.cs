using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackAnimScript : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetFloat("AttackAnimation", Random.Range(0, 4));
        animator.SetBool("AttackTrigger", false);
        animator.SetBool("AttackAnimationPlaying", true);
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1 > 0.95)
        {
            animator.SetBool("AttackPlaying", false);
        }

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackAnimationPlaying", false);
    }

}
