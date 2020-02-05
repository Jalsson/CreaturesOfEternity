using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadButtonUI : MonoBehaviour {

    Button loadButton;
    void Start()
    {
        loadButton = GetComponent<Button>();
        loadButton.onClick.AddListener(LoadPlayer);
    }

    public void LoadPlayer()
    {
        FindObjectOfType<AudioManager>().Play("menu");
        PlayerManager.S_INSTANCE.LoadPlayer();
        Application.Quit();
    }
}
