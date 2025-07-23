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
    
    [Header("Health System")]
    public int maxHealth = 3; // Maximum health/lives
    public int currentHealth; // Current health
    
    [Header("UI Elements")]
    public Text scoreText; // Reference to the score UI text (Legacy UI Text component)
    public Transform heartContainer; // Parent object to hold heart sprites
    public GameObject heartPrefab; // Heart sprite prefab
    
    private List<GameObject> heartSprites = new List<GameObject>(); // List to track heart UI elements

    public static Player instance; 

    private void Awake()
    {
        if (instance == null) 
            instance = this;
            
        // Initialize health
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateScoreUI(); // Initialize the score display
        InitializeHeartUI(); // Initialize the heart display
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)   
    {
        currentHealth -= damage;
        UpdateHeartUI();
        
        if (currentHealth <= 0)
        {
            Destruction();
        }
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
    
    // Method to initialize heart UI
    private void InitializeHeartUI()
    {
        if (heartContainer == null || heartPrefab == null) return;
        
        // Clear existing hearts
        ClearHeartUI();
        
        // Create heart sprites based on max health
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartSprites.Add(heart);
        }
        
        UpdateHeartUI();
    }
    
    // Method to update heart UI display
    private void UpdateHeartUI()
    {
        if (heartSprites.Count == 0) return;
        
        for (int i = 0; i < heartSprites.Count; i++)
        {
            if (i < currentHealth)
            {
                heartSprites[i].SetActive(true); // Show heart
            }
            else
            {
                heartSprites[i].SetActive(false); // Hide heart (or you could change sprite to empty heart)
            }
        }
    }
    
    // Method to clear heart UI
    private void ClearHeartUI()
    {
        foreach (GameObject heart in heartSprites)
        {
            if (heart != null)
                Destroy(heart);
        }
        heartSprites.Clear();
    }
    
    // Method to add health (for health pickups)
    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHeartUI();
    }
    
    // Method to get current health (useful for other scripts)
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
















