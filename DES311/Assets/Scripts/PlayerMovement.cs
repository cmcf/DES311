using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joystick")]
    public VariableJoystick movementJoystick;
    public VariableJoystick aimJoystick;
    public Canvas inputCanvas;
    public CharacterController controller;

    //public Animator animator;
    [Header("Projectile Settings")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float projectileSpeed;
    [SerializeField] float firingCooldown = 1f;
    float lastFireTime;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    bool isMoving;
    bool isAiming;


    void Start()
    {
        EnableJoystick();
    }
    public void EnableJoystick()
    {
        // Enables the joystick by setting it to visible
        isMoving = true;
        inputCanvas.gameObject.SetActive(true);
    }

    void Update()
    {
        Movement();
        Aim();
    }

    void Movement()
    {
        // Checks if the joystick is enabled
        if (isMoving)
        {
            // Gets the movement direction from the joystick
            var movementDirection = new Vector3(movementJoystick.Direction.x, 0f, movementJoystick.Direction.y);

            // Moves the character using the SimpleMove method with speed
            controller.Move(movementDirection * Time.deltaTime * moveSpeed);

            // Checks if the player is not moving
            if (movementDirection.sqrMagnitude <= 0f)
            {
                // Disables walk animation
                //animator.SetBool("Walk", false);
                return;
            }

            // Walk animation plays when the player is moving
            //animator.SetBool("Walk", true);

            // If not aiming, rotate the character
            if (!isAiming)
            {
                // Calculates the target direction for character rotation
                var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);

                // Rotates the character towards the target direction
                controller.transform.rotation = Quaternion.LookRotation(targetDirection);
            }
        }
    }

    void Aim()
    {
        if (aimJoystick == null)
        {
           
            return;
        }

        var rotationDirection = new Vector3(aimJoystick.Direction.x, 0f, aimJoystick.Direction.y);
        if (rotationDirection.sqrMagnitude <= 0f)
        {
            isAiming = false;
            return;
        }

        isAiming = true;

        // Calculates and rotates the player character towards the target direction 
        var targetDirection = Vector3.RotateTowards(controller.transform.forward, rotationDirection, rotationSpeed * Time.deltaTime, 0f);
        controller.transform.rotation = Quaternion.LookRotation(targetDirection);

        // Fire projectile when the aim joystick is pressed and cooldown has passed
        if (aimJoystick.Direction.magnitude > 0.01f && CanFire())
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (Time.time - lastFireTime >= firingCooldown)
        {
            if (projectilePrefab == null || spawnPoint == null)
            {
                return;
            }

            // Spawn a new projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            // Get the rigidbody component of the projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Check if the projectile has a rigidbody
            if (projectileRb != null)
            {
                // Set the velocity of the projectile based on the world forward direction
                projectileRb.velocity = projectile.transform.forward * projectileSpeed;

                // Add a script for handling bullet behaviour
                projectile.AddComponent<Bullet>();
            }

            // Update the last fire time
            lastFireTime = Time.time;
        }
    }

    bool CanFire()
    {
        // Check if enough time has passed since the last firing
        return Time.time - lastFireTime >= firingCooldown;
    }

}
