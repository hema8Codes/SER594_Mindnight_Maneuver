using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopManager : MonoBehaviour
{
    public static CopManager Instance;
    private List<CarNavigatorScript> copCars = new List<CarNavigatorScript>();

    void Awake()
    {
        // Singleton pattern to ensure only one CopManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Register a cop in the manager
    public void RegisterCop(CarNavigatorScript cop)
    {
        if (!copCars.Contains(cop))
        {
            copCars.Add(cop);
            Debug.Log("Cop registered: " + cop.name);
        }
    }

    public void StartChaseForAllCops(Transform playerTransform)
{
       foreach (var cop in copCars)
    {
            cop.StartChase(playerTransform);
    }
    }

    // Optionally, you can also have a method to get all cops or other management features
    public List<CarNavigatorScript> GetAllCops()
    {
        return copCars;
    }
}