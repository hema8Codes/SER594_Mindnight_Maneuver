using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public Transform[] fixedPoints; // Assign pickup and dropoff points in the inspector
    public GameObject customerPrefab; // Customer prefab with the blue ring
    public GameObject destinationRingPrefab; // Destination ring prefab with the red ring

    private Transform currentPickupPoint;
    private Transform currentDropoffPoint;

    public float minDistance = 10f; // Minimum distance threshold for source and destination
    private Transform lastPickupPoint; // To track the last pickup point
    private Transform lastDropoffPoint; // To track the last dropoff point

    private GameObject activeCustomer;
    private GameObject activeDestinationRing;

    void Start()
    {
        AssignPickupAndDropoff();
    }

    public void AssignPickupAndDropoff()
    {
        // Select a random pickup point, avoiding the last pickup point
        currentPickupPoint = GetAlternativePoint(lastPickupPoint);

        // Find a dropoff point far away from the pickup point and avoid last dropoff point
        currentDropoffPoint = GetFarPoint(currentPickupPoint, lastDropoffPoint);

        // Spawn the customer at the pickup point
        activeCustomer = Instantiate(customerPrefab, currentPickupPoint.position, Quaternion.identity);

        // Spawn the destination ring at the dropoff point
        activeDestinationRing = Instantiate(destinationRingPrefab, currentDropoffPoint.position, Quaternion.identity);

        // Update the last used points
        lastPickupPoint = currentPickupPoint;
        lastDropoffPoint = currentDropoffPoint;
    }

    private Transform GetAlternativePoint(Transform excludePoint)
    {
        Transform alternativePoint = null;

        // Filter out the excluded point
        Transform[] availablePoints = System.Array.FindAll(fixedPoints, point => point != excludePoint);

        // Randomly select a point from the filtered list
        if (availablePoints.Length > 0)
        {
            alternativePoint = availablePoints[Random.Range(0, availablePoints.Length)];
        }

        return alternativePoint != null ? alternativePoint : fixedPoints[0]; // Fallback to the first point
    }

    private Transform GetFarPoint(Transform pickupPoint, Transform excludePoint)
    {
        Transform farPoint = null;
        float maxDistance = 0;

        foreach (Transform point in fixedPoints)
        {
            // Skip the pickup point itself and the excluded point
            if (point == pickupPoint || point == excludePoint) continue;

            // Calculate distance from the pickup point
            float distance = Vector3.Distance(pickupPoint.position, point.position);

            // Select the farthest point that meets the minimum distance criteria
            if (distance > minDistance && distance > maxDistance)
            {
                maxDistance = distance;
                farPoint = point;
            }
        }

        // Fallback: If no point meets the criteria, pick the first valid point not excluded
        foreach (Transform point in fixedPoints)
        {
            if (point != pickupPoint && point != excludePoint)
            {
                return point;
            }
        }

        return farPoint != null ? farPoint : fixedPoints[0];
    }

    public void CompleteDropoff()
    {
        // Destroy the current customer and destination ring
        if (activeCustomer != null) Destroy(activeCustomer);
        if (activeDestinationRing != null) Destroy(activeDestinationRing);

        // Assign new pickup and dropoff points
        AssignPickupAndDropoff();
    }
}