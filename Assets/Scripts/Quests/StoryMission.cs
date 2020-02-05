using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.QuestSystem;

[CreateAssetMenu(fileName = "New Item", menuName = "Quest/Story quest")]
public class StoryMission : ScriptableObject
{
    /// <summary>
    /// Reward after completing the Quest.
    /// </summary>
    public int Reward
    {
        get
        {
            return reward;
        }
    }

    /// <summary>
    /// The string ID of this quest.
    /// </summary>
    public string QuestStringID
    {
        get
        {
            return questStringID;
        }
    }

    /// <summary>
    /// The name of this quest.
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
    }

    /// <summary>
    /// The description of this quest.
    /// </summary>
    public string Description
    {
        get
        {
            return description;
        }
    }

    /// <summary>
    /// Mission states.
    /// </summary>
    public BaseMissionState[] MissionStates
    {
        get
        {
            return missionStates;
        }
    }

    /// <summary>
    /// Reward after completing the Quest.
    /// </summary>
    [SerializeField]
    private int reward = 100;

    /// <summary>
    /// Current mission state.
    /// </summary>
    private int currentMissionState = 0;

    [Space]

    /// <summary>
    /// ID that will be used to identify this quest.
    /// </summary>
    [SerializeField] [Tooltip("ID that will be used to identify this quest.")]
    private string questStringID;

    [Space]

    /// <summary>
    /// The name of the mission.
    /// </summary>
    [SerializeField] [TextArea(1, 3)] [Tooltip("The name of this quest.")]
    new private string name = "default mission name";

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] [TextArea(3, 8)] [Tooltip("The description of this quest.")]
    private string description = "default mission description";

    [Space]

    [SerializeField] [Tooltip("Next quest after finishing this.")]
    private StoryMission nextStoryQuest;

    [Space]

    /// <summary>
    /// These are all the states in mission which player will have to go through in order to complete the mission
    /// </summary>
    [SerializeField] [Tooltip("These are all the states in mission which player will have to go through in order to complete the mission")]
    private BaseMissionState[] missionStates;


    public void NextState()
    {
        Debug.Log("Next mission State");
        if (missionStates[currentMissionState] != null)
        {
            if (currentMissionState >= missionStates.Length)
            {
                EndMission();
            }
            else
            {
                currentMissionState++;
                missionStates[currentMissionState].StateTrigger(this);
            }
        }
    }

    /// <summary>
    /// Ends the current quest, rewards the player and starts the next quest.
    /// </summary>
    public void EndMission()
    {
        Debug.Log("Mission Completed!. You got " + reward + " money for reward");

        if (PlayerManager.S_INSTANCE.player != null) //Give money
        {
            PlayerManager.S_INSTANCE.player.GetComponent<EntityInventory>().ReceiveMoney(reward);
        }

        if (nextStoryQuest != null)
        {
            QuestManager.S_INSTANCE.StartStoryMission(nextStoryQuest);
        }
    }
}
