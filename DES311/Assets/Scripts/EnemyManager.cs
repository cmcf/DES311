using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<MeleeEnemy> enemies = new List<MeleeEnemy>();
    public void RegisterEnemy(MeleeEnemy enemy)
     {
        enemies.Add(enemy);
    }
    public void LevelUpEnemies()
    {
        foreach (MeleeEnemy enemy in enemies)
        {
            enemy.IncreaseEnemyStats();

        }
    }

    // Method to wipe out all enemies
    public void WipeOutEnemies()
    {
        // Create a copy of the enemies list
        List<MeleeEnemy> enemiesCopy = new List<MeleeEnemy>(enemies);

        // Iterate over the copy of the enemies list
        foreach (MeleeEnemy enemy in enemiesCopy)
        {
            // Check if the enemy is null or its game object is null
            if (enemy == null || enemy.gameObject == null)
            {
                // Remove the null or destroyed enemy from the original list
                enemies.Remove(enemy);
                continue;
            }

            // Check if the enemy's game object is active
            if (enemy.gameObject.activeSelf)
            {
                // If the enemy is active, destroy it
                Destroy(enemy.gameObject);
                // Remove the destroyed enemy from the original list
                enemies.Remove(enemy);
            }
        }
    }

    public void IncreaseEnemyHealth(float amount)
    {
        Debug.Log("Increasing enemy health by: " + amount);
        foreach (MeleeEnemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.IncreaseHealth(amount);
            }
        }
    }
}
