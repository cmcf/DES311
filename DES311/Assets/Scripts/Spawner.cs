using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPortal;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] EnemyManager enemyManager;

    [SerializeField] int initialEnemyAmount = 5;
    [SerializeField] int enemiesPerWaveIncrease = 2;

    [SerializeField] float spawnRate = 1f;
    [SerializeField] float spawnProbability = 0.8f;

    [SerializeField] float delayBetweenWaves = 2f;
    [SerializeField] float delayIncreasePerWave = 1f;

    [SerializeField] int spawnFromIndex1AfterWave = 2;

    int currentWave = 0;
    int currentEnemyAmount;
    public bool canSpawn = true;
    bool bossSpawned = false;

    GameObject portal;

    void Start()
    {
        currentEnemyAmount = initialEnemyAmount;
        StartCoroutine(SpawnWave());
        CheckCurrentWave();
    }

    IEnumerator SpawnWave()
    {
        while (canSpawn && !bossSpawned)
        {
            // Spawns new wave of enemies after a delay
            yield return new WaitForSeconds(delayBetweenWaves + (currentWave * delayIncreasePerWave));

            // Check if it's the last wave
            if (currentWave == 6)
            {
                // Ensure that the enemyPrefabs array has enough elements
                if (enemyPrefabs.Length > 2)
                {
                    // Spawn the boss
                    GameObject bossPrefab = enemyPrefabs[2];
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject bossObject = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
                    bossSpawned = true;
                    enemyManager.RegisterEnemy(bossObject.GetComponent<Enemy>());
                }

            }
            else
            {
                if (bossSpawned) { break; }
                // Counter to track the number of ranged enemies spawned in current wave
                int rangedEnemyCount = 0;

                // Iterates through each enemy to be spawned in the current wave
                for (int i = 0; i < currentEnemyAmount; i++)
                {
                    // Determine the spawn point for this enemy
                    Transform spawnPoint = spawnPoints[i % spawnPoints.Length];

                    // Selects a random enemy prefab from the array
                    int prefabIndex;

                    // Checks if ranged enemy can be spawned
                    if (currentWave >= spawnFromIndex1AfterWave && rangedEnemyCount < 2)
                    {
                        prefabIndex = 1;
                        rangedEnemyCount++;
                    }
                    else
                    {
                        prefabIndex = 0;
                    }

                    GameObject enemyPrefab = enemyPrefabs[prefabIndex];

                    // Checks a random value for spawning at a spawn point
                    if (Random.value < spawnProbability)
                    {
                        SpawnPortal(spawnPoint);

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
                            enemy.currentHealth = enemy.maxHealth;
                            // Register the spawned enemy with the EnemyManager
                            enemyManager.RegisterEnemy(enemy);
                        }
                    }
                    // Controls the spawn rate
                    yield return new WaitForSeconds(spawnRate);
                }
            }

            // Increase difficulty by spawning more enemies each wave
            currentWave++;
            currentEnemyAmount += enemiesPerWaveIncrease;
        }
    }

    void CheckCurrentWave()
    {
        // Stop spawning waves after wave 10 is reached
        if (currentWave == 5)
        {
            canSpawn = false;
        }
    }

    void SpawnPortal(Transform spawnPoint)
    {
        // Set the spawn rotation
        float spawnRotationX = 100f;

        // Set the offset to lower the portal
        float yOffset = -0.8f;

        // Create a quaternion rotation
        Quaternion rotation = Quaternion.Euler(spawnRotationX, 0f, 0f);

        // Set the spawn position with the offset applied
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0f, yOffset, 0f);

        // Spawn the portal 
        GameObject portal = Instantiate(spawnPortal, spawnPosition, rotation);

        StartCoroutine(DestroyPortalWithDelay(portal, 1f));
    }

    IEnumerator DestroyPortalWithDelay(GameObject portal, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the portal object after delay
        Destroy(portal);
    }
}

