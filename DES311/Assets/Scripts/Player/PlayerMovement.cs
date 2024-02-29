using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    public WeaponItem currentLoadout;

    [Header("Joystick")]
    public VariableJoystick movementJoystick;
    public VariableJoystick aimJoystick;
    public Canvas inputCanvas;
    public CharacterController controller;

    [Header("Projectile")]
    [SerializeField] Transform spawnPoint;

    public bool isAiming;
    public bool isFiring = false;

    [Header("Movement Settings")]
    [SerializeField] float rotationSpeed;

    
    float lastFireTime;

    bool isMoving;

    void Start()
    {
        ResetPlayerStats();
        EnableJoystick();
    }
    public WeaponItem GetDefaultWeapon()
    {
        return currentLoadout;
    }

    void ResetPlayerStats()
    {
        // Reset default rifle upgrades to their base values
        currentLoadout.cooldown = currentLoadout.baseCooldown;
        currentLoadout.speed = currentLoadout.baseSpeed;
        currentLoadout.moveSpeed = currentLoadout.baseMoveSpeed;
        currentLoadout.projectilePrefab = currentLoadout.defaultProjectile;
        // Reset health
        currentLoadout.healthMaxValue = currentLoadout.baseHealth;
        currentLoadout.health = currentLoadout.baseHealth;
    }
    public void EnableJoystick()
    {
        // Enables the joystick by setting it to visible
        isMoving = true;
        inputCanvas.gameObject.SetActive(true);
        animator = GetComponent<Animator>();
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
            controller.Move(movementDirection * Time.deltaTime * currentLoadout.moveSpeed);

            // Calculate the forward and right speed based on movement direction and player position
            float forward = Vector3.Dot(movementDirection, transform.forward);
            float right = Vector3.Dot(movementDirection, transform.right);

            // Sets the forward speed parameter in the animator controller
            animator.SetFloat("ForwardSpeed", forward);

            // Sets the right speed parameter in the animator controller
            animator.SetFloat("RightSpeed", right);

            // Checks if the player is not moving   
            if (movementDirection.sqrMagnitude <= 0f)
            {
                return;
            }

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
            isFiring = true;
            FireProjectile();
        }
      
    }

    void FireProjectile()
    {

        if (Time.time - lastFireTime >= currentLoadout.cooldown)
        {
            if (currentLoadout.projectilePrefab == null || spawnPoint == null)
            {
                return;
            }

            // Spawn a single projectile
            GameObject projectile = Instantiate(currentLoadout.projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.velocity = projectile.transform.forward * currentLoadout.speed;


            // Update last fire time and call StopFiring
            lastFireTime = Time.time;
            StartCoroutine(StopFiring(currentLoadout.cooldown));
        }
    }

    // Example method to check if the player has a specific projectile equipped using a tag
    public bool HasProjectileWithTag(string tag)
    {
        // Check if the player has a projectile equipped and if its tag matches the desired tag
        if (currentLoadout.projectilePrefab != null && currentLoadout.projectilePrefab.CompareTag(tag))
        {
            return true; // Player has the projectile with the desired tag equipped
        }
        else
        {
            return false; // Player does not have the projectile with the desired tag equipped
        }
    }

    IEnumerator StopFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFiring = false;
    }

    bool CanFire()
    {
        // Check if enough time has passed since the last firing
        return Time.time - lastFireTime >= currentLoadout.cooldown;
    }

}
