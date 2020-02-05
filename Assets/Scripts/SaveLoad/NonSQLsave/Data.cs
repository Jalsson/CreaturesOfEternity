using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data {

    // Use this for initialization
    
    public float[] position;
    public Data (PlayerController player)
    {
        position = new float[3];
        Debug.Log(player.transform.position.x);
        Debug.Log(player.transform.position.y);
        Debug.Log(player.transform.position.z);

        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

    
}
