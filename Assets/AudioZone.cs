using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour {

    private int triggerCount = 0;
    void OnTriggerEnter(Collider collider)
    {
        AudioManager manager = collider.gameObject.GetComponent<AudioManager>();
        Debug.Log("hit");
        if (manager != null)
        {
            manager.Play("theme");
        }
    }

        void OnTriggerExit(Collider collider)
        {
            AudioManager manager = collider.gameObject.GetComponent<AudioManager>();

            if (manager != null)
            {
                triggerCount--;
                if (triggerCount <= 0)
                {
                    //audio.Stop();
                }
            }
        }
    

    
}
