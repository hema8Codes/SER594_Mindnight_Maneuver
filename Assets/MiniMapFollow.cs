using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [Header("MiniMap Settings")]
    public Vector3 cityCenterPosition = new Vector3(0, 50, 0); // Adjust to the center of your city
    public bool rotateWithPlayer = false; // Set to false to keep the camera from rotating with the player

    [Header("Zoom Settings")]
    public float fixedZoom = 50f; // Set your desired fixed zoom level

    private Camera miniMapCamera; // Reference to the MiniMap camera component

    void Start()
    {
        // Get the Camera component
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            Debug.LogError("MiniMapFollow: No Camera component found on this GameObject.");
        }

        // Set the camera position to be fixed above the city
        transform.position = cityCenterPosition;

        // Keep the rotation fixed for a top-down view (90 degrees on the X-axis)
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Set the orthographic size to the fixed zoom value
        miniMapCamera.orthographicSize = fixedZoom;

        // Set the near and far clipping planes if necessary
        miniMapCamera.nearClipPlane = 0.3f;
        miniMapCamera.farClipPlane = 1000f;
    }

    void LateUpdate()
    {
        if (miniMapCamera == null) return;

        // Since we don't need zoom, remove the HandleZoom method
        // No zooming logic is needed here anymore
    }
}