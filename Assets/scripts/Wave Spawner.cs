using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    // This creates a clean dropdown menu in the Inspector
    [System.Serializable]
    public class Wave
    {
        public string waveName = "Wave 1";
        public GameObject enemyPrefab; // Which enemy to spawn?
        public int count = 2;          // How many enemies in this wave?
        public float spawnRate = 1f;   // Time between each enemy spawn (seconds)
    }

    [Header("Wave Settings")]
    public Wave[] waves;                // List of all your waves
    public float timeBetweenWaves = 5f; // Rest time after a wave finishes

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;     // Where enemies appear (drag empty objects here)

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Error: No Spawn Points referenced!");
        }
        else
        {
            StartCoroutine(StartWaveLogic());
        }
    }

    IEnumerator StartWaveLogic()
    {
        // Loop through every wave you created in the Inspector
        for (int i = 0; i < waves.Length; i++)
        {
            Wave currentWave = waves[i];
            Debug.Log("Starting " + currentWave.waveName);

            // Spawn the enemies for THIS wave
            for (int j = 0; j < currentWave.count; j++)
            {
                SpawnEnemy(currentWave.enemyPrefab);

                // Wait before spawning the next enemy in this wave
                yield return new WaitForSeconds(currentWave.spawnRate);
            }

            // Wave finished. Wait before starting the next wave.
            if (i < waves.Length - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        // Pick a random spawn point from your list
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[randomIndex];

        Instantiate(enemy, sp.position, Quaternion.identity);
    }
}