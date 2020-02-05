using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimContoller : MonoBehaviour {

    public Animator P_animator { private set; get; }

    #region Fields
    public bool TriggerDash
    {
        get
        {
            return a_DashTrigger;
        }

        set
        {
            a_DashTrigger = value;
        }
    }

    public bool TriggerAttack
    {
        set
        {
           a_AttackTrigger = value;
        }
    }

    public int AttackStyle
    {
        set
        {
            attackStyle = value;
        }
    }
    #endregion

    // drives movement animation blend spaces
    private float a_turnAmout;
    private float a_ForwardAmount;

    // used for triggering animations
    private bool a_AttackTrigger;
    private bool a_DashTrigger;
    private int attackStyle = 0;
    private float animationStyle = 0;

    private void Start()
    {
        // setting animator and disabling root motion so animations dont move the player during movement
        P_animator = GetComponentInChildren<Animator>();
        P_animator.applyRootMotion = false;
    }


    public void SetAttackStyle(float animationStyle)
    {
        this.animationStyle = animationStyle;
    }

    public void SetAnimatorLayerWeight(int layer, float weight)
    {
        P_animator.SetLayerWeight(layer, weight);
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


        P_animator.SetFloat("Forward", a_ForwardAmount);
        P_animator.SetFloat("Side", a_turnAmout);
        P_animator.SetFloat("AnimationStyle", animationStyle);
        P_animator.SetBool("AttackTrigger", a_AttackTrigger);
        P_animator.SetBool("DashTrigger", a_DashTrigger);
        P_animator.SetInteger("AttackStyle", attackStyle);
    }

}
