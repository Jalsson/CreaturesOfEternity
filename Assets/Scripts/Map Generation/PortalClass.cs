using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalClass : MonoBehaviour {

    MapGeneration mapGeneration;

    private void Start()
    {
        mapGeneration = GameObject.Find("MapGenerator").GetComponent<MapGeneration>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartNextLevel();
        }
    }

    public void StartNextLevel()
    {
        mapGeneration.CurrentLevel++;
    }
}
