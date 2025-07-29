using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Enemy : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;
    
    [Header("Pickup Drops")]
    [Range(0f, 1f)]
    [Tooltip("Chance to drop health pickup when destroyed (0 = never, 1 = always)")]
    public float healthPickupDropChance = 0.15f; // 15% chance
    public GameObject healthPickupPrefab;

    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path
    #endregion

    private void Start()
    {
        Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
    }

    //coroutine making a shot
    void ActivateShooting()
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {
            Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
        }
    }

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage)
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        
        // Play hit sound
        if (AudioManager.instance != null)
            AudioManager.instance.PlayHitSound();
            
        if (health <= 0)
        {
            Destruction();
            ScoreIncrease();
        }
        else
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
    }

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
            else
                Player.instance.GetDamage(1);
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()
    {
        // Try to drop health pickup
        if (healthPickupPrefab != null && Random.value < healthPickupDropChance)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
        
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void ScoreIncrease()
    {
        Player.IncreaseScore(1); // Use the global score system
    }
    
}
