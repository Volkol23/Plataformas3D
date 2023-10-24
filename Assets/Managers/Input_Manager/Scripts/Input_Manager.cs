using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour
{
    public static Input_Manager _INPUT_MANAGER;

    private PlayerInputActions playerInputs;

    private float timeSinceJumpPressed = 0f;
    private float timeSinceCrouchPressed = 0f;

    private Vector2 leftAxisValue = Vector2.zero;
    private Vector2 rightAxisValue = Vector2.zero;

    private bool resetValue = false;
    private bool batButton = false;

    private void Awake()
    {
        //Check if it can be created this singleton
        if(_INPUT_MANAGER != null && _INPUT_MANAGER != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Create this singleton class
            _INPUT_MANAGER = this;
            DontDestroyOnLoad(gameObject);

            //Enable player inputs
            playerInputs = new PlayerInputActions();
            playerInputs.Player.Enable();

            //Perform the delegates of the actions of the player
            playerInputs.Player.Move.performed += LeftAxisUpdate;
            playerInputs.Player.Jump.performed += JumpButtonPressed;
            playerInputs.Player.Rotate.performed += RightAxisUpdate;
            playerInputs.Player.Reset.performed += ResetButtonPressed;
            playerInputs.Player.Crouch.performed += CrouchButtonHold;
            playerInputs.Player.Bat.performed += BatButtonPressed;
        }
    }
    void Update()
    {
        timeSinceJumpPressed += Time.deltaTime;
        timeSinceCrouchPressed += Time.deltaTime;
        InputSystem.Update();
    }

    //Check if JumpButton is Pressed
    private void JumpButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceJumpPressed = 0f;
    }

    //Check Left Axis input values
    private void LeftAxisUpdate(InputAction.CallbackContext context)
    {
        leftAxisValue = context.ReadValue<Vector2>();
    }

    //Check Right Axis input values
    private void RightAxisUpdate(InputAction.CallbackContext context)
    {
        rightAxisValue = context.ReadValue<Vector2>();
    }

    //Check if Reset Button is Pressed
    private void ResetButtonPressed(InputAction.CallbackContext context)
    {
        resetValue = !resetValue;
    }

    //Check if Crouch Button is Pressed
    private void CrouchButtonHold(InputAction.CallbackContext context)
    {
        timeSinceCrouchPressed = 0f;
    }

    //Check if Bat Button is Pressed
    private void BatButtonPressed(InputAction.CallbackContext context)
    {
        batButton = !batButton;
    }

    //Geters of the values of the inputs 
    public bool GetJumpButtonPressed()
    {
        return timeSinceJumpPressed == 0f;
    }

    public Vector2 GetMovement()
    {
        return leftAxisValue;
    }

    public Vector2 GetRotation()
    {
        return rightAxisValue;
    }

    public bool GetResetButtonPressed()
    {
        return resetValue;
    }

    public bool GetCrouchButtonPressed()
    {
        return timeSinceCrouchPressed == 0f;
    }

    public bool GetBatButtonPressed()
    {
        return batButton;
    }
}
