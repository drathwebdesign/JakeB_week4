using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject[] mapPrefabs; // Array of all possible maps, Keep Boss as last
    public Transform landingPoint; // Reference to the landing point for Boss
    private Vector3 spawnPosition = new Vector3(65, 0, 0);

    private int mapSpawnCount = 0; //track how many maps have been spawned
    private bool specialMapSpawned = false; // Flag to check if the special map has been spawned
    private bool isPaused = false;

    void Start() {
        InvokeRepeating("SpawnMaps", 3, 20); // Starts the spawning process
    }

    void Update() {
    }

    void SpawnMaps() {
        if (isPaused) return; // Do not spawn anything if paused

        mapSpawnCount++;

        if (mapSpawnCount >= 6 && !specialMapSpawned) {
            // Spawn the special map (DragonAI) after at least a certain number of random maps
            GameObject specialMapInstance = Instantiate(mapPrefabs[mapPrefabs.Length - 1], spawnPosition, Quaternion.identity);
            specialMapSpawned = true;

            // Find the DragonEntry script in the special map
            DragonEntry dragonEntry = specialMapInstance.GetComponentInChildren<DragonEntry>();

            if (dragonEntry != null) {
                // Assign the landing point to the DragonEntry script
                dragonEntry.SetLandingPoint(landingPoint);
            }

            PauseSpawning(); // Pause further spawning until the dragon is defeated
        } else {
            // Randomly spawn one of the regular maps
            Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length - 1)], spawnPosition, Quaternion.identity);
        }
    }

    public void PauseSpawning() {
        isPaused = true;
    }

    public void ResumeSpawning() {
        isPaused = false;
    }
}