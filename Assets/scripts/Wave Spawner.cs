using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Needed for Lists
using TMPro; // Needed for the Text

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName = "Wave 1";
        public GameObject enemyPrefab;
        public int count = 2;
        public float spawnRate = 1f;
    }

    [Header("Wave Settings")]
    public Wave[] waves;
    public float timeBetweenWaves = 5f;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    [Header("Battle End Settings")]
    public GameObject invisibleWall;  // Drag your Invisible Cube here
    public TextMeshProUGUI winText;   // Drag your "Proceed" text here

    // Internal tracking
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool finishedSpawningAllWaves = false;

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

    void Update()
    {
        // 1. Clean the list (Remove dead enemies automatically)
        activeEnemies.RemoveAll(enemy => enemy == null);

        // 2. Only check for the win condition if we are TOTALLY done spawning
        if (finishedSpawningAllWaves == true && activeEnemies.Count == 0)
        {
            WinGame();
        }
    }

    IEnumerator StartWaveLogic()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            Wave currentWave = waves[i];

            // Spawn the enemies for THIS wave
            for (int j = 0; j < currentWave.count; j++)
            {
                SpawnEnemy(currentWave.enemyPrefab);
                yield return new WaitForSeconds(currentWave.spawnRate);
            }

            // Wave finished. Wait before starting the next wave.
            if (i < waves.Length - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        // Loop is finished. All waves are done.
        finishedSpawningAllWaves = true;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[randomIndex];

        // Capture the new enemy in a variable
        GameObject newEnemy = Instantiate(enemyPrefab, sp.position, Quaternion.identity);

        // Add it to our tracking list
        activeEnemies.Add(newEnemy);
    }

    void WinGame()
    {
        if (invisibleWall != null)
            invisibleWall.SetActive(false); // Open the wall

        if (winText != null)
        {
            winText.gameObject.SetActive(true); // Show text
            winText.text = "You may proceed";
        }

        this.enabled = false; // Stop this script
    }
}