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

    [SerializeField] float initialDelayBetweenWaves = 2f;
    [SerializeField] float delayIncreasePerWave = 1f;

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
        while (true) // Infinite loop for spawning waves indefinitely
        {
            yield return new WaitForSeconds(initialDelayBetweenWaves + (currentWave * delayIncreasePerWave));

            for (int i = 0; i < currentEnemyAmount; i++)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                    if (Random.value < spawnProbability)
                    {
                        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                        break;
                    }
                }

                yield return new WaitForSeconds(spawnRate);
            }

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
