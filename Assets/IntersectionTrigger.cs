using UnityEngine;
using System.Collections.Generic;

public class IntersectionTrigger : MonoBehaviour
{
    public TrafficLightController trafficLightController;                        
    public List<CarNavigatorScript> copCars;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player Entered");
        Debug.Log($"Intersection Trigger other: {other.gameObject.name}");
        if (other.CompareTag("PlayerCar"))
        {
            Debug.Log($"Player confirmed");
            Transform parentTransform = other.transform.root;
            Debug.Log($"Intersection Trigger other: {parentTransform.gameObject.tag}");

            if (parentTransform.CompareTag("PlayerCar"))
            {
                Debug.Log("Player confirmed via TaxiBody!");
                 int lightState = trafficLightController.GetCurrentState();

                // Notify all cop cars
                foreach (var copCar in copCars)
                {
                    if (copCar != null)
                    {
                        copCar.CheckTrafficLightViolation(lightState, parentTransform);
                    }
                    else
                    {
                        Debug.LogWarning("A cop car reference is missing in the copCars list.");
                    }
                }
            }
        }
    }

}