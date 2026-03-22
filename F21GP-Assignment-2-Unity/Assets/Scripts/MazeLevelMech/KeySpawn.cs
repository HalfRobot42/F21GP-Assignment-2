using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn : MonoBehaviour
{
    public GameObject keyPrefab;
    public GameObject skullPrefab;
    private int spawnPointIndex;

    private GameObject[] keySpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        keySpawnPoints = GameObject.FindGameObjectsWithTag("KeySpawnPosition");
        spawnPointIndex = Random.Range(0, keySpawnPoints.Length);
        // spawnPointIndex = 0;
        for (int i = 0; i < keySpawnPoints.Length; i++)
        {
            if (i != spawnPointIndex)
            {
                Instantiate(skullPrefab, keySpawnPoints[i].transform.position, Quaternion.identity);
            }
        }
        Instantiate(keyPrefab,keySpawnPoints[spawnPointIndex].transform.position,Quaternion.identity);
    }

    
}
