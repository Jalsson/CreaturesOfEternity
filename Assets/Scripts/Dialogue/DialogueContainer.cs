//Original file created by Julius Liias 14.11.2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.UISystem;

namespace Venus.DialogueSystem
{
    /// <summary>
    /// Contains and triggers the dialogue inside it.
    /// </summary>
    public class DialogueContainer : MonoBehaviour
    {
        /// <summary>
        /// Name of this entity in the dialogue. Private set.
        /// </summary>
        public string DialogueName
        {
            get
            {
                return dialogueName;
            }
            private set
            {
                name = value;
            }
        }

        /// <summary>
        /// Name of this entity in the dialogue.
        /// </summary>
        [SerializeField]
        private string dialogueName;
        [Space]
        /// <summary>
        /// All the dialogues played by this container.
        /// </summary>
        [SerializeField]
        [Tooltip("All the dialogue played by this container.")]
        private Dialogue[] dialogues;
        /// <summary>
        /// ID of the current dialogue being shown from this.
        /// </summary>
        private int currentDialogueIndex;

        /// <summary>
        /// Sets the dialogue array to the given array.
        /// </summary>
        /// <param name="dialogues"></param>
        public void SetDialogues (Dialogue[] dialogues)
        {
            this.dialogues = dialogues;
        }

        /// <summary>
        /// Sets the dialogue name to the given string.
        /// </summary>
        /// <param name="dialogueName">New name of the dialogue character.</param>
        public void SetDialogueName(string dialogueName)
        {
            this.dialogueName = dialogueName;
        }

        /// <summary>
        /// Starts the dialogue of this
        /// </summary>
        public void StartDialogue ()
        {
            if (!UIManager.S_INSTANCE.IsInDialogue)
            {
                currentDialogueIndex = 0;

                UIManager.S_INSTANCE.StartDialogue(this);
            }
        }

        /// <summary>
        /// Plays the dialogue with the index of the current dialogue index.
        /// </summary>
        /// <returns>Returns false if there is no more dialogue to show.</returns>
        public Dialogue GetNextDialogue ()
        {
            if (dialogues.Length > currentDialogueIndex)
            {
                Dialogue dialogueToReturn = dialogues[currentDialogueIndex];

                currentDialogueIndex++;

                return dialogueToReturn;
            }

            return null;
        }

        /// <summary>
        /// Called by the UIManager when the dialogue ends. Can be used for quests when inheriting.
        /// </summary>
        public virtual void DialogueEnd()
        {
            //Nothing. Used in inheritance for quests
        }
    }
}