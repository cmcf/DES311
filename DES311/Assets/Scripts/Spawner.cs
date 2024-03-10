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
        while (true)
        {
            // Spawns new wave of enemies after a delay
            yield return new WaitForSeconds(initialDelayBetweenWaves + (currentWave * delayIncreasePerWave));

            // Iterates through each enemy to be spawned in the current wave
            for (int i = 0; i < currentEnemyAmount; i++)
            {
                // Iterates through each spawn point in the scene
                foreach (Transform spawnPoint in spawnPoints)
                {
                    // Selects a random enemy prefab from the array
                    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                    // Checks a random value for spawning at a spawn point
                    if (Random.value < spawnProbability)
                    {
                        // Spawns an enemy at a spawn point with no rotation
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

                        // Break after spawning one enemy per spawn point
                        break;
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
