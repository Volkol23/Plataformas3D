using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    [Header("Jump variables")]
    [SerializeField] private float gravity;

    private Vector3 finalVelocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;

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
        HandleMovement();
    }

    private void HandleInputs()
    {

        direction = +Input_Manager._INPUT_MANAGER.GetMovement().x * transform.right + Input_Manager._INPUT_MANAGER.GetMovement().y * transform.forward;

        
        direction.Normalize();

        finalVelocity.x = direction.x * speed;
        finalVelocity.z = direction.z * speed;
    }

    private void HandleMovement()
    {
        characterController.Move(finalVelocity * Time.deltaTime);
    }
}
