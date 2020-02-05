using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonUI : MonoBehaviour {

    // Use this for initialization
    Button saveButton;
	void Start () {
        saveButton = GetComponent<Button>();
        saveButton.onClick.AddListener(SaveAndExit);
	}
	
    public void SaveAndExit()
    {
        FindObjectOfType<AudioManager>().Play("menu");
        PlayerManager.S_INSTANCE.SavePlayer();
        Application.Quit();
    }
}
