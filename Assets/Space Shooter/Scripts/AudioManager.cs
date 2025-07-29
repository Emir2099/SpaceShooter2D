using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip laserSound;
    public AudioClip pickupSound;
    public AudioClip hitSound;
    
    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float laserVolume = 0.6f;
    [Range(0f, 1f)]
    public float pickupVolume = 0.8f;
    [Range(0f, 1f)]
    public float hitVolume = 0.7f;
    
    // Singleton pattern
    public static AudioManager instance;
    
    // Audio sources for different sound types
    private AudioSource laserAudioSource;
    private AudioSource pickupAudioSource;
    private AudioSource hitAudioSource;
    
    private void Awake()
    {
        // Singleton setup
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
            
        // Setup audio sources
        SetupAudioSources();
    }
    
    private void Start()
    {
        // Try to load audio clips if not assigned in inspector
        TryLoadAudioClips();
    }
    
    private void SetupAudioSources()
    {
        // Create separate audio sources for different sound types
        laserAudioSource = gameObject.AddComponent<AudioSource>();
        laserAudioSource.playOnAwake = false;
        laserAudioSource.volume = laserVolume;
        
        pickupAudioSource = gameObject.AddComponent<AudioSource>();
        pickupAudioSource.playOnAwake = false;
        pickupAudioSource.volume = pickupVolume;
        
        hitAudioSource = gameObject.AddComponent<AudioSource>();
        hitAudioSource.playOnAwake = false;
        hitAudioSource.volume = hitVolume;
    }
    
    private void TryLoadAudioClips()
    {
        // Try to load audio clips from Resources if not assigned in inspector
        if (laserSound == null)
            laserSound = Resources.Load<AudioClip>("Audio/laser_shoot");
            
        if (pickupSound == null)
            pickupSound = Resources.Load<AudioClip>("Audio/pickup");
            
        if (hitSound == null)
            hitSound = Resources.Load<AudioClip>("Audio/hit");
    }
    
    // Play laser sound - prevents overlapping by controlling the timing
    public void PlayLaserSound()
    {
        if (laserSound == null) return;
        
        // Play without overlapping itself
        if (!laserAudioSource.isPlaying)
        {
            laserAudioSource.clip = laserSound;
            laserAudioSource.Play();
        }
    }
    
    // Play pickup sound
    public void PlayPickupSound()
    {
        if (pickupSound == null) return;
        
        pickupAudioSource.clip = pickupSound;
        pickupAudioSource.Play();
    }
    
    // Play hit sound
    public void PlayHitSound()
    {
        if (hitSound == null) return;
        
        hitAudioSource.clip = hitSound;
        hitAudioSource.Play();
    }
}
