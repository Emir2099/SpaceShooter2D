using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Camera mainCamera;   

    private void Start()
    {
        mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        for (int i = 0; i<enemyWaves.Length; i++) 
        {
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }
        StartCoroutine(PowerupBonusCreation());
        StartCoroutine(PlanetsCreation());
    }
    
    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, GameObject Wave) 
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null)
            Instantiate(Wave);
    }

    //endless coroutine generating 'levelUp' bonuses and health pickups. 
    IEnumerator PowerupBonusCreation() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            
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
}
