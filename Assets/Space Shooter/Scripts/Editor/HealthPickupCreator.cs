using UnityEngine;

/// <summary>
/// Helper script to quickly set up a health pickup prefab
/// Right-click in Project → Create → Health Pickup Prefab
/// </summary>
public class HealthPickupCreator
{
    [UnityEditor.MenuItem("GameObject/2D Space Shooter/Create Health Pickup")]
    public static void CreateHealthPickupPrefab()
    {
        // Create the main GameObject
        GameObject healthPickup = new GameObject("HealthPickup");
        
        // Add SpriteRenderer (you'll need to assign the heart sprite manually)
        SpriteRenderer spriteRenderer = healthPickup.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        
        // Add Collider2D as trigger
        CircleCollider2D collider = healthPickup.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
        
        // Add HealthPickup script
        HealthPickup pickup = healthPickup.AddComponent<HealthPickup>();
        pickup.healthAmount = 1;
        
        // Add DirectMoving script for falling motion
        DirectMoving movement = healthPickup.AddComponent<DirectMoving>();
        // You'll need to set the speed in the inspector (try -2 or -3 for downward movement)
        
        // Add tag
        healthPickup.tag = "Bonus";
        
        // Select the created object
        UnityEditor.Selection.activeGameObject = healthPickup;
        
        Debug.Log("Health Pickup created! Don't forget to:");
        Debug.Log("1. Assign a heart sprite to the SpriteRenderer");
        Debug.Log("2. Set DirectMoving speed to negative value (e.g., -2)");
        Debug.Log("3. Save as prefab in your project");
    }
}
