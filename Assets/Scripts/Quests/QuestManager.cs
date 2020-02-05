using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;
using Venus.DialogueSystem;
using Venus.Delegates;

namespace Venus.QuestSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        /// <summary>
        /// All the loaded missions found from resources. Get only.
        /// </summary>
        public StoryMission[] StoryMissions
        {
            get
            {
                return storyMissions;
            }
        }

        /// <summary>
        /// Current active mission.
        /// </summary>
        public StoryMission CurrentMission
        {
            get
            {
                return currentMission;
            }
        }

        /// <summary>
        /// Gets called whenever the quest changes.
        /// </summary>
        public QuestChangeDelegate QuestChanged
        {
            get
            {
                return questChanged;
            }
            set
            {
                questChanged = value;
            }
        }

        /// <summary>
        /// All the loaded missions found from resources. Get only.
        /// </summary>
        private StoryMission[] storyMissions;

        /// <summary>
        /// Current active mission.
        /// </summary>
        [SerializeField]
        private StoryMission currentMission;

        private QuestChangeDelegate questChanged;

        protected override void Awake()
        {
            base.Awake();

            //Load missions
            storyMissions = Resources.LoadAll<StoryMission>("Missions");
        }

        /// <summary>
        /// Searches for a mission with the corresponding quest string ID and initiates it.
        /// </summary>
        /// <param name="stringID">String ID for the quest mission that will be loaded and initiated.</param>
        public void LoadMissionByStringID (string stringID)
        {
            foreach (StoryMission loadedMission in storyMissions) //Loop through
            {
                if (stringID == loadedMission.QuestStringID) //Check if matches
                {
                    StartStoryMission(loadedMission);
                    return;
                }
            }

            Debug.Log("Did not find a quest to load with the given string ID!");
        }

        /// <summary>
        /// Starts the given mission by assigning it to the current story mission.
        /// </summary>
        /// <param name="missionToStart">Mission to initiate.</param>
        public void StartStoryMission (StoryMission missionToStart)
        {
            currentMission = missionToStart;

            if (questChanged != null) //Call delegate
            {
                questChanged(currentMission.QuestStringID);
            }
        }

        public void EnemyKilled (CharacterStats character)
        {

        }

        public void CharacterSpokenTo (DialogueContainer dialogueContainer)
        {

        }

        public void ItemPickedUp (Item item)
        {

        }
    }
}