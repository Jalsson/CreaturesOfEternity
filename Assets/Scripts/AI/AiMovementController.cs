using System.Collections;
using UnityEngine;
using UnityEngine.AI;


    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (AiAnimController))]
    public class AiMovementController : MonoBehaviour
    {
        PlayerManager playerManager;
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public AiAnimController character;                   // the character we are controlling
                                                                                      // target to aim for
        ItemAiManager aiManager;
        
        public AiBehaviorEnum behavior;
        public float lookRadius = 10f;
        private float[] stoppingDistances;
        bool isAttackedOnce = false;
        public int positionInRange;
        bool canAttack = false;
        public int offsetNumber;

        private bool combatIdleState = false;

        private void Start()
        {
            stoppingDistances = new float[6] { 1.4f, 2f, 0, 0, 3.5f, 4 };
            aiManager = ItemAiManager.S_INSTANCE;
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<AiAnimController>();

	        agent.updateRotation = true;
	        agent.updatePosition = true;

            agent.stoppingDistance = stoppingDistances[0];

            
            character.Move(Vector3.zero);
            character.characterStats.Dying += AgentStop;
            stoppingDistances[2] = stoppingDistances[0] + 0.4f;
            stoppingDistances[3] = stoppingDistances[1] + 0.4f;
        }
        /// <summary>
        /// Handles all AI behavior depending of distnace to player character.
        /// </summary>
        public float MoveToTarget(Vector3 target,AiBehaviorEnum aiBehavior,float InteractionDistance)
    {
        behavior = aiBehavior;
        float distance = Vector3.Distance(target, transform.position);

            switch (aiBehavior)
            {
                case AiBehaviorEnum.Attack:

                    CannotAttack(target);
                    if (target != null)
                    {
                        agent.SetDestination(target);
                    }

                    if (distance < stoppingDistances[4])
                    {
                        DefineAttackerDirAndPlaceOnList();
                    }

                    if (distance > stoppingDistances[5])
                    {
                        if (aiManager.aiControllerList.Contains(this))
                        {
                            aiManager.aiControllerList.Remove(this);
                            aiManager.UpdateAIBehaviors();
                        }
                        canAttack = false;
                    }                               // poistaa hahmon listalta t‰m‰n p‰‰st‰ess‰ liian kauas

                    if (character.m_Animator.GetBool("AttackPlaying"))
                    {
                        character.Move(agent.desiredVelocity);
                        target = this.transform.TransformPoint(transform.right * offsetNumber) + target;
                    }               // pys‰ytt‰‰ hahmon hyˆkk‰yksen ajaksi

                    if (agent.remainingDistance >= agent.stoppingDistance)
                    {
                        AgentBehaviorDefiner();
                    }             // pys‰ytt‰‰ hahmon kun on tarpeeksi l‰hell‰ kohdetta ja muuttaa et‰isyytt‰ milloin alkaa seuraamaan uudestaan kohdetta

                    if (agent.remainingDistance <= agent.stoppingDistance && canAttack)
                    {
                        CallAttack(target);
                    } //kutsuu hyˆkk‰yksen kun sen aika on

                    if (distance < 1 && combatIdleState)
                    {
                        character.m_CombatMode = true;
                        character.m_AttackTrigger = true;
                    }                               //kutsuu hyˆkk‰yksen kun pelaaja tulee kiinni ja AI on idle statessa.
                    break;

                case AiBehaviorEnum.Follow:

                    if (target != null)
                    {
                        agent.SetDestination(target);
                        agent.stoppingDistance = InteractionDistance;
                        character.Move(agent.desiredVelocity);
                    }

                    break;
                case AiBehaviorEnum.Avoid:
                    break;
                default:
                    break;
            }

        return distance;

    }

    public void AgentStop()
    {
        agent.isStopped = true;
        character.Move(Vector3.zero);
    }

    /// <summary>
    /// When called, AI will check it's position on AImanager list and adjust it stoppingDistance from player acordingly.
    /// </summary>
    public void AgentBehaviorDefiner()
        {
            isAttackedOnce = false;
            character.m_AttackTrigger = false;
            character.m_CombatMode = false;
            character.Move(agent.desiredVelocity);
            switch (positionInRange)
            {
                case 0:
                    agent.stoppingDistance = stoppingDistances[0];
                    combatIdleState = false;
                    break;
                case 1:
                    agent.stoppingDistance = stoppingDistances[2];
                    combatIdleState = false;
                    break;
                case 2:
                    agent.stoppingDistance = stoppingDistances[2];
                    combatIdleState = false;
                    break;
                default:
                    agent.stoppingDistance = stoppingDistances[4];
                    combatIdleState = true;
                    break;

            }
        }

        /// <summary>
        /// Called when AI is close enogh to player. This method will add player to the AIManagers list and set position in range integer value.
        /// </summary>
        private void DefineAttackerDirAndPlaceOnList()
        {
            if (!aiManager.aiControllerList.Contains(this))
            {
                character.m_AttackCircleDirection = aiManager.circleDirection;
                aiManager.aiControllerList.Add(this);

                for (int i = 0; i < aiManager.aiControllerList.Count; i++)
                {
                    if (aiManager.aiControllerList[i] == this)
                    {
                        positionInRange = aiManager.aiControllerList.IndexOf(this);
                    }
                }

                if (positionInRange <= 2)
                    canAttack = true;
            }
        } //m‰‰ritt‰‰ hyˆkk‰‰kˆ AI:n paikan AI managerin listalla.

        /// <summary>
        /// When called AI faces the set target
        /// </summary>
        void FaceTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }                         // kutsuettaessa hahmo katsoo kohdetta p‰in 

        /// <summary>
        ///  sets combat mode ON and set's circle direction for AI. also switches stopping distance from target acordingly. calls animation depending on what is AI position on a AI manager list
        /// </summary>
        private void CallAttack(Vector3 target)
        {
            character.m_CombatMode = true;
            character.Move(Vector3.zero);
            if (positionInRange == 0)
            {
                agent.stoppingDistance = stoppingDistances[1];
            }

            else
            agent.stoppingDistance = stoppingDistances[3];

            FaceTarget(target);

            if (positionInRange != 0)
            {
                if (!isAttackedOnce)
                {
                    AttackTimer();
                }
            }

            if (positionInRange == 0 && !isAttackedOnce)
            {
                AttackTimer();
            }


        }                   // kutsutaan kun halutaa hyˆkk‰ys animaatio laittaa hahmon kiert‰m‰‰n kohdetta(combatMode) vaihtaa stopping distancet ja kutsuu animaation riippuen kuinka  mones hahmo mik‰ l‰hetyi kohedetta

        /// <summary>
        /// called when we dont want AI to attack and just stay near player when other AI's are attacking
        /// </summary>
        public virtual void CannotAttack(Vector3 target) 
        {
            if (positionInRange >= 3)
            {
                
                agent.stoppingDistance = stoppingDistances[4];
                
                if (agent.remainingDistance <= agent.stoppingDistance && positionInRange >= 3)
                {
                    agent.stoppingDistance = stoppingDistances[5];
                    character.Move(Vector3.zero);
                    FaceTarget(target);
                }
            }

        }       // kutsutaan kun ei haluta hahmon hyˆkk‰‰v‰n, hahmo j‰‰ kauemmas paikallee ja katsoo kohdetta p‰in

        /// <summary>
        /// gives direction for AI to start circle around a target. 
        /// </summary>
        public void SetDirection()
        {
            aiManager.circleDirection = character.m_AttackCircleDirection;
        }                // hakee AiManagerilta suunnan kumpaan p‰in alkaa kiert‰m‰‰n kohdetta(joka toinen kiert‰‰ eri suuntaan)

        /// <summary>
        /// times attacks that AI can first  circle around the player 
        /// </summary>
        private void AttackTimer()
        {
            isAttackedOnce = true;
            if(positionInRange != 0)
            InvokeRepeating("AttackTrigger", 1.5f, Random.Range(1f, 2f));

            if (positionInRange == 0)
                AttackTrigger();
        }

        /// <summary>
        /// triggers animation if AI is not attacking
        /// </summary>
        private void AttackTrigger()
        {
                character.m_AttackTrigger = true;   
        }


    }

public enum AiBehaviorEnum { Attack,Follow, Avoid}