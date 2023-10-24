using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Behaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Interactions with the player
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
