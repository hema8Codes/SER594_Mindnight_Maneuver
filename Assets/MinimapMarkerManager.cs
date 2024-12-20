using UnityEngine;
using UnityEngine.UI;

public class MinimapMarkerManager : MonoBehaviour
{
    [Header("Minimap Settings")]
    public RectTransform minimapImage; // Assign the RectTransform of the minimap Raw Image
    public Camera minimapCamera;      // Assign your minimap camera

    [Header("Marker Prefabs")]
    public GameObject PickupPoint;  // Prefab for pickup point marker
    public GameObject DropoffPoint; // Prefab for drop-off point marker

    void Start()
    {
        // Add markers for all pickup points
        GameObject[] pickupPoints = GameObject.FindGameObjectsWithTag("PickupPoints");
        foreach (var pickup in pickupPoints)
        {
            AddMarker(pickup.transform.position, PickupPoint);
        }

        // Add markers for all drop-off points
        GameObject[] dropoffPoints = GameObject.FindGameObjectsWithTag("DropoffPoints");
        foreach (var dropoff in dropoffPoints)
        {
            AddMarker(dropoff.transform.position, DropoffPoint);
        }
    }

    void AddMarker(Vector3 worldPosition, GameObject markerPrefab)
    {
        // Convert the world position to minimap position
        Vector2 minimapPosition = WorldToMinimapPosition(worldPosition);

        // Instantiate the marker as a child of the minimap
        GameObject marker = Instantiate(markerPrefab, minimapImage);

        // Set the position of the marker on the minimap
        RectTransform rectTransform = marker.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = minimapPosition;
        }
        else
        {
            Debug.LogError("Marker prefab is missing a RectTransform component.");
        }
    }

    Vector2 WorldToMinimapPosition(Vector3 worldPosition)
    {
        // Convert the world position to viewport position (0 to 1 range)
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(worldPosition);

        // Adjust the position based on the camera offset
        // Ensure that the min/max bounds on the minimap map are respected
        Vector2 minimapSize = minimapImage.rect.size;
        Vector2 minimapPos = new Vector2(
            (viewportPos.x * minimapSize.x) - minimapSize.x * 0.5f,  // Center horizontally
            (viewportPos.y * minimapSize.y) - minimapSize.y * 0.5f   // Center vertically
        );

        return minimapPos;
    }
}