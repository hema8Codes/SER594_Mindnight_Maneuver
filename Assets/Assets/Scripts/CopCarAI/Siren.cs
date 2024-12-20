using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Siren : MonoBehaviour
{
    [Header("Siren Lights")]
    [SerializeField] private Light blueSirenLight; // Blue siren light
    [SerializeField] private Light redSirenLight;  // Red siren light

    [Header("Siren Settings")]
    [SerializeField] private float flashSpeed = 10f;       // Speed at which the sirens flash
    [SerializeField] private float lightIntensity = 10f;   // Max intensity of the lights
    [SerializeField] private float minIntensity = 0f;     // Min intensity (off)
    [SerializeField] private float rotationSpeed = 60f;   // Speed at which the sirens rotate

    [Header("Screen Flash Settings")]
    [SerializeField] private Image screenFlashImage;      // UI Image for screen flash
    [SerializeField] private Color blueFlashColor = Color.blue;
    [SerializeField] private Color redFlashColor = Color.red;
    [SerializeField] private float screenFlashAlpha = 0.1f; // Flash transparency

    private bool inPursuit = false; // Tracks whether the cop is in pursuit
    private float timePassed = 0f;

    private void Update()
    {
        if(inPursuit)
        {
            FlashSirenLights();
            RotateSirenLights();
            FlashScreen();
        }
    }

    public void StartSiren()
    {
        // Reset the timer for flashing effects
        //timePassed = 0f;
        inPursuit = true;  // Enable Update calls for flashing, rotating, and screen flash
    }

    public void StopSiren()
    {
        // Stop siren lights
        blueSirenLight.intensity = minIntensity;
        redSirenLight.intensity = minIntensity;
         Debug.Log($"Blue Light Intensity: {blueSirenLight.intensity}, Red Light Intensity: {redSirenLight.intensity}");

        // Stop screen flash
        if (screenFlashImage != null)
        {
            screenFlashImage.color = Color.clear;
        }

        inPursuit = false; // Disable Update calls when not in pursuit

        Debug.Log("Player escaped. Stopping the pursuit!");
    }

    private void FlashSirenLights()
    {
        // Increase the timer for flashing
        timePassed += Time.deltaTime * flashSpeed;

        // Oscillate intensity for the lights
        float intensityFactor = Mathf.Sin(timePassed); // Alternates between -1 and 1

        blueSirenLight.intensity = Mathf.Lerp(minIntensity, lightIntensity, Mathf.Abs(intensityFactor));
        redSirenLight.intensity = Mathf.Lerp(minIntensity, lightIntensity, Mathf.Abs(-intensityFactor));
    }

    private void RotateSirenLights()
    {
        // Rotate both siren lights around the Y-axis
        blueSirenLight.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        redSirenLight.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void FlashScreen()
    {
        if (screenFlashImage != null)
        {
            // Alternate screen flash color between red and blue
            Color currentColor = Mathf.Sin(timePassed) > 0 ? blueFlashColor : redFlashColor;
            currentColor.a = screenFlashAlpha; // Set transparency
            screenFlashImage.color = currentColor;
        }
    }

}
