using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.QuestSystem;

public class EnemyKillingQuest : MonoBehaviour
{
    /// <summary>
    /// Quest id of this quest.
    /// </summary>
    [SerializeField]
    private string questID;

    /// <summary>
    /// All the character stats to kill before finihing this quest 
    /// </summary>
    [SerializeField]
    CharacterStats[] charactersToKill;

    /// <summary>
    /// Amount of enemies remaining to kill
    /// </summary>
    private int enemiesLeft;

    private void Start()
    {
        foreach (CharacterStats stats in charactersToKill)
        {
            stats.Dying += CharacterKilled;
        }

        enemiesLeft = charactersToKill.Length;
    }

    /// <summary>
    /// Called whenever one of the characters in the array gets killed
    /// </summary>
    private void CharacterKilled ()
    {
        enemiesLeft--;

        if (enemiesLeft <= 0 && questID == QuestManager.S_INSTANCE.CurrentMission.QuestStringID) //No enemies left
        {
            QuestManager.S_INSTANCE.CurrentMission.EndMission();
        }
    }
}
