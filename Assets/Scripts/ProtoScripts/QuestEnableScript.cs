using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.QuestSystem;

public class QuestEnableScript : MonoBehaviour
{
    /// <summary>
    /// Quest ID to wait for before triggering the game objects.
    /// </summary>
    [SerializeField] [Tooltip("Quest ID to wait for before triggering the game objects.")]
    private string questStringID;

    [Space]

    /// <summary>
    /// Game objects that will get their active state set to the given ones when the quest with the same ID gets triggered.
    /// </summary>
    [SerializeField] [Tooltip("Game objects that will get their active state set to the given ones when the quest with the same ID gets triggered.")]
    private ActivitySetGameObject[] gameObjects;

    private void Start()
    {
        QuestManager.S_INSTANCE.QuestChanged += CheckForEnable;
    }

    /// <summary>
    /// Subscribed to the QuestChanged delegate in the quest manager. Triggers the objects if the new quest has the same quest ID.
    /// </summary>
    /// <param name="questID">ID of the quest to check.</param>
    private void CheckForEnable (string questID)
    {
        if (questID == questStringID)
        {
            TriggerObjects();
        }
    }

    /// <summary>
    /// Calls the trigger method of all the game objects in the array.
    /// </summary>
    private void TriggerObjects ()
    {
        if (gameObjects != null && gameObjects.Length > 0) //Make sure the array is not null or empty
        {
            foreach (ActivitySetGameObject actSetObj in gameObjects)
            {
                if (actSetObj != null)
                {
                    actSetObj.Trigger(); //Trigger
                }
            }
        }
    }
}

/// <summary>
/// Class holding a reference to a gameobject and a state that it gets set to when the Trigger function gets called.
/// </summary>
[System.Serializable]
public class ActivitySetGameObject
{
    /// <summary>
    /// Game object to be set to the given state.
    /// </summary>
    [SerializeField]
    private GameObject gameObject;

    /// <summary>
    /// State to set the game object to when this class gets triggered.
    /// </summary>
    [SerializeField]
    private bool stateToSetTo;

    /// <summary>
    /// Sets the activity state of the game object stored in this class to the state indicated.
    /// </summary>
    public void Trigger ()
    {
        gameObject.SetActive(stateToSetTo);
    }
}
