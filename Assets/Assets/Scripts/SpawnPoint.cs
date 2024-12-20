using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public GameObject healthPackPrefab; // Prefab of the health pack
    public float respawnTime = 10f;     // Time to respawn the health pack

    private GameObject currentHealthPack;

    void Start()
    {
        SpawnHealthPack(); // Spawn the first health pack
    }

    public void SpawnHealthPack()
    {
        if (currentHealthPack == null) // Only spawn if there isn't an existing health pack
        {
        Quaternion desiredRotation = Quaternion.Euler(90, 0, 0);

        // Instantiate the health pack with the desired rotation
        currentHealthPack = Instantiate(healthPackPrefab, transform.position, desiredRotation);

        // Attach the OnPickup event if the health pack has a script
        HealthPack healthPackScript = currentHealthPack.GetComponent<HealthPack>();
        if (healthPackScript != null)
        {
            healthPackScript.OnPickup += HandleHealthPackPickup; // Listen for pickup events
        }
        }
    }

    private void HandleHealthPackPickup()
    {
        StartCoroutine(RespawnHealthPack());
    }

    private IEnumerator RespawnHealthPack()
    {
        yield return new WaitForSeconds(respawnTime); // Wait for respawn time
        SpawnHealthPack(); // Respawn the health pack
    }
}