using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponentInParent<MapGeneration>().CurrentLevel = 2;
    }
}
