using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissionState : MonoBehaviour {


    [SerializeField]
    private string stateDescription;

    private StoryMission Mission;

    /// <summary>
    /// First thing that is called when new mission state is started
    /// </summary>
    public virtual void StateTrigger(StoryMission currentMission)
    {
        Mission = currentMission;
        UiMessage();
        Debug.Log("Start missions coroutine");
    }

    /// <summary>
    /// Show message to UI about the state task and how to complete it.
    /// </summary>
    public virtual void UiMessage()
    {
        Debug.Log(stateDescription);
    }

    public virtual void StateEnding()
    {
        Mission.NextState();
    }
}
