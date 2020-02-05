using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


    GameObject enemyProp;

	// Use this for initialization
	void Start () {

        enemyProp = Resources.Load<GameObject>("Props/Enemys/RaiderEnemy");

        MapGeneration mapGeneration = GameObject.Find("MapGenerator").GetComponent<MapGeneration>();
        if (mapGeneration != null)
        {
            Vector3 playerPos = transform.position + new Vector3(0, 0.1f, 0);
            Vector3 playerDirection = transform.right;

            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                float spawnDistance = Random.Range(1, 3);

                Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

                Instantiate(enemyProp, spawnPos, transform.rotation);

            }

        }
        
    }
	
}
