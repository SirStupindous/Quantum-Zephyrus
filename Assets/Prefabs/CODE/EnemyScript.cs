using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health = 2; // Initial health
    private bool isTouchingPlayer = false;
    private float damageInterval = 1.0f; // Damage inflicted interval
    private float damageTimer = 0.0f;
    private int damagePerSecond = 5; // Damage per second
    private GameObject playerObject; // Reference to the player GameObject

    private void Start()
    {
        // Find and assign the player GameObject at the start
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isTouchingPlayer)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                DamagePlayer();
                damageTimer = 0.0f;
            }
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log("Enemy health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
            playerObject.GetComponent<PlayerXP>().AddXP(1); // Add XP to player
            Debug.Log("Enemy defeated!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            damageTimer = 0.0f; // Reset damage timer when not touching the player
        }
    }

    private void DamagePlayer()
    {
        PlayerXP playerXP = playerObject.GetComponent<PlayerXP>(); // Use playerObject reference

        if (playerXP != null)
        {
            playerXP.TakeDamage(damagePerSecond); // Inflict damage to player
            Debug.Log("Player health: " + playerXP.GetHealth());
        }
    }
}
