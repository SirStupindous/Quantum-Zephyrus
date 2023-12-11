using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Transform player;
    public float force;
    public float lifespan = 60.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePosition - player.position;
        Vector3 rotation = player.position - mousePosition;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<TilemapCollider2D>() != null)
        {
            // Collision with a 2D Tilemap Collider
            Destroy(gameObject);
            Debug.Log("Bullet hit the ground");
        }
        else
        {
            // Destroy the bullet when it collides with anything else
            EnemyScript enemy = collision.collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(); // Inflict damage to enemy

                if (!enemy.IsAlive())
                {
                    PlayerXP playerXP = player.GetComponent<PlayerXP>(); // Get PlayerXP reference

                    if (playerXP != null)
                    {
                        playerXP.AddXP(1); // Add 1 XP to player's counter
                        Debug.Log("Player XP: " + playerXP.GetXP());

                        // Enemy defeated, handle removal
                        Destroy(collision.gameObject);
                        Debug.Log("Enemy defeated!");
                    }
                }
            }

            Destroy(gameObject); // Destroy the bullet in any collision case
            Debug.Log("Bullet hit enemy alien");
        }
    }
}
