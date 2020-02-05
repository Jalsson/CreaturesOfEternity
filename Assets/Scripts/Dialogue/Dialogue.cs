//Original file created by Julius Liias 14.11.2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Venus.DialogueSystem
{
    /// <summary>
    /// Dialogue struct containing all the data of a single dialogue.
    /// </summary>
    [System.Serializable]
    public class Dialogue
    {
        /// <summary>
        /// The text of this dialogue. Private set.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            private set
            {
                text = value;
            }
        }

        /// <summary>
        /// The image shown during this dialogue. Private set.
        /// </summary>
        public Sprite Image
        {
            get
            {
                return image;
            }
            private set
            {
                image = value;
            }
        }

        /// <summary>
        /// Indicates if this dialogue is from the player, therefore the box will be located on top of him in the scene. Private Set.
        /// </summary>
        public bool PlayerDialogue
        {
            get
            {
                return playerDialog;
            }
            private set
            {
                playerDialog = value;
            }
        }

        /// <summary>
        /// The text of this dialogue.
        /// </summary>
        [SerializeField]
        [TextArea(3,10)]
        [Tooltip("The text of this dialogue.")]
        private string text;

        /// <summary>
        /// The image shown during this dialogue.
        /// </summary>
        [SerializeField]
        [Tooltip("The image shown during this dialogue.")]
        private Sprite image;

        /// <summary>
        /// Indicates if this dialogue is from the player, therefore the box will be located on top of him in the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("Indicates if this dialogue is from the player, therefore the box will be located on top of him in the scene.")]
        private bool playerDialog;
    }
}