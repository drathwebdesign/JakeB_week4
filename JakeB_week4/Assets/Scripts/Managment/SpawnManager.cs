using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject[] mapPrefabs; // Array of all possible maps, Keep Boss as last
    private Vector3 spawnPosition = new Vector3(65, 0, 0);

    private int mapSpawnCount = 0; //track how many maps have been spawned
    private bool specialMapSpawned = false; // Flag to check if the special map has been spawned

    void Start() {
        InvokeRepeating("SpawnObstacles", 3, 20); // Starts the spawning process
    }

    void Update() {
        // Additional logic can be placed here if needed
    }

    void SpawnObstacles() {
        GameObject obstacleToSpawn;

        if (mapSpawnCount >= 10 && !specialMapSpawned) {
            // Spawn the special map after 10 random spawns
            obstacleToSpawn = mapPrefabs[mapPrefabs.Length - 1]; // Keep Boss as last in array
            specialMapSpawned = true;
        } else {
            // Randomly spawn from the first 10 maps
            obstacleToSpawn = mapPrefabs[Random.Range(0, mapPrefabs.Length - 1)];
        }

        Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity);
        mapSpawnCount++;
    }
}
