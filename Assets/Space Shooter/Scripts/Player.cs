using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    public GameObject destructionFX;
    public static int globalScore = 0; // Global score that persists across all enemies
    
    [Header("UI Elements")]
    public Text scoreText; // Reference to the score UI text (Legacy UI Text component)

    public static Player instance; 

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private void Start()
    {
        UpdateScoreUI(); // Initialize the score display
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)   
    {
        Destruction();
    }    

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        Destroy(gameObject);
    }

    // Method to increase the global score
    public static void IncreaseScore(int points = 1)
    {
        globalScore += points;
        Debug.Log("Total Score: " + globalScore);
        
        // Update the UI if the player instance exists
        if (instance != null)
        {
            instance.UpdateScoreUI();
        }
    }

    // Method to update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + globalScore.ToString();
        }
    }
}
















