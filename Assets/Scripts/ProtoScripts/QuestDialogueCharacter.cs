using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Delegates;
using Venus.DialogueSystem;
using Venus.QuestSystem;

public class QuestDialogueCharacter : DialogueCharacter, IQuestObject
{
    [Space(30)]

    /// <summary>
    /// Dialogue ran when the player is not in the mission needed yet.
    /// </summary>
    [SerializeField] [Tooltip("Dialogue ran when the player is not in the mission needed yet.")]
    protected Dialogue[] dialogueWhenNotInMission;

    /// <summary>
    /// The ID of the quest where this character is needed.
    /// </summary>
    [SerializeField] [Tooltip("The ID of the quest where this character is needed.")]
    private string questStringID;

    /// <summary>
    /// Delegate called when this quest is done.
    /// </summary>
    private VoidDelegate questDoneDelegate;

    /// <summary>
    /// Container used for a second set of dialogue when the current quest doesn't match up with the quest id of this.
    /// </summary>
    private DialogueContainer noQuestContainer;

    protected virtual void Start()
    {
        //Set dialogue outside quest
        noQuestContainer = gameObject.AddComponent<DialogueContainer>();

        noQuestContainer.SetDialogues(dialogueWhenNotInMission);
        noQuestContainer.SetDialogueName(DialogueName);
    }

    /// <summary>
    /// Returns the quest done delegate.
    /// </summary>
    /// <returns>questDone delegate</returns>
    public VoidDelegate GetQuestDoneDelegate()
    {
        return questDoneDelegate;
    }

    /// <summary>
    /// Returns the quest string id of this.
    /// </summary>
    /// <returns>Quest ID string of this.</returns>
    public string GetQuestStringID()
    {
        return questStringID;
    }

    /// <summary>
    /// Interacts with this interactable, triggering either the quest dialogue or normal dialogue.
    /// </summary>
    public override void Interact()
    {
        if (QuestManager.S_INSTANCE.CurrentMission.QuestStringID == questStringID) //Only interact with the quest lines when the quest os active.
        {
            base.Interact();
        }
        else
        {
            noQuestContainer.StartDialogue();
        }
    }

    /// <summary>
    /// Called when the quest is done. In this case when the player is done talking to this NPC.
    /// </summary>
    public void QuestDone()
    {
        QuestManager.S_INSTANCE.CurrentMission.EndMission();
    }

    /// <summary>
    /// Marks the quest as done.
    /// </summary>
    public override void DialogueEnd()
    {
        base.DialogueEnd(); //In case something is added there in the future xd

        QuestDone();
    }
}
