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
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationThreshold;

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
    [SerializeField] private float batForce;
    [SerializeField] private GameObject batCappy;
    [SerializeField] private GameObject batSpawner;
    [SerializeField] private AudioClip[] achieveAudios;

    //Movement Variables
    private Vector3 finalVelocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;

    private float accelerationIncrease;

    //Jump variables
    private float jumpTimer;
    private float doubleJumpTimer;

    private bool jump;
    private bool doubleJump;
    private bool backJump = false;

    //Wall Jump Variables
    private bool wallJump;
    private Vector3 wallNormal = Vector3.zero;

    //Components of the gameObject
    private CharacterController characterController;
    private Animator animator;
    private AudioSource audioSource;

    //External objects
    private Camera mainCamera;

    private void Awake()
    {
        //Initialize components
        characterController = GetComponent<CharacterController>();
        direction = transform.forward;
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleInputs();
        HandleRotation();
        HandleJump();
        HandleMovement();
    }

    private void HandleInputs()
    {
        //Handle input values form the movement
        if(Input_Manager._INPUT_MANAGER.GetMovement().magnitude != 0)
        {
            direction = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f)
            * new Vector3(Input_Manager._INPUT_MANAGER.GetMovement().x, 0f, Input_Manager._INPUT_MANAGER.GetMovement().y);
            lastDirection = direction;
        }
       
        accelerationIncrease = Input_Manager._INPUT_MANAGER.GetMovement().magnitude;

        //Check if it is Crouching
        isCrouching = Input_Manager._INPUT_MANAGER.GetCrouchButtonPressed();

        //Spawn Bat Cappy
        if (Input_Manager._INPUT_MANAGER.GetBatButtonPressed())
        {
            SpawnBat();
        }

        //Reset Input Button
        if (Input_Manager._INPUT_MANAGER.GetResetButtonPressed())
        {
            Game_Manager._GAME_MANAGER.ResetGame();
        }

        //Normaliza direction vectors
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
        else if (Input_Manager._INPUT_MANAGER.GetJumpButtonPressed())
        {
            if (wallJump)
            {
                //WallJump
                //animator.SetTrigger("Jump");
                direction = wallNormal.normalized;
                finalVelocity.y = jumpForce;
                wallJump = false;
            }
        }
        else
        {
            finalVelocity.x = lastDirection.x * speed;
            finalVelocity.z = lastDirection.z * speed;
        }

        //Animator Setup
        animator.SetFloat("Velocity", GetCurrentSpeed());

        //Move character controller
        characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        //Applt gravity direction
        direction.y = -1f;

        animator.SetBool("Grounded", characterController.isGrounded);

        //Jump behaviour with gravity
        if (characterController.isGrounded)
        {
            wallJump = false;

            if (Input_Manager._INPUT_MANAGER.GetJumpButtonPressed())
            {
                if (doubleJumpTimer > 0.1f && doubleJump == true)
                {
                    //TripleJump
                    animator.SetTrigger("Triple Jump");
                    finalVelocity.y = tripleJumpForce;
                }
                else if (jumpTimer > 0.1f && jump == true)
                {
                    //Double Jump
                    animator.SetTrigger("Double Jump");
                    finalVelocity.y = doubleJumpForce;
                    doubleJump = true;
                    doubleJumpTimer = maxTimeJump;
                }
                else if (isCrouching)
                {
                    //Back Jump
                    backJump = true;
                    isCrouching = false;
                    finalVelocity.y = backJumpForce;
                }
                else
                {
                    //Normal Jump
                    animator.SetTrigger("Jump");
                    jump = true;
                    jumpTimer = maxTimeJump;
                    finalVelocity.y = jumpForce;
                }
            }
            else
            {
                backJump = false;
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

    private void HandleRotation()
    {
        //Roatate character relative to the forward of the camera
        float rotation = Vector3.SignedAngle(direction, -transform.forward, transform.up);
        Debug.Log(rotation);
        if(direction != transform.forward /*&& rotation > rotationThreshold*/)
        {
            transform.Rotate(Vector3.up * rotation * Time.deltaTime);
        }
    }

    private void SpawnBat()
    {
        //Check if its a bat in the scene
        GameObject[] bats = GameObject.FindGameObjectsWithTag("Bat");

        if (bats.Length == 0)
        {
            Instantiate(batCappy, batSpawner.transform.position, Quaternion.identity).GetComponent<BatCappy>().GetPlayerDirection(transform.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Death Elements
        if (other.CompareTag("Death"))
        {
            Game_Manager._GAME_MANAGER.ResetGame();
        }

        //Bounce Elements
        if (other.CompareTag("Bounce"))
        {
            finalVelocity = bounceDirection * bounceForce;
        }

        //Points Elements
        if (other.CompareTag("Coin"))
        {
            AchievePoint();
            Game_Manager._GAME_MANAGER.UpdatePoints();
        }

        //Bat Cappy
        if (other.CompareTag("Bat"))
        {
            finalVelocity = bounceDirection * bounceForce;
        }
    }

    private void AchievePoint()
    {
        int index = Random.Range(0, 4);
        audioSource.clip = achieveAudios[index];
        audioSource.Play();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Wall Elements
        if (!characterController.isGrounded && hit.gameObject.CompareTag("Wall"))
        {
            wallJump = true;
            wallNormal = hit.normal;
        }
    }

    //Geters for Animator Manager
    public float GetCurrentSpeed()
    {
        return speed;
    }

    public bool GetJump()
    {
        return jump;
    }

    public bool GetDoubleJump()
    {
        return jumpTimer > 0.1f && jump == true;
    }

    public bool GetTripleJump()
    {
        return doubleJumpTimer > 0.1f && doubleJump == true;
    }
}
