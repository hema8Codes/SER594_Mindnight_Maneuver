using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healthValue = 25; // Amount of health restored
    public delegate void HealthPackPickup();
    public event HealthPackPickup OnPickup; // Event for pickup

    private void OnTriggerEnter(Collider other)
    {
        Transform rootObject = other.transform.root;
        if (rootObject.CompareTag("PlayerCar")) // Check if the car picked it up
        {
            CarContoller car = rootObject.GetComponent<CarContoller>();
            if (car != null)
            {
                car.IncreaseHealth(healthValue); // Increase health
                Debug.Log("Health Pack picked up!");
                OnPickup?.Invoke(); // Notify the spawn point
                Destroy(gameObject); // Destroy the health pack
            }
        }
    }
}
