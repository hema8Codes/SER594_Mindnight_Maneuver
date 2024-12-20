using UnityEngine;
using System.Collections;

public class NitroSpawnPoint : MonoBehaviour
{
    public GameObject nitroPackPrefab; // Nitro pack prefab
    public float respawnTime = 10f;    // Time to respawn nitro packs

    private GameObject currentNitroPack; // Reference to the spawned nitro pack

    void Start()
    {
        SpawnNitroPack(); // Spawn the first nitro pack
    }

    public void SpawnNitroPack()
    {
        if (currentNitroPack == null) // Only spawn if there isn't an existing nitro pack
        {
        // Set the desired rotation for the nitro pack (90 degrees around the X-axis or adjust as needed)
        Quaternion desiredRotation = Quaternion.Euler(90, 0, 0);

        // Instantiate the nitro pack with the desired rotation
        currentNitroPack = Instantiate(nitroPackPrefab, transform.position, desiredRotation);

        // Attach the OnPickup event if the nitro pack has a script
        NitroPack nitroPackScript = currentNitroPack.GetComponent<NitroPack>();
        if (nitroPackScript != null)
        {
            nitroPackScript.OnPickup += HandleNitroPickup; // Subscribe to the pickup event
        }
        }
    }

    private void HandleNitroPickup()
    {
        StartCoroutine(RespawnNitroPack()); // Trigger respawn after the pack is picked up
    }

    private IEnumerator RespawnNitroPack()
    {
        yield return new WaitForSeconds(respawnTime); // Wait for the respawn time
        SpawnNitroPack(); // Respawn the nitro pack
    }
}