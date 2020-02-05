using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    private void Start()
    {
        Vector3 playerPos = transform.position + new Vector3(0,0.5f,0);
        Vector3 playerDirection = transform.right;
        Quaternion playerRotation = transform.rotation;
        float spawnDistance = 3f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        PlayerManager.S_INSTANCE.player.transform.position = spawnPos;
    }
}
