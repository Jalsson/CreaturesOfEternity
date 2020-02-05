using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimContoller : MonoBehaviour {

    private Animator p_animator;
    private PlayerController PlayerController;
    

    private float a_turnAmout;
    private float a_ForwardAmount;
    public bool A_AttackTrigger { private set; get; }
    private float animationStyle = 0;

    private void Start()
    {
        // setting animator and disabling root motion so animations dont move the player during movement
        p_animator = GetComponent<Animator>();
        p_animator.applyRootMotion = false;

        PlayerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Set Attack animation trigger boolean
    /// </summary>
    /// <param name="triggerbool"> Wanted parameter</param>
    public void TriggerAttack(bool triggerbool)
    {
        A_AttackTrigger = triggerbool;
    }

    public void SetAttackStyle(float attackStyle)
    {
        animationStyle = attackStyle;
    }

    /// <summary>
    /// Updates Player character movement animator with given parameters
    /// </summary>
    /// <param name="input">Player input in Vector 2</param>
    /// <param name="targetLocation">Players current heading direction in Vector3 position</param>
    public void UpdateAnimator(Vector2 input,Vector3 targetLocation)
    {
        a_ForwardAmount = Mathf.Clamp(Mathf.Abs(input.x) + Mathf.Abs(input.y),0,1);

        a_turnAmout = Mathf.Atan2(transform.InverseTransformDirection(targetLocation).x, transform.InverseTransformDirection(targetLocation).z);


        p_animator.SetFloat("Forward", a_ForwardAmount);
        p_animator.SetFloat("Side", a_turnAmout);
        p_animator.SetFloat("AnimationStyle", animationStyle);
        p_animator.SetBool("AttackTrigger", A_AttackTrigger);
    }


    /*To clarify for new questions here:

On your child object with the animator, implement a script (you could call it AnimatorForward) with OnAnimatorMove which calls OnAnimatorMove on a target parent script.

In the OnAnimatorMove on that target script on your parent, apply Animator.deltaPosition to your current position - literally transform.position += myAnimator.deltaPosition;

Profit!

I hope that clears any lingering doubt.*/
}
