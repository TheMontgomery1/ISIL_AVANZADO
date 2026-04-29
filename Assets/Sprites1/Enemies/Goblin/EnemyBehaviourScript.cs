using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Player transform reference
    public Transform player;

    // Projectile prefab
    public GameObject projectilePrefab;

    // Shooting rate (shots per second)
    public float fireRate = 1f;

    // Time until next shot
    private float nextFireTime = 0f;

    // Shooting range (optional)
    public float shootingRange = 5f;

    void Update()
    {
        // Check if the player is within range
        if (Vector2.Distance(transform.position, player.position) <= shootingRange)
        {
            // Check if it's time to shoot
            if (Time.time > nextFireTime)
            {
                // Shoot the projectile
                Shoot();

                // Set the next fire time
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    // Shoot the projectile
    void Shoot()
    {
        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Get the direction to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Set the projectile's velocity
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 5f; // Adjust speed as needed
    }
}