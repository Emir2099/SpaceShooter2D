using UnityEngine;

/// <summary>
/// Health pickup that restores player health when collected
/// </summary>
public class HealthPickup : MonoBehaviour
{
    [Header("Health Pickup Settings")]
    public int healthAmount = 1; // Amount of health to restore
    public GameObject pickupEffect; // Optional visual effect when picked up
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Only heal if player is not at max health
                if (player.GetCurrentHealth() < player.maxHealth)
                {
                    player.AddHealth(healthAmount);
                    
                    // Play pickup effect if available
                    if (pickupEffect != null)
                    {
                        Instantiate(pickupEffect, transform.position, Quaternion.identity);
                    }
                    
                    // Play pickup sound
                    if (AudioManager.instance != null)
                        AudioManager.instance.PlayPickupSound();
                    
                    // Destroy the pickup
                    Destroy(gameObject);
                }
            }
        }
    }
}
