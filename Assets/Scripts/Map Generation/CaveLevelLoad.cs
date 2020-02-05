using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveLevelLoad : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.S_INSTANCE.SavePlayer(); //Save

            StartCoroutine(JumpToHub()); //Enter after 1.5 seconds
        }
    }

    private IEnumerator JumpToHub()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("prebakedCave");
    }
}
