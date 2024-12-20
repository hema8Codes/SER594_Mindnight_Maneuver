using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject healthPackPrefab; // Prefab for health packs
    public Transform[] spawnPoints;    // Array of spawn point locations
    public float respawnTime = 10f;    // Respawn time for all spawn points

    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnPoint sp = spawnPoint.gameObject.AddComponent<SpawnPoint>();
            sp.healthPackPrefab = healthPackPrefab; // Assign the prefab
            sp.respawnTime = respawnTime;           // Assign the respawn time
        }
    }
}
