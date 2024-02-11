using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 60f;
    float currentHealth;
    
    // Gets the Position property from IDamageable interface
    public float Health { get; set; }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

    }
    public void Damage(float damage)
    {
        Debug.Log("PlayerHit");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }
   
}
