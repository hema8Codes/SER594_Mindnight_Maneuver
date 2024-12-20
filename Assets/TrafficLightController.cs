using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float redDuration = 10f;
    public float greenDuration = 10f;
    public float yellowDuration = 6f;

    public AudioSource transitionSound;

    private float timer;
    private int state;

    public delegate void TrafficLightChange(int state);
    public static event TrafficLightChange OnTrafficLightChange;

    void Start()
    {
        timer = 0f;
        state = 0; // Start with Red Light
        UpdateLights();
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case 0: // Red Light
                if (timer > redDuration)
                {
                    state = 1; // Green Light
                    timer = 0f;
                    UpdateLights();
                }
                break;

            case 1: // Green Light
                if (timer > greenDuration)
                {
                    state = 2; // Yellow Light
                    timer = 0f;
                    UpdateLights();
                }
                break;

            case 2: // Yellow Light
                if (timer > yellowDuration)
                {
                    state = 0; // Red Light
                    timer = 0f;
                    UpdateLights();
                }
                break;
        }
    }

    void UpdateLights()
    {
        redLight.SetActive(state == 0);
        yellowLight.SetActive(state == 2);
        greenLight.SetActive(state == 1);

        if (transitionSound != null)
            transitionSound.Play();

        Debug.Log($"Traffic Light State Changed: {state}");

        OnTrafficLightChange?.Invoke(state); // Notify listeners of state change
    }

    public int GetCurrentState()
    {
        return state; // 0 = Red, 1 = Green, 2 = Yellow
    }
}