using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPosition = new Vector3(25, 0, 0);

    void Start()
    {
        InvokeRepeating("SpawnObstacles", 3, 3);
    }


    void Update()
    {
        
    }

    void SpawnObstacles() {
        Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnPosition, Quaternion.identity);
    }
}
