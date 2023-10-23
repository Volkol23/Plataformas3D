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
    private bool crouchValueHold = false;

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
            playerInputs.Player.Reset.performed += ResetButonPressed;
            playerInputs.Player.Crouch.started += CrouchButtonHold;
            playerInputs.Player.Crouch.canceled += CrouchButtonNotHold;
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
        Debug.Log("Jump Pressed");
    }

    //Check Left Axis input values
    private void LeftAxisUpdate(InputAction.CallbackContext context)
    {
        leftAxisValue = context.ReadValue<Vector2>();

        //Magnitud Velocidad max y min
        //Debug.Log("Magnitude: " + leftAxisValue.magnitude);
        //Direción de el movimiento
        //Debug.Log("Normalized: " + leftAxisValue.normalized);
    }

    //Check Right Axis input values
    private void RightAxisUpdate(InputAction.CallbackContext context)
    {
        rightAxisValue = context.ReadValue<Vector2>();

        //Magnitud Velocidad max y min
        //Debug.Log("Magnitude: " + rightAxisValue.magnitude);
        //Direción de el movimiento
        //Debug.Log("Normalized: " + rightAxisValue.normalized);
    }

    //Check if Reset Button is Pressed
    private void ResetButonPressed(InputAction.CallbackContext context)
    {
        resetValue = context.performed;
    }

    //Check if Crouch Button is Pressed
    private void CrouchButtonHold(InputAction.CallbackContext context)
    {
        crouchValueHold = true;
        //Debug.Log(crouchValue);
    }

    private void CrouchButtonNotHold(InputAction.CallbackContext context)
    {
        crouchValueHold = false;
        //Debug.Log(crouchValue);
    }

    private void BatButtonPressed(InputAction.CallbackContext context)
    {
        batButton = context.performed;
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
        return crouchValueHold;
    }

    public bool GetBatButtonPressed()
    {
        return batButton;
    }
}
