using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get the current position of the camera
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the position of the camera
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);

        // Explicitly set the y-axis rotation of the camera to 0
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}