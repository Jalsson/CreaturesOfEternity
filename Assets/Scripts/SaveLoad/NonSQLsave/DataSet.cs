using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;



//CODE NOT IN USE
public class DataSet : MonoBehaviour {
    PlayerController player;
    StaminaController stamina;
    
    
    void Start()
    {
       player = PlayerManager.S_INSTANCE.player.GetComponent<PlayerController>();
       stamina = PlayerManager.S_INSTANCE.player.GetComponent<StaminaController>();
        
    }
    
    
    public void Save()
    {
        SaveLoad.SavePlayer(player);
        
    }

    public void Load()
    {
        SaveLoad.LoadPlayer();
        Data data = SaveLoad.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        Debug.Log(position.x);
        Debug.Log(position.y);
        Debug.Log(position.z);

        float stam;

       // stam = data.stam[0];
        //Debug.Log("current stamina is = " + stam);

        

        //rb.transform.position = new Vector3(position.x, position.y, position.z);
    }
}
