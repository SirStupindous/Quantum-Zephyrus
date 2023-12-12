using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressTracker : MonoBehaviour
{
    public Transform player; // The player's transform
    public TMP_Text progressText; // Reference to the UI Text component
    public float levelWidth = 1000f; // The width of the level
    private float progress; // The player's progress through the level
    private float playerPosition; // The player's position along the X axis
    private bool isLevelComplete = false; // Whether the level is complete
    public PlayerXP playerXP; // Reference to the PlayerXP script

    // Start is called before the first frame update
    private void Start()
    {
        playerXP = GetComponent<PlayerXP>();
    }

    private void Update()
    {
        // Calculate the player's position
        playerPosition = player.position.x;

        // Calculate progress as a percentage
        progress = (playerPosition / levelWidth) * 100f;

        // Update the UI Text to display the progress
        progressText.text = progress.ToString("F1") + "%";

        // Check if the player has reached 99% completion
        if (progress >= 99f && !isLevelComplete)
        {
            isLevelComplete = true;
            Debug.Log("Round complete!");

            // Save the total XP gained before transitioning to the next scene
            int totalXP = playerXP.GetXP();

            // Load the scene that displays total XP and options to continue or return to main menu
            SceneManager.LoadScene(2); // Make sure the scene name is correct
        }
    }
}
