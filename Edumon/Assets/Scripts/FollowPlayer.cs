using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 playerPosition;
    public float leftLimit, rightLimit, topLimit, bottomLimit;

    // Update is called once per frame
    void Update()
    {

        transform.position = player.transform.position + new Vector3(
			Mathf.Clamp(player.transform.position.x, leftLimit, rightLimit),
			Mathf.Clamp(player.transform.position.y ,bottomLimit, topLimit),
			transform.position.z
		);
    }
}
