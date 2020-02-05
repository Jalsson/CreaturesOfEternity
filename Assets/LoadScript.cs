using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.S_INSTANCE.LoadPlayer();
    }
}
