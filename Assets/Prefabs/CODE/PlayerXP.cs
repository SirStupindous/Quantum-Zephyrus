using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerXP : MonoBehaviour
{
    private int xp = 0; // Initial XP
    private int xpToLevelUp = 100; // XP required to level up
    public int health = 100; // Initial health
    public HealthScript healthBar; // Reference to the HealthScript component
    private int currentLevel = 1; // Private field to hold the current level

    void Start()
    {
        // Set the player's health to the max health
        healthBar.SetMaxHealth(health);
    }

    public void AddXP(int amount)
    {
        xp += amount;
        CheckLevelUp();
    }

    public int GetXP()
    {
        return xp;
    }

    public int GetHealth()
    {
        return health;
    }

    public void CheckLevelUp()
    {
        if (xp >= xpToLevelUp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        xp -= xpToLevelUp; // Reduce XP by the amount required for the level up
        // Perform actions for leveling up (e.g., increasing player's stats, resetting XP)
        currentLevel++; // Increment the private field currentLevel
        xpToLevelUp = currentLevel * 100; // Update xpToLevelUp based on the new level
        xp = 0; // Reset XP
    }

    // Public property to get the level
    public int level
    {
        get { return currentLevel; } // Use the private field currentLevel
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        Debug.Log("Player health: " + health);
        
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player died");
            // Handle player death here (e.g., scene reload)
            SceneManager.LoadScene(2);
        }
    }
}
