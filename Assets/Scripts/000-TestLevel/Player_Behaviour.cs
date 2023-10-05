using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;

    [Header("Jump variables")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float tripleJumpForce;

    private Vector3 finalVelocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private float accelerationIncrease;

    private float jumpTimer;

    //Components of the gameObject
    private CharacterController characterController;

    private void Awake()
    {
        //Initialize components
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInputs();
        HandleJump();
        HandleMovement();
    }

    private void HandleInputs()
    {
        //Handle input values from the controller
        direction = Input_Manager._INPUT_MANAGER.GetMovement().x * transform.right + Input_Manager._INPUT_MANAGER.GetMovement().y * transform.forward;
        accelerationIncrease = Input_Manager._INPUT_MANAGER.GetMovement().magnitude;

        direction.Normalize();
    }

    private void HandleMovement()
    {
        //Movement behaviour with acceleration
        if(accelerationIncrease > 0f)
        {
            speed += acceleration * accelerationIncrease * Time.deltaTime;
        } 
        else
        {
            speed -= deacceleration * Time.deltaTime;
        }
        
        if(speed > maxSpeed)
        {
            speed = maxSpeed;
        } 
        else if(speed < 0f)
        {
            speed = 0f;
        }

        finalVelocity.x = direction.x * speed;
        finalVelocity.z = direction.z * speed;

        characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        direction.y = -1f;

        //Jump behaviour with gravity
        if (characterController.isGrounded)
        {
            if (Input_Manager._INPUT_MANAGER.GetJumpButtonPressed())
            {
                finalVelocity.y = jumpForce;
            }
            else
            {
                finalVelocity.y = direction.y * gravity * Time.deltaTime;
            }
        }
        else
        {
            finalVelocity.y += direction.y * gravity * Time.deltaTime;
        }
    }
}
