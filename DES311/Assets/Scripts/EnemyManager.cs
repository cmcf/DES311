using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void LevelUpEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.IncreaseEnemyStats();
        }
    }

    // Method to wipe out all enemies
    public void WipeOutEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            // Check if the enemy has been destroyed before accessing it
            if (enemy != null && !enemy.IsDestroyed())
            {
                Destroy(enemy.gameObject);
            }
        }
        // Clear the list of enemies
        enemies.Clear();
    }
}
