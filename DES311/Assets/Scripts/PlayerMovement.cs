using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public VariableJoystick joystick;
    public FixedJoystick aimJoystick;
    public Canvas inputCanvas;
    public CharacterController controller;

    //public Animator animator;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float projectileSpeed;
    [SerializeField] float firingCooldown = 1f;
    float lastFireTime;


    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    bool isJoystick;


    void Start()
    {
        EnableJoystick();
    }
    public void EnableJoystick()
    {
        // Enables the joystick by setting it to visible
        isJoystick = true;
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
        if (isJoystick)
        {
            // Gets the movement direction from the joystick
            var movementDirection = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
            // Moves the character using the SimpleMove method with speed
            controller.SimpleMove(movementDirection * moveSpeed);
            // Checks if the player is not moving
            if (movementDirection.sqrMagnitude <= 0f)
            {
                // Disables walk animation
                //animator.SetBool("Walk", false);
                return;
            }
            // Walk animation plays when the player is moving
            //animator.SetBool("Walk", true);

            // Calculates the target direction for character rotation
            var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);
            // Rotates the character towards the target direction
            controller.transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }

    void Aim()
    {
        if (aimJoystick == null)
        {
            Debug.LogError("Aim Joystick not assigned!");
            return;
        }

        var rotationDirection = new Vector3(aimJoystick.Direction.x, 0f, aimJoystick.Direction.y);
        if (rotationDirection.sqrMagnitude <= 0f)
        {
            return;
        }

        var targetDirection = Vector3.RotateTowards(controller.transform.forward, rotationDirection, rotationSpeed * Time.deltaTime, 0f);
        controller.transform.rotation = Quaternion.LookRotation(targetDirection);

        // Fire projectile when the aim joystick is pressed
        if (aimJoystick.Direction.magnitude > 0.1f && CanFire()) // Adjust the threshold as needed
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
                Debug.LogError("Projectile prefab or spawn point not assigned!");
                return;
            }

            // Instantiate a new projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            // Get the rigidbody component of the projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Check if the projectile has a rigidbody
            if (projectileRb != null)
            {
                // Set the velocity of the projectile based on the direction the player is facing
                projectileRb.velocity = controller.transform.forward * projectileSpeed;
            }
            else
            {
                Debug.LogError("Projectile prefab does not have a Rigidbody component!");
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
