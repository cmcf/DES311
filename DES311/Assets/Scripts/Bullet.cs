using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] float damageAmount = 15f;
    Enemy enemy;
    [SerializeField] float destroyDelay = 3f;
    void Start()
    {
        initialPosition = transform.position;
        // Call the DestroyActor method after a delay
        Invoke(nameof(DestroyBullet), destroyDelay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            other.GetComponent<Enemy>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

}

