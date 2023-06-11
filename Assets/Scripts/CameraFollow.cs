using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float minX;
    public float maxX;

    private Vector3 offset;

    void Start()
    {
        // Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        // Follow the player horizontally, within the limits minX and maxX
        float newX = Mathf.Clamp(player.transform.position.x + offset.x, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
