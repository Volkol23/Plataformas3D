using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCappy : MonoBehaviour
{
    [SerializeField]
    private float angleRotation;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float timeMoving;

    [SerializeField]
    private bool collisionActive = false;

    private float currentiTime = 0f;
    private BoxCollider boxCollider;

    private void Awake()
    {
        currentiTime = 0f;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        boxCollider.enabled = collisionActive;
        currentiTime += Time.deltaTime;
        if(currentiTime < timeMoving)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.Rotate(Vector3.up, angleRotation);
            collisionActive = true;
        }
    }
}
