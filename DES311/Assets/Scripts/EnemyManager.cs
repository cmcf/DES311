using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

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
}
