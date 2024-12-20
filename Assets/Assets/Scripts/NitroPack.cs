using UnityEngine;

public class NitroPack : MonoBehaviour
{
    public float nitroRefillAmount = 0.25f; // Amount to refill the nitro bar (e.g., 25%)

    // Event to notify when the nitro pack is picked up
    public delegate void NitroPackPickup();
    public event NitroPackPickup OnPickup;

    private void OnTriggerEnter(Collider other)
    {
        Transform rootObject = other.transform.root;
        if (rootObject.CompareTag("PlayerCar")) // Check if the car picked it up
        {
            CarContoller car = rootObject.GetComponent<CarContoller>();
            if (car != null)
            {
                car.IncreaseNitro(nitroRefillAmount); // Refill the nitro bar
                Debug.Log("Nitro Pack collected!");
                
                // Notify listeners that the nitro pack was picked up
                OnPickup?.Invoke();

                // Destroy the nitro pack
                Destroy(gameObject);
            }
        }
    }
}
