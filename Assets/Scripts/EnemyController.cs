using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private bool movingRight = true;
    public Transform groundDetection;
    public Transform wallDetection; // Add this
    public float detectionDistance = 2f; // Distance to check for walls and ledges
    public LayerMask groundLayer; // Layer to detect ground
    public LayerMask wallLayer; // Layer to detect walls

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, detectionDistance, groundLayer);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, transform.right, detectionDistance, wallLayer); // Add this

        if (groundInfo.collider == false || wallInfo.collider == true) // Add this
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    public void FellEnemy()
    {
        Destroy(this.gameObject);
    }
}
