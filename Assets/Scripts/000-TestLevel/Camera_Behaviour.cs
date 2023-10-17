using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float targetDistance;

    [SerializeField]
    private float cameraLerp;

    [SerializeField]
    private float sensivity;

    private float rotationX;
    private float rotationY;

    private RaycastHit hitInfo;

    private void LateUpdate()
    {
        rotationX += Input_Manager._INPUT_MANAGER.GetRotation().y * sensivity;
        rotationY += Input_Manager._INPUT_MANAGER.GetRotation().x * sensivity;

        rotationX = Mathf.Clamp(rotationX, -50f, 50f);

        transform.eulerAngles = new Vector3(rotationX, rotationY, 0);

        Vector3 finalPosition = Vector3.Lerp(transform.position, target.transform.position - transform.forward * targetDistance, cameraLerp * Time.deltaTime);

        if (Physics.Linecast(target.transform.position, finalPosition, out hitInfo))
        {
            finalPosition = hitInfo.point;
        }

        transform.position = finalPosition;
    }
}
