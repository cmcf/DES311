using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] EnemyManager enemyManager;

    [SerializeField] int initialEnemyAmount = 5;
    [SerializeField] int enemiesPerWaveIncrease = 2;
    [SerializeField] float delayBetweenWaves = 2f;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] float spawnProbability = 0.8f;

    [SerializeField] float initialDelayBetweenWaves = 2f;
    [SerializeField] float delayIncreasePerWave = 1f;
    [SerializeField] float initialEnemyHealth = 30;

    [SerializeField] int spawnFromIndex1AfterWave = 2;

    int currentWave = 0;
    int currentEnemyAmount;
    bool canSpawn = true;

    void Start()
    {
        currentEnemyAmount = initialEnemyAmount;
        StartCoroutine(SpawnWave());
        CheckCurrentWave();
    }

    IEnumerator SpawnWave()
    {
        while (canSpawn)
        {
            // Spawns new wave of enemies after a delay
            yield return new WaitForSeconds(initialDelayBetweenWaves + (currentWave * delayIncreasePerWave));

            Debug.Log("Starting spawn wave.");

            // Counter to track the number of index 1 enemies spawned in this wave
            int index1PrefabSpawnedCount = 0;

            // Iterates through each enemy to be spawned in the current wave
            for (int i = 0; i < currentEnemyAmount; i++)
            {
                // Determine the spawn point for this enemy
                Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // Alternates between the spawn points

                // Selects a random enemy prefab from the array
                int prefabIndex;

                // Check if index 1 should be spawned
                if (currentWave >= spawnFromIndex1AfterWave && index1PrefabSpawnedCount < 2)
                {
                    prefabIndex = 1;
                    index1PrefabSpawnedCount++;
                }
                else
                {
                    prefabIndex = 0;
                }

                GameObject enemyPrefab = enemyPrefabs[prefabIndex];

                // Checks a random value for spawning at a spawn point
                if (Random.value < spawnProbability)
                {
                    // Spawns an enemy at the selected spawn point with no rotation
                    GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                    // Register the spawned enemy with the EnemyManager
                    enemyManager.RegisterEnemy(enemyObject.GetComponent<Enemy>());

                    // Get the Enemy component from the instantiated enemy object
                    Enemy enemy = enemyObject.GetComponent<Enemy>();

                    // Check if the enemy component exists
                    if (enemy != null)
                    {
                        // Set the default health of the enemy
                        enemy.currentHealth = enemy.defaultHealth;
                        // Register the spawned enemy with the EnemyManager
                        enemyManager.RegisterEnemy(enemy);
                    }
                    else
                    {
                        Debug.LogError("Enemy script not found on the instantiated object.");
                    }
                }
                // Controls the spawn rate
                yield return new WaitForSeconds(spawnRate);
            }

            // Increase difficulty by spawning more enemies each wave
            currentWave++;
            currentEnemyAmount += enemiesPerWaveIncrease;
        }

    }

    
    void CheckCurrentWave()
    {
        // Stop spawning waves after wave 10 is reached
        if (currentWave == 10)
        {
            canSpawn = false;
        }
    }
}

