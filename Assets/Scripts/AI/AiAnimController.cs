using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class AiAnimController : MonoBehaviour
{
	public float m_MovingTurnSpeed = 360;
	public float m_StationaryTurnSpeed = 180;
	public float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	public float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	public float m_AnimSpeedMultiplier = 1f;
	public float m_GroundCheckDistance = 0.1f;
    public float m_AttackStyle = 1f;

	Rigidbody m_Rigidbody;
	public Animator m_Animator;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
    float m_sideAmount;
	float m_ForwardAmount;
    Vector3 m_GroundNormal;

    public bool m_CombatMode;
    public bool m_AttackTrigger = false;
    public bool inventoryOn;
    float cameraSteps;
    public float m_AttackCircleDirection;

    public CharacterStats characterStats { private set; get; }
    private EntityInventory inventory;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        characterStats = GetComponent<CharacterStats>();
        inventory = GetComponent<EntityInventory>();
    }

    void Start()
	{
        m_CombatMode = false;
        inventoryOn = false;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
        characterStats.Dying += DeadAnimation;
	}

    /// <summary>
    ///                  convert the world relative moveInput vector into a local-relative
    ///                  turn amount and forward amount required to head in the desired direction
    /// </summary>
    /// <param name="move"></param>
    /// <param name="crouch"></param>
    /// <param name="jump"></param>
    public void Move(Vector3 move)
    {
            

            if (m_CombatMode != true)
            {
                if (move.magnitude > 1f) move.Normalize();
                move = transform.InverseTransformDirection(move);
                move = Vector3.ProjectOnPlane(move, m_GroundNormal);
                m_TurnAmount = Mathf.Atan2(move.x, move.z);
                m_ForwardAmount = move.z;
                m_Animator.SetFloat("RunSpeed", characterStats.MovementModfiers.GetValue());
            ApplyExtraTurnRotation();


            }
            if (m_CombatMode == true)
            {
                
                if (move.magnitude > 1f) move.Normalize();
                move = transform.InverseTransformDirection(move);
                    
                move = Vector3.ProjectOnPlane(move, m_GroundNormal);
                m_ForwardAmount = move.z;
                m_sideAmount = move.x;


                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            }

            // send input and other state parameters to the animator
            UpdateAnimator(move);
            

    }

 
    public void DeadAnimation()
    {
        m_Animator.SetBool("Dead", true);
        m_Rigidbody.isKinematic = true;
    }

	void UpdateAnimator(Vector3 move)
	{
        if (m_Animator)
        {
            // update the animator parameters
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Sideway", m_sideAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("combatMode", m_CombatMode);
            m_Animator.SetBool("AttackTrigger", m_AttackTrigger);
            m_Animator.SetFloat("AttackStyle", m_AttackStyle);
            m_Animator.SetFloat("AttackCircleDirection", m_AttackCircleDirection);



            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;


            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                m_Animator.speed = 1;
            }
        }
	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}


	public void OnAnimatorMove()
	{
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.

        float m_MoveSpeedMultiplier = 1f;

        if (characterStats)
        {
            m_MoveSpeedMultiplier = characterStats.MovementModfiers.GetValue();
        }
            
		if (Time.deltaTime > 0 )
		{
            if (m_Animator)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }

		}

    }
        
}