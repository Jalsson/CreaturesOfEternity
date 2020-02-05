using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.DialogueSystem;
using Venus.UISystem;

public class DialogueTestScript : MonoBehaviour
{
    public void Start()
    {
        UIManager.S_INSTANCE.SetKeyPopupActive(true);
    }

    private void Update ()
    {
        UIManager.S_INSTANCE.UpdateKeyPopupPosition(transform.position);

        if (Input.GetKeyDown(KeyCode.E) && !UIManager.S_INSTANCE.IsInDialogue)
        {
            GetComponent<DialogueContainer>().StartDialogue();
        }
    }
}
