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
    public WeaponItem currentWeapon;

    [Header("Joystick")]
    public VariableJoystick movementJoystick;
    public VariableJoystick aimJoystick;
    public Canvas inputCanvas;
    public CharacterController controller;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPoint;

    public bool isAiming;
    public bool isFiring = false;
    
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    
    float lastFireTime;

    bool isMoving;

    void Start()
    {
        // Initialize currentWeapon if necessary
        if (currentWeapon == null)
        {
            // For example, you can set it to a default weapon
            currentWeapon = GetDefaultWeapon();
        }
        EnableJoystick();
    }
    public WeaponItem GetDefaultWeapon()
    {
        // Implement logic to get the default weapon
        // For example, you might load it from a ScriptableObject
        return currentWeapon;
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
            controller.Move(movementDirection * Time.deltaTime * moveSpeed);

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
        
        if (Time.time - lastFireTime >= currentWeapon.cooldown)
        {
            if (projectilePrefab == null || spawnPoint == null)
            {
                return;
            }

            // Spawn a new projectile
            GameObject projectile = Instantiate(currentWeapon.projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            // Get the rigidbody component of the projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Check if the projectile has a rigidbody
            if (projectileRb != null)
            {
                // Set the velocity of the projectile based on the world forward direction
                projectileRb.velocity = projectile.transform.forward * currentWeapon.speed;

            }

            // Update the last fire time
            lastFireTime = Time.time;
            
            // Call StopFiring after a delay
            StartCoroutine(StopFiring(currentWeapon.cooldown));
        }
    }

    IEnumerator StopFiring(float delay)
    {
        yield return new WaitForSeconds(lastFireTime);
        isFiring = false;
    }

    bool CanFire()
    {
        // Check if enough time has passed since the last firing
        return Time.time - lastFireTime >= currentWeapon.cooldown;
    }

}
