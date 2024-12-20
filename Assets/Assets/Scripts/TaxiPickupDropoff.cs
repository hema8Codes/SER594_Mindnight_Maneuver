using System.Collections;
using UnityEngine;

public class TaxiPickupDropoff : MonoBehaviour
{
    private bool hasCustomer = false;

    public GameObject customer; // Assign dynamically during runtime
    public GameObject destinationRing; // Assign dynamically during runtime
    public GameObject magicRing;
    public GameManager gameManager; // Assign the GameManager in the Inspector
    public int minCashPerTrip = 50; // Minimum cash reward
    public int maxCashPerTrip = 100; // Maximum cash reward
    public CustomerManager customerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupZone") && !hasCustomer)
        {
            Debug.Log("Customer Picked Up!");
            hasCustomer = true;

                    // Get the parent object (CustomerPrefab)
            customer = other.transform.parent.gameObject;

                        // Get the parent object (customer) and child (magic_ring_01)
            customer = other.transform.parent.gameObject; // Parent object
            magicRing = other.gameObject; // The pickup ring itself (child)

            // Hide the customer immediately (excluding the ring)
            HideCustomerModel();

            // Start a coroutine to hide the pickup ring after 5 seconds
            StartCoroutine(HideRingAfterDelay(magicRing, 5f));

        }
        else if (other.CompareTag("DropoffZone") && hasCustomer)
        {
            Debug.Log("Customer Dropped Off!");
            hasCustomer = false;

            // Move customer to the dropoff ring center
            if (destinationRing != null)
            {
                customer.transform.position = destinationRing.transform.position;
                customer.SetActive(true);
            }

            // Calculate a random cash reward
            int cashReward = Random.Range(minCashPerTrip, maxCashPerTrip);

            // Notify the GameManager to add cash
            if (gameManager != null)
            {
                gameManager.AddCash(cashReward);
            }

            // Additional logic for resetting the customer can go here

            // Notify the manager to spawn new pickup and dropoff points
            customerManager.CompleteDropoff();
        }
    }


        private void HideCustomerModel()
    {
        // Hide all child objects of the customer except the magic ring
        foreach (Transform child in customer.transform)
        {
            if (child.gameObject != magicRing)
            {
                child.gameObject.SetActive(false);
            }
        }
        Debug.Log("Customer model hidden, magic ring remains visible.");
    }

    private IEnumerator HideRingAfterDelay(GameObject ring, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Hide the ring after the delay
        if (ring != null)
        {
            ring.SetActive(false);
            Debug.Log("Pickup ring has been hidden.");
        }
    }
}

