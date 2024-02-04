using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 100f; // Adjust the maximum travel distance as needed
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the traveled distance
        float traveledDistance = Vector3.Distance(transform.position, initialPosition);

        // Check if the bullet has traveled far enough
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}

