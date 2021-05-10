using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerToFollow;
    Transform playerTransform;
    Vector3 cameraVelocity;
    Vector3 cameraOffset;


    // Start is called before the first frame update
    void Start()
    {
        cameraVelocity = Vector3.zero;
        cameraOffset = new Vector3(0, 6, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFollow)
        {
            playerTransform = playerToFollow.transform;
            transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position+cameraOffset, ref cameraVelocity, 1f);
        }        
    }

    public void FollowPlayer(GameObject player)
    {
        playerToFollow = player;
        playerTransform = playerToFollow.transform;
    }
}
