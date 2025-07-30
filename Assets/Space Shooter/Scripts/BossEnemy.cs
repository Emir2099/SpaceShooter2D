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
        // Boss will shoot at a fixed interval, no random chance
        if (Projectile != null)
        {
            StartCoroutine(BossShootingPattern());
            if (debugMode) Debug.Log($"Boss {gameObject.name}: Started fixed interval shooting pattern (every {bossShootInterval} seconds)");
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
    
    // Boss-specific shooting pattern: shoot every bossShootInterval seconds, no random chance
    private IEnumerator BossShootingPattern()
    {
        if (debugMode) Debug.Log($"Boss {gameObject.name}: BossShootingPattern started (fixed interval)");
        yield return new WaitForSeconds(bossShootInterval); // Initial delay
        while (gameObject != null && health > 0)
        {
            if (Projectile != null)
            {
                Instantiate(Projectile, transform.position, Quaternion.identity);
                if (debugMode) Debug.Log($"Boss {gameObject.name}: Fired projectile at position {transform.position}");
            }
            else
            {
                Debug.LogError($"Boss {gameObject.name}: Tried to shoot but Projectile is NULL!");
            }
            yield return new WaitForSeconds(bossShootInterval);
        }
        if (debugMode) Debug.Log($"Boss {gameObject.name}: BossShootingPattern ended - GameObject:{gameObject != null}, Health:{health}");
    }
    
    // ActivateShooting is no longer needed (removed random chance logic)
    
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
