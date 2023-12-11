using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float hoverHeight = 2.0f;
    public float movementSpeed = 1.5f;
    public float jumpForce = 2.5f;
    public LayerMask terrainLayer;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Freeze rotation so that the enemy doesn't fall over
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(HoverOverGround());
    }

    IEnumerator HoverOverGround()
    {
        while (true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, hoverHeight, terrainLayer);

            if (hit.collider != null)
            {
                rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (distanceToPlayer <= 10 && distanceToPlayer > 1)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        Vector2 moveDirection = new Vector2(directionToPlayer.x, 0).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1.0f, terrainLayer);

        if (hit.collider != null)
        {
            moveDirection = Vector2.Reflect(moveDirection, hit.normal);
        }

        rb.velocity = moveDirection * movementSpeed;

        // Face the player without rotating around the Z-axis
        Vector3 lookDir = player.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Set the rotation directly without affecting the z-axis rotation
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
