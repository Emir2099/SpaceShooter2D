using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Specialized Enemy script for Boss enemies with enhanced debugging and setup validation
/// </summary>
public class BossEnemy : Enemy
{
    [Header("Boss Specific Settings")]
    [Tooltip("Boss shoots more frequently than regular enemies")]
    public float bossShootInterval = 2f;
    
    [Tooltip("Enable debug logging for boss behavior")]
    public bool debugMode = true;
    
    private bool isInitialized = false;
    
    private void Awake()
    {
        // Ensure boss has proper tag
        if (!gameObject.CompareTag("Enemy"))
        {
            gameObject.tag = "Enemy";
            if (debugMode) Debug.Log($"Boss {gameObject.name}: Set tag to Enemy");
        }
        
        // Validate collider setup
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.isTrigger)
        {
            col.isTrigger = false;
            if (debugMode) Debug.Log($"Boss {gameObject.name}: Disabled trigger on collider for damage detection");
        }
        
        // Ensure we have required components
        if (GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError($"Boss {gameObject.name}: Missing SpriteRenderer component!");
        }
        
        if (debugMode)
        {
            Debug.Log($"Boss {gameObject.name}: Initialized with {health} health");
            Debug.Log($"Boss {gameObject.name}: Projectile assigned: {(Projectile != null ? Projectile.name : "NULL")}");
        }
    }
    
    private void Start()
    {
        // Initialize shooting with random delay (replicate Enemy.Start() functionality)
        Invoke("ActivateShooting", UnityEngine.Random.Range(shotTimeMin, shotTimeMax));
        
        // Additional boss initialization
        if (debugMode)
        {
            Debug.Log($"Boss {gameObject.name}: Starting with shotChance={shotChance}, shotTimeMin={shotTimeMin}, shotTimeMax={shotTimeMax}");
        }
        
        // Start continuous shooting pattern for boss
        if (Projectile != null)
        {
            StartCoroutine(BossShootingPattern());
        }
        else
        {
            Debug.LogError($"Boss {gameObject.name}: No projectile assigned! Boss won't be able to shoot.");
        }
        
        isInitialized = true;
    }
    
    private void Update()
    {
        // Handle screen boundary check (replicate Enemy.Update() functionality)
        if (transform.position.y < -10f) // Adjust this value based on your screen size
        {
            // Since we can't invoke the static event directly from derived class,
            // we'll destroy the object and let the system handle it naturally
            // The LevelController tracks enemies through the OnEnemyDestroyed event instead
            Destroy(gameObject);
        }
    }
    
    // Boss-specific shooting pattern (more aggressive than regular enemies)
    private IEnumerator BossShootingPattern()
    {
        // Wait for initial delay
        yield return new WaitForSeconds(UnityEngine.Random.Range(shotTimeMin, shotTimeMax));
        
        while (gameObject != null && health > 0)
        {
            // Boss shoots more frequently
            if (UnityEngine.Random.value < (float)shotChance / 100)
            {
                if (Projectile != null)
                {
                    Instantiate(Projectile, transform.position, Quaternion.identity);
                    if (debugMode) Debug.Log($"Boss {gameObject.name}: Fired projectile");
                }
            }
            
            yield return new WaitForSeconds(bossShootInterval);
        }
    }
    
    // Single shot method (called by Invoke from Start, similar to Enemy class)
    private void ActivateShooting()
    {
        if (UnityEngine.Random.value < (float)shotChance / 100)
        {
            if (Projectile != null)
            {
                Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
                if (debugMode) Debug.Log($"Boss {gameObject.name}: Initial shot fired");
            }
        }
    }
    
    // Override GetDamage to add boss-specific behavior
    public new void GetDamage(int damage)
    {
        if (debugMode)
        {
            Debug.Log($"Boss {gameObject.name}: Taking {damage} damage. Health: {health} -> {health - damage}");
        }
        
        // Call base method
        base.GetDamage(damage);
        
        // Add boss-specific damage behavior if needed
        if (health <= 0 && debugMode)
        {
            Debug.Log($"Boss {gameObject.name}: Destroyed!");
        }
    }
    
    // Validate boss setup
    public void ValidateBossSetup()
    {
        Debug.Log("=== BOSS VALIDATION ===");
        Debug.Log($"Name: {gameObject.name}");
        Debug.Log($"Tag: {gameObject.tag}");
        Debug.Log($"Health: {health}");
        Debug.Log($"Projectile: {(Projectile != null ? Projectile.name : "NULL - CRITICAL ERROR!")}");
        Debug.Log($"SpriteRenderer: {(GetComponent<SpriteRenderer>() != null ? "Found" : "MISSING - CRITICAL ERROR!")}");
        Debug.Log($"Collider2D: {(GetComponent<Collider2D>() != null ? "Found" : "MISSING - CRITICAL ERROR!")}");
        Debug.Log($"Collider is Trigger: {(GetComponent<Collider2D>() != null ? GetComponent<Collider2D>().isTrigger.ToString() : "N/A")}");
        Debug.Log($"FollowThePath: {(GetComponent<FollowThePath>() != null ? "Found" : "MISSING - CRITICAL ERROR!")}");
        Debug.Log($"Shot Settings: Chance={shotChance}, TimeMin={shotTimeMin}, TimeMax={shotTimeMax}");
        Debug.Log("========================");
    }
}
