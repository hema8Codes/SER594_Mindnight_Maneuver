using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarContoller : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public UIHealthAlchemy.PowerBarMaterial healthBar; 
    public float maxHealth = 100f;                 
    private float currentHealth;                  

    public AudioSource crashSound;                    

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    public float nitroBoostMultiplier = 2.0f;        
    public float nitroDuration = 20.0f;        
    public UnityEngine.UI.Image nitroBarFill;    

    public List<ParticleSystem> nitroEffects;         

    private float moveInput;
    private float steerInput;
    private Rigidbody carRb;
    private bool isNitroActive = false;
    private float nitroValue = 1f;  

    private bool canTakeDamage = true;               
    public float damageCooldown = 1.0f;   

    public GameManager gameManager; // Reference to the GameManager
    private bool isGameOver = false; // Prevent multiple Game Over triggers        

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        // Initialize nitro bar
        if (nitroBarFill != null)
        {
            nitroBarFill.fillAmount = nitroValue;
        }

        // Initialize health
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }


    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Cap health to max
        Debug.Log("Health increased. Current health: " + currentHealth);
        Debug.Log($"Health increased by {amount}. Current health: {currentHealth}");
        UpdateHealthBar(); 
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        WheelEffects();


        if (Input.GetKey(KeyCode.N) && !isNitroActive && nitroValue > 0f)
        {
            StartCoroutine(ActivateNitro());
        }

        if (nitroBarFill != null)
        {
            nitroBarFill.fillAmount = nitroValue;
        }
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 5.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    public IEnumerator ActivateNitro()
    {
        isNitroActive = true;

        maxAcceleration *= nitroBoostMultiplier;

        foreach (var effect in nitroEffects)
        {
            if (effect != null)
                effect.Play();
        }
        while (Input.GetKey(KeyCode.N) && nitroValue > 0)
        {
            nitroValue -= Time.deltaTime / nitroDuration;  
            nitroBarFill.fillAmount = nitroValue;         
            yield return null;
        }

        maxAcceleration /= nitroBoostMultiplier;

        foreach (var effect in nitroEffects)
        {
            if (effect != null)
                effect.Stop();
        }

        isNitroActive = false;
    }

    public void IncreaseNitro(float amount)
{
    nitroValue = Mathf.Clamp(nitroValue + amount, 0f, 1f); // Clamp to ensure it doesn't exceed 1
    if (nitroBarFill != null)
    {
        nitroBarFill.fillAmount = nitroValue; // Update the UI
    }
    Debug.Log($"Nitro increased by {amount * 100}%. Current nitro: {nitroValue * 100}%");
}


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle") && canTakeDamage)
        {
            TakeDamage(10f); 

        }
        
    }

    public void TakeDamage(float damage)
    {
        if (isGameOver) return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
    if (isGameOver) return;

    isGameOver = true; // Prevent multiple triggers
    Debug.Log("Game Over Triggered!");

    if (gameManager != null)
    {
        gameManager.ShowGameOverScreen();
    }
    else
    {
        Debug.LogError("GameManager not assigned!");
    }

    enabled = false; // Disable further car control
    }


    void UpdateHealthBar()
    {
    if (healthBar != null)
    {
        float normalizedHealth = currentHealth / maxHealth;

        healthBar.Value = Mathf.Clamp(normalizedHealth, 0f, 1f);

    }
    }

}
