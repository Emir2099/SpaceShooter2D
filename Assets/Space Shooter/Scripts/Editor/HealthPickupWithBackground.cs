using UnityEngine;

/// <summary>
/// Alternative health pickup creator with circular background
/// </summary>
public class HealthPickupWithBackground
{
    [UnityEditor.MenuItem("GameObject/2D Space Shooter/Create Health Pickup (Circular BG)")]
    public static void CreateHealthPickupWithCircularBackground()
    {
        // Create the main GameObject
        GameObject healthPickup = new GameObject("HealthPickup_CircularBG");
        
        // Create circular background
        GameObject circleBackground = new GameObject("CircleBackground");
        circleBackground.transform.SetParent(healthPickup.transform);
        circleBackground.transform.localPosition = Vector3.zero;
        
        // Add circle sprite renderer
        SpriteRenderer circleRenderer = circleBackground.AddComponent<SpriteRenderer>();
        circleRenderer.sortingOrder = 0;
        circleRenderer.color = new Color(0.2f, 0.2f, 0.2f, 0.8f); // Semi-transparent dark background
        // Note: You'll need to create a circle sprite or use Unity's default circle
        
        // Create main heart
        GameObject heartSprite = new GameObject("HeartSprite");
        heartSprite.transform.SetParent(healthPickup.transform);
        heartSprite.transform.localPosition = Vector3.zero;
        heartSprite.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        
        // Add heart sprite renderer
        SpriteRenderer heartRenderer = heartSprite.AddComponent<SpriteRenderer>();
        heartRenderer.sortingOrder = 1;
        heartRenderer.color = Color.red; // Bright red for visibility
        
        // Add glow effect
        HeartGlowEffect glowEffect = heartSprite.AddComponent<HeartGlowEffect>();
        glowEffect.pulseSpeed = 3f;
        glowEffect.glowColor = Color.white;
        glowEffect.glowIntensity = 0.3f;
        
        // Add components to main object
        CircleCollider2D collider = healthPickup.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.6f;
        
        HealthPickup pickup = healthPickup.AddComponent<HealthPickup>();
        pickup.healthAmount = 1;
        
        DirectMoving movement = healthPickup.AddComponent<DirectMoving>();
        
        healthPickup.tag = "Bonus";
        
        UnityEditor.Selection.activeGameObject = healthPickup;
        
        Debug.Log("Health Pickup with circular background created!");
        Debug.Log("1. Assign circle sprite to CircleBackground (or create a simple circle sprite)");
        Debug.Log("2. Assign heart sprite to HeartSprite");
        Debug.Log("3. Set DirectMoving speed to negative value");
        Debug.Log("4. Save as prefab");
    }
}
