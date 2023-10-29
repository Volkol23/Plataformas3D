using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Interactions with the player
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Open");
        }
    }
}
