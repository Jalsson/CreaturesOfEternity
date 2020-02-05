//Original file created by Julius Liias 14.11.2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Venus.Utilities;
using Venus.DialogueSystem;
using System;
using TMPro;
using Venus.Delegates;
using Venus.QuestSystem;

namespace Venus.UISystem
{
    /// <summary>
    /// A snigleton class taking care of all the UI element updates on the player's screen.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        public bool IsInDialogue
        {
            get
            {
                if (currentDialogueContainer == null)
                {
                    return false;
                }

                return true;
            }
        }

        #region propertys
        /// <summary>
        /// Main canvas containing all the player's UI elements.
        /// </summary>
        Canvas mainCanvas;

        /// <summary>
        /// Parent of all the trading UI items
        /// </summary>
        private RectTransform tradingPanel;

        //Dialogue box
        /// <summary>
        /// RectTransform of the dialogue box UI element.
        /// </summary>
        private RectTransform dialogueBox;
        /// <summary>
        /// ReactTransform of the key popup showing interactable things
        /// </summary>
        private RectTransform keyPopup;
        /// <summary>
        /// Image component for showing the dialogue's image inside the dialogue box.
        /// </summary>
        private Image dialogueBoxImage;
        /// <summary>
        /// Text field to display the name of the entity that's speaking.
        /// </summary>
        private TextMeshProUGUI dialogueBoxNameField;
        /// <summary>
        /// Text component inside the dialogue UI element that will show the text of the current dialogue.
        /// </summary>
        private TextMeshProUGUI dialogueBoxText;

        /// <summary>
        /// Dialogue container that is showing dialogue at the moment. Null if no dialogue.
        /// </summary>
        private DialogueContainer currentDialogueContainer;

        [SerializeField]
        private TextMeshProUGUI questNameText;
        [SerializeField]
        private TextMeshProUGUI questDescriptionText;

        private bool currentDialogueOnPlayer;

        private bool referencesSet = false;
        #endregion

        protected override void  Awake()
        {
            base.Awake();

            tradingPanel = transform.Find("TradingPanel").GetComponent<RectTransform>();

            dialogueBox = transform.Find("DialogueBox").GetComponent<RectTransform>();
            keyPopup = transform.Find("KeyPopup").GetComponent<RectTransform>();

            dialogueBoxText = dialogueBox.Find("TextField").GetComponent<TextMeshProUGUI>();
            dialogueBoxImage = dialogueBox.Find("InfoPanel").Find("Image").GetComponent<Image>();
            dialogueBoxNameField = dialogueBox.Find("InfoPanel").Find("NameField").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            InputManager.S_INSTANCE.UpdateUI += OpenUI;

            if (QuestManager.S_INSTANCE != null)
            {
                QuestManager.S_INSTANCE.QuestChanged += UpdateQuestInfo;
                UpdateQuestInfo(QuestManager.S_INSTANCE.CurrentMission.QuestStringID);
            }
        }

        /// <summary>
        /// Updates the UI with given UI state
        /// </summary>
        /// <param name="uiState"></param>
        public void OpenUI(UiStateEnum uiState)
        {
            switch (uiState)
            {
                case UiStateEnum.GamePlay:
                    GetComponent<InventoryUI>().InventoryState(false);
                    GetComponent<InventoryUI>().StorageState(false);
                    break;
                case UiStateEnum.Inventory:
                    GetComponent<InventoryUI>().InventoryState(true);
                    if (GetComponent<InventoryUI>().currentStorageRef != null)
                    {
                        GetComponent<InventoryUI>().StorageState(true);
                    }
                    else
                        GetComponent<InventoryUI>().StorageState(false);
                    break;
                case UiStateEnum.Trading:
                    break;
                case UiStateEnum.Dialog:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the position of the interaction key popup to the given position on screen.
        /// </summary>
        /// <param name="positionInWorld"></param>
        public void UpdateKeyPopupPosition(Vector3 positionInWorld)
        {
            keyPopup.position = Camera.main.WorldToScreenPoint(positionInWorld);
        }

        /// <summary>
        /// Sets the key popup objective's active state to the given state.
        /// </summary>
        /// <param name="state">Is set active.</param>
        public void SetKeyPopupActive (bool state)
        {
            keyPopup.gameObject.SetActive(state);
        }

        /// <summary>
        /// Starts the dialogue of the given dialogue container
        /// </summary>
        /// <param name="dialogueContainer">The container giving the dialogue.</param>
        public void StartDialogue (DialogueContainer dialogueContainer)
        {
            //REMOVE THIS LINE AFTER TESTING AND USE SOMETHING BETTER
            SetKeyPopupActive(false);

            if (!IsInDialogue)
            {
                InputManager.S_INSTANCE.Interact += ShowNextDialogue;
                InputManager.S_INSTANCE.FrameUpdate += SetDialogueBoxPoisitionToCurrentDialogue;
                InputManager.S_INSTANCE.CanInteract = false;

                //Set current dialogue container
                currentDialogueContainer = dialogueContainer;

                //Show next dialogue
                ShowNextDialogue();
            }
        }

        /// <summary>
        /// Loads the next dialogue from the current dialogue container.
        /// </summary>
        private void ShowNextDialogue ()
        {
            if (currentDialogueContainer == null)
            {
                return;
            }

            Dialogue dialogue = currentDialogueContainer.GetNextDialogue();

            if (dialogue == null) //Dialogue ended
            {
                EndDialogue();

                return;
            }

            if (dialogue.PlayerDialogue) //Focus dialogue box on player
            {
                currentDialogueOnPlayer = true;

                dialogueBoxNameField.text = "You";

                //Find player's transform
                if (PlayerManager.S_INSTANCE != null && PlayerManager.S_INSTANCE.player != null)
                {
                    SetDialogueBoxPoisitionTo(PlayerManager.S_INSTANCE.player.transform);
                }
                else
                {
                    SetDialogueBoxPoisitionTo(GameObject.Find("Player").transform);
                }
            }
            else //Focus dialogue box on dialogue container
            {
                currentDialogueOnPlayer = false;

                dialogueBoxNameField.text = currentDialogueContainer.DialogueName;

                SetDialogueBoxPoisitionTo(currentDialogueContainer.transform);
            }

            //Update the image
            if (dialogue.Image != null)
            {
                //Set image active and update it's image
                dialogueBoxImage.gameObject.SetActive(true);
                dialogueBoxImage.sprite = dialogue.Image;
            }
            else
            {
                //Set image inactive
                dialogueBoxImage.gameObject.SetActive(false);
            }

            //Update text
            dialogueBoxText.text = dialogue.Text;

            //Enable gameobject
            dialogueBox.gameObject.SetActive(true);
        }

        /// <summary>
        /// Ends the dialogue
        /// </summary>
        public void EndDialogue ()
        {
            InputManager.S_INSTANCE.Interact -= ShowNextDialogue;
            InputManager.S_INSTANCE.FrameUpdate -= SetDialogueBoxPoisitionToCurrentDialogue;
            InputManager.S_INSTANCE.CanInteract = true;

            //REMOVE THIS LINE AFTER TESTING AND USE SOMETHING BETTER
            SetKeyPopupActive(true);

            //Call dialogue end
            currentDialogueContainer.DialogueEnd();

            //Dereference dialogue contianer
            currentDialogueContainer = null;

            //Disable gameobject
            dialogueBox.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the position of the dialogue box's origin on top of the given world space transform.
        /// </summary>
        /// <param name="worldSpaceTransform">Transform or the "speaker" that will have the dialogue on top of it.</param>
        private void SetDialogueBoxPoisitionTo (Transform worldSpaceTransform)
        {
            dialogueBox.position = Camera.main.WorldToScreenPoint(worldSpaceTransform.position);
        }

        /// <summary>
        /// Sets the position of the dialogue box's origin on top of the current dialogue container.
        /// </summary>
        private void SetDialogueBoxPoisitionToCurrentDialogue()
        {
            if (currentDialogueContainer != null)
            {
                if (currentDialogueOnPlayer)
                {
                    SetDialogueBoxPoisitionTo(PlayerManager.S_INSTANCE.player.transform);
                }
                else
                {
                    SetDialogueBoxPoisitionTo(currentDialogueContainer.transform);
                }
            }
        }

        private void UpdateQuestInfo (string questStringID)
        {
            foreach (StoryMission loadedMission in QuestManager.S_INSTANCE.StoryMissions) //Loop through
            {
                if (questStringID == loadedMission.QuestStringID) //Check if matches
                {
                    //Change text
                    questNameText.text = loadedMission.Name;
                    questDescriptionText.text = loadedMission.Description;
                }
            }
        }
    }
}