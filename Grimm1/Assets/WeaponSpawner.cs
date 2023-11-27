using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WeaponSpawner : MonoBehaviourPun
{
    public GameObject weaponPrefab;
    public float spawnInterval = 2.0f;

    private GameObject currentWeapon;  // Reference to the currently spawned weapon

    void Start()
    {
        // Start the coroutine to spawn the weapon at intervals
        StartCoroutine(SpawnWeaponWithInterval());
    }

    IEnumerator SpawnWeaponWithInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Check if there is no current weapon before spawning a new one
            if (currentWeapon == null)
            {
                // Spawn the weaponPrefab using PhotonNetwork.Instantiate
                currentWeapon = PhotonNetwork.Instantiate(weaponPrefab.name, transform.position, Quaternion.identity);
                Debug.Log("Weapon spawned!");
            }
            else
            {
                Debug.Log("Waiting for the current weapon to be destroyed...");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset the reference to the current weapon
            currentWeapon = null;
            Debug.Log("Player entered the trigger zone. Resetting the current weapon reference.");
        }
    }
}
