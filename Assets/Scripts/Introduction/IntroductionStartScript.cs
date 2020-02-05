using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionStartScript : QuestDialogueCharacter
{
    protected override void Start()
    {
        base.Start();

        StartDialogue(); //Start dialogue as soon as loaded in
    }
}
