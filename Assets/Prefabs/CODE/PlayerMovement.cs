using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedX, speedY;
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private int maxJumpCount = 2; // Set the maximum number of jumps here
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    public Transform spawnPoint;
    public float respawnY = -1.0f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the player's position to the spawn point
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Makes the player move left and right
        float dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * speedX, rb.velocity.y);

        // Check if the player is grounded
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Reset jump count when grounded
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Check if the player's Y coordinate is below the respawn threshold
        if (transform.position.y < respawnY)
        {
            Respawn();
        }

        // Makes the player jump if they explicitly press the jump button
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumpCount))
        {
            rb.velocity = new Vector2(rb.velocity.x, speedY);
            jumpCount++;
        }
    }

    // Respawn the player at the spawn point
    private void Respawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            rb.velocity = Vector2.zero;
        }
    }
}
