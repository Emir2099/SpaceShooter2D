using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region Serializable classes
[System.Serializable]

 
public class EnemyWaves
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;
}

#endregion

public class LevelController : MonoBehaviour {

    //Serializable classes implements
    public EnemyWaves[] enemyWaves; 

    public Text completedText;
    
    [Header("Level Progression")]
    [Tooltip("Name of the next level to load after completion")]
    public string nextLevelName = "Level2";
    
    [Tooltip("Time to wait before loading next level (in seconds)")]
    public float timeBeforeNextLevel = 3f;


    [Header("Power-ups")]
    public GameObject powerUp; // Weapon power-up
    public GameObject healthPickup; // Health pickup
    [Range(0f, 1f)]
    public float healthPickupChance = 0.3f; // 30% chance for health pickup, 70% for weapon
    public float timeForNewPowerup;
    
    [Header("Planets")]
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;
    List<GameObject> planetsList = new List<GameObject>();
    
    // Tracking variables for level completion
    private int totalEnemiesSpawned = 0;
    private int enemiesFinished = 0; // Either destroyed or left screen
    private int totalWaves = 0;
    private int wavesSpawned = 0;
    private bool levelCompleted = false;
    private float levelStartTime;
    private float maxLevelDuration = 300f; // 5 minutes max per level

    Camera mainCamera;   

    private void Start()
    {
        mainCamera = Camera.main;
        levelStartTime = Time.time;
        
        // Subscribe to enemy events
        Enemy.OnEnemyDestroyed += OnEnemyFinished;
        Enemy.OnEnemyLeftScreen += OnEnemyFinished;
        
        // Set total number of waves
        totalWaves = enemyWaves.Length;
        
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        for (int i = 0; i<enemyWaves.Length; i++) 
        {
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }
        StartCoroutine(PowerupBonusCreation());
        StartCoroutine(PlanetsCreation());
        StartCoroutine(CheckLevelTimeout());
    }
    
    private void Update()
    {
        // Manual level completion for testing (press L key)
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Manual level completion triggered!");
            CompleteLevel();
        }
        
        // Show debug info every few seconds
        if (Time.time % 5f < 0.1f && !levelCompleted)
        {
            int activeEnemies = FindObjectsOfType<Enemy>().Length;
            Debug.Log($"DEBUG: Active enemies: {activeEnemies}, Finished: {enemiesFinished}/{totalEnemiesSpawned}, Waves: {wavesSpawned}/{totalWaves}");
        }
    }
    
    private void OnDestroy()
    {
        // Clean up subscriptions
        Enemy.OnEnemyDestroyed -= OnEnemyFinished;
        Enemy.OnEnemyLeftScreen -= OnEnemyFinished;
    }
    
    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, GameObject Wave) 
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null && !levelCompleted)
        {
            GameObject waveObj = Instantiate(Wave);
            wavesSpawned++;
            
            // Track enemy count in the wave
            Wave waveScript = waveObj.GetComponent<Wave>();
            if (waveScript != null) {
                // Count enemies in this wave
                totalEnemiesSpawned += waveScript.count;
                Debug.Log($"Wave {wavesSpawned} spawned with {waveScript.count} enemies. Total enemies: {totalEnemiesSpawned}");
            }
        }
    }

    //endless coroutine generating 'levelUp' bonuses and health pickups. 
    IEnumerator PowerupBonusCreation() 
    {
        while (!levelCompleted) 
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            
            // Skip if level is completed
            if (levelCompleted) break;
            
            // Debug: Check if healthPickup is assigned
            if (healthPickup == null)
            {
                Debug.LogWarning("Health Pickup prefab is not assigned in LevelController!");
            }
            
            // Generate random value for debugging
            float randomValue = Random.value;
            Debug.Log($"Random value: {randomValue}, Health pickup chance: {healthPickupChance}");
            
            // Randomly choose between weapon power-up and health pickup
            GameObject pickupToSpawn;
            if (randomValue < healthPickupChance && healthPickup != null)
            {
                pickupToSpawn = healthPickup;
                Debug.Log("Spawning HEALTH pickup!");
            }
            else
            {
                pickupToSpawn = powerUp;
                Debug.Log("Spawning WEAPON pickup!");
            }
            
            // Spawn the chosen pickup
            if (pickupToSpawn != null)
            {
                Instantiate(
                    pickupToSpawn,
                    //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
                    new Vector2(
                        Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX), 
                        mainCamera.ViewportToWorldPoint(Vector2.up).y + pickupToSpawn.GetComponent<Renderer>().bounds.size.y / 2), 
                    Quaternion.identity
                    );
            }
            else
            {
                Debug.LogError("Pickup to spawn is null!");
            }
        }
    }

    IEnumerator PlanetsCreation()
    {
        //Create a new list copying the arrey
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }
        yield return new WaitForSeconds(10);
        while (true)
        {
            ////choose random object from the list, generate and delete it
            int randomIndex = Random.Range(0, planetsList.Count);
            GameObject newPlanet = Instantiate(planetsList[randomIndex]);
            planetsList.RemoveAt(randomIndex);
            //if the list decreased to zero, reinstall it
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }
            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }
    
    // Handle enemy finished event (either destroyed or left screen)
    private void OnEnemyFinished()
    {
        enemiesFinished++;
        
        Debug.Log($"Enemy finished! {enemiesFinished}/{totalEnemiesSpawned} enemies done (destroyed or left screen)");
        Debug.Log($"Waves spawned: {wavesSpawned}/{totalWaves}");
        
        // Check if all enemies are finished and all waves have been spawned
        if (enemiesFinished >= totalEnemiesSpawned && wavesSpawned >= totalWaves && totalEnemiesSpawned > 0)
        {
            Debug.Log("All conditions met for level completion!");
            CompleteLevel();
        }
        else
        {
            Debug.Log($"Level not complete yet. Missing: {totalEnemiesSpawned - enemiesFinished} enemies, {totalWaves - wavesSpawned} waves");
        }
    }
    
    // Complete the level when all enemies are destroyed
    private void CompleteLevel()
    {
        if (levelCompleted) return; // Only run once
        
        levelCompleted = true;
        Debug.Log("LEVEL COMPLETED! All enemies finished (destroyed or left screen).");
        
        // Stop player shooting
        if (PlayerShooting.instance != null)
        {
            PlayerShooting.instance.DisableShooting();
            Debug.Log("Player shooting disabled");
        }
        
        // Show completion text
        if (completedText != null)
        {
            completedText.gameObject.SetActive(true);
            Debug.Log("Level complete text shown");
        }
        else
        {
            Debug.LogWarning("CompletedText is not assigned in LevelController!");
        }
        
        Debug.Log("Pickup spawning stopped (PowerupBonusCreation coroutine will exit)");
        
        // Start coroutine to load next level after delay
        StartCoroutine(LoadNextLevelAfterDelay());
    }
    
    // Check for level timeout to prevent infinite gameplay
    IEnumerator CheckLevelTimeout()
    {
        while (!levelCompleted)
        {
            yield return new WaitForSeconds(10f); // Check every 10 seconds
            
            // If it's been too long and all waves are spawned, force completion
            if (Time.time - levelStartTime > maxLevelDuration && wavesSpawned >= totalWaves)
            {
                Debug.LogWarning("Level timeout reached! Forcing completion.");
                CompleteLevel();
                break;
            }
            
            // Alternative completion check: if all waves spawned and reasonable time has passed
            if (wavesSpawned >= totalWaves && Time.time - levelStartTime > 60f)
            {
                int activeEnemies = FindObjectsOfType<Enemy>().Length;
                Debug.Log($"Timeout check: {activeEnemies} active enemies remaining after 60 seconds");
                
                if (activeEnemies == 0)
                {
                    Debug.Log("No active enemies found, completing level");
                    CompleteLevel();
                    break;
                }
            }
        }
    }
    
    // Coroutine to load the next level after a delay
    private IEnumerator LoadNextLevelAfterDelay()
    {
        Debug.Log($"Waiting {timeBeforeNextLevel} seconds before loading next level: {nextLevelName}");
        
        // Wait for the specified time
        yield return new WaitForSeconds(timeBeforeNextLevel);
        
        // Check if the next level exists
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Debug.Log($"Loading next level: {nextLevelName}");
            
            try
            {
                SceneManager.LoadScene(nextLevelName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load level '{nextLevelName}': {e.Message}");
                Debug.LogError("Make sure the scene is added to Build Settings!");
                
                // Fallback: Try to load main menu
                try
                {
                    SceneManager.LoadScene("MainMenu");
                    Debug.Log("Loaded MainMenu as fallback");
                }
                catch
                {
                    Debug.LogError("Could not load MainMenu either. Check your scene names in Build Settings.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Next level name is not set! Cannot load next level.");
        }
    }
}
