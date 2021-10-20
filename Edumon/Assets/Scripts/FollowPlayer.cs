using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 minValues, maxValues;

    private void FixedUpdate()
    {
        Follow();
    }

    // Update is called once per frame
    void Follow()
    {
        //Define min. x, y, z values and max x, y, z values
        
        Vector3 targetPosition = target.position + offset;

        //Verify if the targetPosition is out of bound or not
        //Limit it to min and max values
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z)
        );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;

        // transform.position = player.transform.position + new Vector3(
		// 	Mathf.Clamp(player.transform.position.x, leftLimit, rightLimit),
		// 	Mathf.Clamp(player.transform.position.y ,bottomLimit, topLimit),
		// 	transform.position.z
		// );
    }
}
