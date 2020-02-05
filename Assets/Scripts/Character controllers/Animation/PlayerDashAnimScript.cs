using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAnimScript : StateMachineBehaviour {

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // stopping animation when it's 0.95% completed
        if (stateInfo.normalizedTime % 1 > 0.95f)
        {
            animator.gameObject.GetComponentInParent<PlayerAnimContoller>().TriggerDash = false;
        }
    }

}
