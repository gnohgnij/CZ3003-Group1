using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    // set the max distance between center of camera and the player
    public float boundX = 0.30f; 
    public float boundY = 0.15f;

    // called after Update() and FixedUpdate()
    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        // * Check whether the camera is inside the bounds wrt the X and Y axes
        // deltaX and deltaY is the diff between player's position and center of camera's position
        float deltaX = lookAt.position.x - transform.position.x; // transform.position is the center position of the current object (which is the CameraMotor object)
        if (deltaX > boundX || deltaX < -boundX) // camera is out of bounds (aka too far away from the player)
        {
            if (transform.position.x < lookAt.position.x) // center of camera is positioned to the left of the player
            {
                delta.x = deltaX - boundX;
            }
            else // center of camera is positioned to the right of the player
            {
                delta.x = deltaX + boundX;
            }
        }
        float deltaY = lookAt.position.y - transform.position.y; // transform.position is the center position of the current object (which is the CameraMotor object)
        if (deltaY > boundY || deltaY < -boundY) // camera is out of bounds (aka too far away from the player)
        {
            if (transform.position.y < lookAt.position.y) // center of camera is positioned to the left of the player
            {
                delta.y = deltaY - boundY;
            }
            else // center of camera is positioned to the right of the player
            {
                delta.y = deltaY + boundY;
            }
        }

        // * Move the camera
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}

