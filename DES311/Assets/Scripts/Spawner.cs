using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;

    [SerializeField] int initialEnemyAmount = 5;
    [SerializeField] int enemiesPerWaveIncrease = 2;
    [SerializeField] float delayBetweenWaves = 2f;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] float spawnProbability = 0.8f;

    int currentWave = 0;
    int currentEnemyAmount;

    void Start()
    {
        currentEnemyAmount = initialEnemyAmount;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // Waits until the delay has finished before spawning a new wave of enemies
        yield return new WaitForSeconds(delayBetweenWaves);

        // Iterates through each enemy to be spawned in the current wave
        for (int i = 0; i < currentEnemyAmount; i++)
        {
            // Iterate through each spawn point in the scene
            foreach (Transform spawnPoint in spawnPoints)
            {
                // Randomly selects an enemy prefab from the enemy prefab array
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Check a random chance for spawning at this spawn point
                if (Random.value < spawnProbability)
                {
                    // Instantiate the selected enemy prefab at the current spawn point's position with no rotation
                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                    // Break after spawning one enemy per spawn point
                    break;
                }
            }

            // Controls the rate of spawning
            yield return new WaitForSeconds(spawnRate);
        }

        // Wait until all enemies in the waves are defeated
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        // Increase difficulty by adding more enemies each wave
        currentWave++;
        currentEnemyAmount += enemiesPerWaveIncrease;

        // Start next wave
        StartCoroutine(SpawnWave());
    }
}
