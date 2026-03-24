using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnPointData
{
    public Transform point;
    public int maxLizards = 2; 
    [HideInInspector] public int currentCount = 0; 
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject lizardPrefab;
    public List<SpawnPointData> spawnPointList; 
    public int totalLizardsToSpawn = 10;

    void Start()
    {
        for (int i = 0; i < totalLizardsToSpawn; i++)
        {
            // Pass 'i' into the function so it knows which number it is
            SpawnLizard(i); 
        }
    }

    void SpawnLizard(int lizardNumber)
    {
        List<SpawnPointData> availablePoints = new List<SpawnPointData>();

        foreach (SpawnPointData sp in spawnPointList)
        {
            if (sp.currentCount < sp.maxLizards)
            {
                availablePoints.Add(sp);
            }
        }

        if (availablePoints.Count == 0)
        {
            // Now we use 'lizardNumber' instead of 'i'
            Debug.LogWarning("All spawn points are full! Could not spawn lizard #" + (lizardNumber + 1));
            return;
        }

        int randomIndex = Random.Range(0, availablePoints.Count);
        SpawnPointData chosenPoint = availablePoints[randomIndex];

        Instantiate(lizardPrefab, chosenPoint.point.position, chosenPoint.point.rotation);
        chosenPoint.currentCount++;
        
        Debug.Log($"Spawned lizard at {chosenPoint.point.name}. Count: {chosenPoint.currentCount}/{chosenPoint.maxLizards}");
    }
}