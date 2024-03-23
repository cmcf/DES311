using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float damageAmount = 10;
    [SerializeField] float projectileHeight;
    [SerializeField] GameObject splatPrefab;

    bool hitPlayer = false;
    bool splatSpawned = false;

    void Start()
     {
        // Play the sound when spit is fired
        FindObjectOfType<AudioManager>().Play("Spit");
    }

    public void Launch(Vector3 Destination)
    {
        // Calculates gravity magnitude in the scene
        float gravity = Physics.gravity.magnitude;
        float halfFlightTime = Mathf.Sqrt(projectileHeight * 2) / gravity;

        // Calculate the direction towards the destination
        Vector3 projectileDestination = Destination - transform.position;
        projectileDestination.y = 0;

        // Calculate the horizontal distances
        float horizontalDistance = projectileDestination.magnitude;

        // Calculate the forward direction
        Vector3 forwardDirection = projectileDestination.normalized;

        // Calculate the up speed
        float upSpeed = halfFlightTime * gravity;

        // Calculate the forward speed
        float forwardSpeed = horizontalDistance / (2 * halfFlightTime);

        // Scale down the forward speed to control the distance
        float projectileDistance = 5;
        float distanceScaleFactor = projectileDistance / horizontalDistance;
        forwardSpeed *= distanceScaleFactor;

        // Calculate the total flight velocity
        Vector3 flightVelocity = forwardDirection * forwardSpeed + Vector3.up * upSpeed;

        // Apply the flight velocity to the rigidbody
        rb.velocity = flightVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hitPlayer)
        {
            // Deals damage to player if hit
            hitPlayer = true;
            DealDamage(other.transform);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            return;
        }
        else if (!splatSpawned)
        {
            StartCoroutine(SpawnSplat());
            splatSpawned = true;
        }
    }

    IEnumerator SpawnSplat()
    {
        // Instantiate the splat prefab at a position with an offset
        Vector3 offsetPosition = transform.position - new Vector3(0, 4f, 0);
        // Instantiate the splat prefab at the position where the projectile landed
        Instantiate(splatPrefab, transform.position, Quaternion.identity);

        // Wait for a short delay before destroying splat
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    void OnTriggerExit(Collider other)
    {
       hitPlayer = false;
    } 

    void DealDamage(Transform target)
    {
        // Check if the target has a damageable component
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Deal damage to the target
            damageable.Damage(damageAmount);
        }
    }
}
