using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float maxSpeed = 3f; // Maximum speed
    public float runningSpeed = 6f;
    public float maxRunningSpeed = 6f;
    public float jumpForce = 5f; // Jump force
    public float jumpTime = 0.5f; // Max time the jump button can be held down
    public Transform groundCheckPoint; // The point where we check if the player is touching the ground
    public float groundCheckRadius; // The radius of the ground check circle
    public LayerMask groundLayer; // The layer of the ground
    public float wallCheckDistance = 1f; // Distance from plater center to wall check points.
    public float wallJumpForce = 10f;
    public LayerMask wallLayer;
    public float controlDelay = 0.2f; // Temporarilly ignores horizontal input after a wall jump
    public int lives = 3; // Number of lives
    public TextMeshProUGUI livesText; // Reference to the UI Text component
    public GameObject DeathScreen;
    public GameObject GameOverScreen;
    public int coinCount = 0;
    public TextMeshProUGUI coinCountText;
    public LayerMask blockLayer;


    private float jumpTimeCounter; // Counts down to zero
    private bool isGrounded;
    private bool isTouchingLeftWall, isTouchingRightWall;
    private Vector2 leftWallCheckPoint, rightWallCheckPoint;
    private Rigidbody2D rb;
    private float direction = 1f;
    private float controlRestoreTime;
    private Vector3 respawnPoint; // Player's respawn point after death

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimeCounter = jumpTime;
        animator = GetComponent<Animator>();

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));

        // Set the respawn point to the initial position
        //respawnPoint = transform.position;

        // Update the lives text
        //livesText.text = "Lives: " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer); // Checks if player is on ground
        animator.SetBool("IsGrounded", isGrounded); // Animation check for ground

        rightWallCheckPoint = new Vector2(transform.position.x + wallCheckDistance, transform.position.y);
        leftWallCheckPoint = new Vector2(transform.position.x - wallCheckDistance, transform.position.y);

        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheckPoint, groundCheckRadius, wallLayer); // Checks if player is touching left wall
        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheckPoint, groundCheckRadius, wallLayer); // Right side

        // In your Update method, draw debug lines to visualize the wall detection
        Debug.DrawRay(transform.position, Vector2.right * wallCheckDistance, isTouchingRightWall ? Color.green : Color.red);
        Debug.DrawRay(transform.position, Vector2.left * wallCheckDistance, isTouchingLeftWall ? Color.green : Color.red);


        float moveHorizontal = Input.GetAxis("Horizontal");

        //livesText.text = "Lives: " + lives; // Update the UI Text

        // If the player's y-coordinate drops below -10, respawn
        if (transform.position.y < -10)
        {
            GameManager.instance.PlayerDied();
        }

        if (Mathf.Abs(moveHorizontal) > 0.1f)
        {
            direction = Mathf.Sign(moveHorizontal);
            animator.SetBool("IsWalking", true);
        }
        else if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        animator.SetFloat("Direction", direction);

        if (Time.time >= controlRestoreTime)
        {
            // Gradually restore control over 0.1 seconds
            float t = (Time.time - controlRestoreTime) / 0.1f;
            moveHorizontal = Mathf.Lerp(0, moveHorizontal, t);

            Vector3 movement = new Vector3(moveHorizontal, 0, 0);

            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if (Input.GetKey(KeyCode.LeftShift))
            {
                //running state
                if (Mathf.Abs(rb.velocity.x) < maxRunningSpeed)
                {
                    rb.AddForce(new Vector2(movement.x * runningSpeed, 0));
                }
                animator.SetBool("IsRunning", true);
            }
            else
            {
                //walking state
                if (Mathf.Abs(rb.velocity.x) < maxSpeed)
                {
                    rb.AddForce(new Vector2(movement.x * speed, 0));
                }
                animator.SetBool("IsRunning", false);
            }

        }
        else if (Time.time < controlRestoreTime)
        {
            float t = (Time.time - (controlRestoreTime - controlDelay)) / controlDelay;
            moveHorizontal = Mathf.Lerp(0, moveHorizontal, t);
        }
        Flip();

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Jump"))
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
        }

        if (isGrounded)
        {
            jumpTimeCounter = jumpTime;
        }

        float wallJumpForceVertical = wallJumpForce * 0.8f; // less force vertically
        float wallJumpForceHorizontal = wallJumpForce * 1.2f; // more force horizontally

        if (!isGrounded && (isTouchingLeftWall || isTouchingRightWall) && Input.GetButtonDown("Jump"))
        {
            Vector2 direction = isTouchingLeftWall ? Vector2.right : Vector2.left;

            // Reset vertical velocity to ensure consistent jumps
            rb.velocity = new Vector2(rb.velocity.x, 0);

            // Add force 
            rb.velocity = new Vector2(wallJumpForceHorizontal * direction.x, wallJumpForceVertical);
            controlRestoreTime = Time.time + controlDelay;

            this.direction = direction.x;
            Flip();
        }

        animator.SetFloat("VerticalVelocity", rb.velocity.y);

        if (Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        bool isJumping = !isGrounded && rb.velocity.y > 0;

        if (isJumping) // The isJumping flag should be true when the player is moving upwards in a jump.
        {
            // Cast a ray upwards to check for a breakable block.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, blockLayer);

            if (hit.collider != null && hit.collider.GetComponent<BreakableBlock>() != null)
            {
                // Break the block
                hit.collider.GetComponent<BreakableBlock>().Break();
            }
        }

    }

    void Flip()
    {
        transform.localScale = new Vector3(direction, 1f, 1f);
    }

    // Detect collision with enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.PlayerDied();
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            GameManager.instance.CoinCollected();

            Destroy(other.gameObject);
        }
    }
}