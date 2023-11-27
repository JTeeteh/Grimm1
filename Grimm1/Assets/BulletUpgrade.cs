using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUpgrade : MonoBehaviour
{
    public GameObject upgradedBulletPrefab;
    public float upgradeDuration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the other collider is the player's collider
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                // Apply the bullet upgrade to the player
                playerMovement.ApplyBuff(upgradedBulletPrefab, upgradeDuration);

                // Optionally, play a sound effect, show visual effects, etc.

                // Destroy the upgrade object
                Destroy(gameObject);
            }
        }
    }
}
