using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 60f;
    float currentHealth;

    MeshRenderer meshRenderer;
    Material redMaterial;

    public bool isDead = false;
    public float Health { get; set; }

    void Start()
    {
        currentHealth = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        redMaterial = Resources.Load<Material>("red");
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
        isDead = true;
        meshRenderer.material = redMaterial;
    }
   
}
