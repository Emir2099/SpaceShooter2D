using UnityEngine;

/// <summary>
/// Makes a heart pickup more visible by adding pulsing glow effect
/// </summary>
public class HeartGlowEffect : MonoBehaviour
{
    [Header("Glow Settings")]
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public Color glowColor = Color.white;
    public float glowIntensity = 0.5f;
    
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private Color originalColor;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;
    }
    
    void Update()
    {
        // Pulsing scale effect
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f; // 0 to 1
        float currentScale = Mathf.Lerp(minScale, maxScale, pulse);
        transform.localScale = originalScale * currentScale;
        
        // Glow color effect
        Color currentColor = Color.Lerp(originalColor, glowColor, pulse * glowIntensity);
        spriteRenderer.color = currentColor;
    }
}
