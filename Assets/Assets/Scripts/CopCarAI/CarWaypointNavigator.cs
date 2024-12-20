using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
    [Header("Car AI")]
    public CarNavigatorScript car;
    public Waypoint currWaypoint;

    private void Awake(){
        car = GetComponent<CarNavigatorScript>();
    }

    private void Start(){
        car.LocateDestination(currWaypoint.GetPosition());
    }

    // Update is called once per frame
    private void Update()
    {
        if (car.destinationReached)
            {
        
                currWaypoint = currWaypoint.nextWaypoint;
                car.LocateDestination(currWaypoint.GetPosition());
           }
    }
}
