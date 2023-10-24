using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCappy : MonoBehaviour
{
    [Header("Bat variables")]
    [SerializeField] private float angleRotation;
    [SerializeField] private float speed;
    [SerializeField] private float timeMoving;

    private bool collisionActive;
    private float currentTime;
    private Vector3 direction;
    private BoxCollider boxCollider;

    private void Awake()
    {
        collisionActive = false;
        currentTime = 0f;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        boxCollider.enabled = collisionActive;
        currentTime += Time.deltaTime;

        if(currentTime < timeMoving)
        {
            //Move BatCappy forward
            if(direction != null)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else
        {
            //Enable BatCappy Behaviour
            transform.Rotate(Vector3.up, angleRotation);
            collisionActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Interaction with the player
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void GetPlayerDirection(Vector3 playerDirection)
    {
        direction = playerDirection;
    }
}
