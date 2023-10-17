using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private bool isCrouching;

    [Header("Jump variables")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float tripleJumpForce;
    [SerializeField] private float backJumpForce;
    [SerializeField] private float backJumpSpeed;
    [SerializeField] private float maxTimeJump;

    [Header("Interactions Variables")]
    [SerializeField] private Vector3 bounceDirection;
    [SerializeField] private float bounceForce;
    [SerializeField] private int points = 0;

    private Vector3 finalVelocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;
    [SerializeField]
    private float accelerationIncrease;

    private float jumpTimer;
    private float doubleJumpTimer;

    private bool jump;
    private bool doubleJump;
    [SerializeField]
    private bool backJump = false;

    //Components of the gameObject
    private CharacterController characterController;

    //External objects
    private Camera mainCamera;

    private void Awake()
    {
        //Initialize components
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
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
        if(Input_Manager._INPUT_MANAGER.GetMovement().magnitude != 0)
        {
            direction = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f)
            * new Vector3(Input_Manager._INPUT_MANAGER.GetMovement().x, 0f, Input_Manager._INPUT_MANAGER.GetMovement().y);
            lastDirection = direction;
        }
       
        accelerationIncrease = Input_Manager._INPUT_MANAGER.GetMovement().magnitude;

        if (Input_Manager._INPUT_MANAGER.GetCrouchButtonPressed())
        {
            isCrouching = !isCrouching;
        }

        direction.Normalize();
        lastDirection.Normalize();
    }

    private void HandleMovement()
    {
        //Movement behaviour with acceleration
        if (accelerationIncrease > 0f)
        {
            speed += acceleration * Time.deltaTime;
        }
        else if (accelerationIncrease <= 0f)
        {
            speed -= deacceleration * Time.deltaTime;
        }

        //Handle max and min speed
        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        if (isCrouching)
        {
            speed = walkSpeed;
        }

        if (backJump)
        {
            finalVelocity.x = -transform.forward.x * backJumpSpeed;
            finalVelocity.z = -transform.forward.z * backJumpSpeed;
        }
        else
        {
            finalVelocity.x = lastDirection.x * speed;
            finalVelocity.z = lastDirection.z * speed;
        }
        Debug.Log(finalVelocity);
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
                if (doubleJumpTimer > 0.1f && doubleJump == true)
                {
                    //TripleJump
                    finalVelocity.y = tripleJumpForce;
                }
                else if (jumpTimer > 0.1f && jump == true)
                {
                    //Double Jump
                    finalVelocity.y = doubleJumpForce;
                    doubleJump = true;
                    doubleJumpTimer = maxTimeJump;
                }
                else if (isCrouching)
                {
                    //Back Jump
                    backJump = true;
                    finalVelocity.y = backJumpForce;
                }
                else
                {
                    //Normal Jump
                    jump = true;
                    jumpTimer = maxTimeJump;
                    finalVelocity.y = jumpForce;
                }
            }
            else
            {
                //backJump = false;
                finalVelocity.y = direction.y * gravity * Time.deltaTime;
            }
        }
        else
        {
            finalVelocity.y += direction.y * gravity * Time.deltaTime;
        }

        //Handle Jump Timers
        if (jump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if (doubleJump)
        {
            doubleJumpTimer -= Time.deltaTime;
        }
        if (jumpTimer < 0f)
        {
            jump = false;
        }
        if (doubleJumpTimer < 0f)
        {
            doubleJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Death Elements
        if (other.CompareTag("Death"))
        {
            Destroy(gameObject);
        }

        //Bounce Elements
        if (other.CompareTag("Bounce"))
        {
            finalVelocity = bounceDirection * bounceForce;
        }

        //Points Elements
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin Added");
            points++;
        }
    }
}
