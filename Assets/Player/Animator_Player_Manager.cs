using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Player_Manager : MonoBehaviour
{
    private Player_Behaviour playerBehaviour;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerBehaviour = GetComponent<Player_Behaviour>();
    }

    private void Update()
    {
        //Scrap For final build
        //animator.SetFloat("Velocity", playerBehaviour.GetCurrentSpeed());

        //if (playerBehaviour.GetTripleJump())
        //{
        //    animator.SetTrigger("Triple Jump");
        //} 
        //else if (playerBehaviour.GetDoubleJump())
        //{
        //    animator.SetTrigger("Double Jump");
        //}
        //else if(playerBehaviour.GetJump())
        //{
        //    animator.SetTrigger("Jump");
        //}
    }
}
