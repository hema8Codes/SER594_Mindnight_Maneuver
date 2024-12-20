using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceEvasionPowerup : MonoBehaviour
{
    public GameObject powerupEffect; // Optional: Visual effect for when the powerup is collected

    private void OnTriggerEnter(Collider other)
    {
        Transform rootObject = other.transform.root;
        if (rootObject.CompareTag("PlayerCar")) // Check if the player collected the power-up
        {
            // Find all CarNavigatorScript instances in the scene
            CarNavigatorScript[] copScripts = FindObjectsOfType<CarNavigatorScript>();

            if (copScripts.Length > 0)
            {
                Debug.Log($"Found {copScripts.Length} cops. Stopping all chases.");

                // Stop the chase for each cop
                foreach (CarNavigatorScript cop in copScripts)
                {
                    cop.StopChase();
                }
            }
            else
            {
                Debug.Log("No cops found in the scene.");
            }

            // Instantiate visual effect (if any)
            if (powerupEffect != null)
            {
                Instantiate(powerupEffect, transform.position, Quaternion.identity);
            }

            // Destroy the powerup object
            Destroy(gameObject);
        }
    }
}
