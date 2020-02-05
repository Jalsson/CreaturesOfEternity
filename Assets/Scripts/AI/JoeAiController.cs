using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.DialogueSystem;

public class JoeAiController : AiController {

    public DialogueContainer NextDialog
    {
        get
        {
            return nextDialog;   
        }
        set
        {
            if (value != null)
            {
                InputManager.S_INSTANCE.FrameUpdate += StartDialog;
                nextDialog = value;
            }
            else
                nextDialog = value;
        }
    }

    [SerializeField]
    private float DefaultInteractionDistance = 2f;

    private bool canAttack = true;

    [SerializeField]
    private DialogueContainer nextDialog;

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// follows the player's location when called
    /// </summary>
    public void FollowPlayer()
    {
        movementController.MoveToTarget(PlayerManager.S_INSTANCE.player.gameObject.transform.position, AiBehaviorEnum.Follow, DefaultInteractionDistance);   
    }

    /// <summary>
    /// when This character is close enogh it will start dialog which player can read.
    /// </summary>
    public void StartDialog()
    {
        if (movementController.agent.remainingDistance != 0 && movementController.agent.remainingDistance < DefaultInteractionDistance)
        {
            nextDialog.StartDialogue();
            InputManager.S_INSTANCE.FrameUpdate -= StartDialog;
        }
    }

    /// <summary>
    /// attacks enemyes when canAttack boolean is true
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (canAttack)
        {
            if (other.tag == "Enemy")
            {
                InputManager.S_INSTANCE.FrameUpdate -= FollowPlayer;
                movementController.MoveToTarget(other.gameObject.transform.position, AiBehaviorEnum.Attack, 0);
            }
        }
       
    }

}
