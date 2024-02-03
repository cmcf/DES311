using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public VariableJoystick joystick;
    public Canvas inputCanvas;
    public CharacterController controller;
    //public Animator animator;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    bool isJoystick;

    void Start()
    {
        EnableJoystick();
    }
    public void EnableJoystick()
    {
        // Sets the joystick to visible
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isJoystick)
        {
            
            var movementDirection = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
            controller.SimpleMove(movementDirection * moveSpeed);
            // If player is not moving
            if (movementDirection.sqrMagnitude <= 0f) 
            {
                // Disables walk animation
                //animator.SetBool("Walk", false);
                return;
            }
            // Walk animation plays when the player is moving
            //animator.SetBool("Walk", true);

            var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);

            controller.transform.rotation = Quaternion.LookRotation(targetDirection);   
        }
    }
}
