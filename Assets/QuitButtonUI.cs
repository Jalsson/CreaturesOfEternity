using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonUI : MonoBehaviour {



    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(QuitTheGame);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
