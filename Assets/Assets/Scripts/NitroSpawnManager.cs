using UnityEngine;

public class NitroSpawnManager : MonoBehaviour
{
    public GameObject nitroPackPrefab; // Prefab for nitro packs
    public Transform[] spawnPoints;    // Array of spawn point locations
    public float respawnTime = 10f;    // Respawn time for nitro packs

    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            NitroSpawnPoint sp = spawnPoint.gameObject.AddComponent<NitroSpawnPoint>();
            sp.nitroPackPrefab = nitroPackPrefab; // Assign the prefab
            sp.respawnTime = respawnTime;         // Assign the respawn time
        }
    }
}
