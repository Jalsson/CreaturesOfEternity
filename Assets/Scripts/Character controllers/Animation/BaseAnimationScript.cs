using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimationScript : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AnimationPlaying", true);
        animator.applyRootMotion = true;
    }

    // Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (stateInfo.normalizedTime % 1 > 0.95f)
        {
            animator.SetBool("AnimationPlaying", false);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AnimationPlaying", false);
        animator.applyRootMotion = false;
    }
}
