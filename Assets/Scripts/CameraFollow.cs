using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The plane's transform
    public float distance = 10.0f; // Distance from the target
    public float height = 3.0f; // Height offset from the target
    public float positionSmoothTime = 0.3f; // Smoothing time for the position
    public float rotationSmoothTime = 0.3f; // Smoothing time for the rotation

    private Vector3 positionVelocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, positionSmoothTime);

            // Calculate the desired rotation for the camera
            Vector3 targetDirection = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate the camera towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime * Time.deltaTime);
        }
    }
}
