using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.DialogueSystem;
using Venus.Interaction;
using Venus.UISystem;

public class DialogueCharacter : DialogueContainer, IInteractable
{
    public virtual Transform GetTransform()
    {
        return transform;
    }

    public virtual void Interact ()
    {
        StartDialogue();
    }
}