using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Quest/Side quest")]
public class SideMission : ScriptableObject
{
    public int Reward { get { return reward; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public BaseSideMissionCondition MissionCondition { get { return missionCondition; } }

    [SerializeField]
    private int reward = 100;
    private int currentMissionState = 0;

    [SerializeField]
    private int targetNumber;

    [SerializeField]
    new private string name = "default mission name";

    [SerializeField]
    private string description = "default mission description";

    [SerializeField]
    private BaseSideMissionCondition missionCondition;

    public void advanceMission()
    {
        if (currentMissionState <= targetNumber)
        {
            currentMissionState++;
        }
        else
            EndMission();
    }

    public void EndMission()
    {
        Debug.Log("mission completed");
    }

}
