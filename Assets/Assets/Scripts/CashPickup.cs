using UnityEngine;

public class CashPickup : MonoBehaviour
{
    public int cashAmount = 20; // Amount of cash this pickup adds

    private void OnTriggerEnter(Collider other)
    {
        Transform rootObject = other.transform.root;
        if (rootObject.CompareTag("PlayerCar")) // Ensure the car has the tag "PlayerCar"
        {
            Debug.Log("Cash Pickup Collected!");

            // Notify the GameManager to add cash
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddCash(cashAmount);
            }

            // Destroy the pickup object
            Destroy(gameObject);
        }
    }
}
