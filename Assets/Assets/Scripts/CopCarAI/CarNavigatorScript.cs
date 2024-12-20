using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNavigatorScript : MonoBehaviour
{
    [Header("Car Info")]
    public float movingSpeed;
    public float turningSpeed;
    public float stopSpeed = 1f;
    public GameObject sensor;
    public float detectionRange = 20f;

    [Header("Destination Car")]
    public Vector3 destination;
    public bool destinationReached;

    private bool obstacleDetected = false;

    [Header("Chase Settings")]
    public float playerDetectionRadius = 20f; // Radius to find the player
    public float chaseSpeed = 12f; // Speed when chasing the player
    private float NavMovingSpeed;
    private bool isChasing = false;
    private bool isCooldownActive = false;
    private Transform targetPlayer;
    public Transform playerToChase;

    private Siren copSiren; 

    public void StartChase(Transform playerTransform)
    {
        if (isChasing || isCooldownActive) return;
        playerToChase = playerTransform;
        Debug.Log("Chasing the player!");
        //CopManager.Instance.StartChaseForAllCops(playerTransform);
    }

    public void StopChase()
    {
        NavMovingSpeed = 0;
        isChasing = false;
        playerToChase = null;
        

        // Stop siren and screen flash when not in pursuit
        if (copSiren != null)
        {
            copSiren.StopSiren();
        }

        Debug.Log("Player escaped. Returning to patrolling.");
        StartCoroutine(RestoreMovingSpeedAfterDelay(5f));
        StartCoroutine(CooldownBeforeDetection(5f));
        //destination = transform.position; 
    }

    private IEnumerator CooldownBeforeDetection(float cooldownTime)
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldownActive = false;
        Debug.Log("Cooldown ended. Cops can now detect the player.");
    }

    private IEnumerator RestoreMovingSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        NavMovingSpeed = 8f; // Restore the patrol speed;
        Debug.Log("Cop has resumed patrolling.");
        LocateDestination(destination);
    }

    void Start()
    {
        // Register this cop with the CopManager
        CopManager.Instance.RegisterCop(this);
        copSiren = GetComponentInChildren<Siren>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerCar");
        if (playerObject != null)
        {
            targetPlayer = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player car not found. Ensure it is tagged as 'PlayerCar' in the scene.");
        }
    }

    void Update()
    {
        // Raycast to detect obstacles
        RaycastHit hitInfo;
        if (Physics.Raycast(sensor.transform.position, sensor.transform.forward, out hitInfo, detectionRange))
        {
            if (hitInfo.rigidbody != null) // Check if the hit object has a Rigidbody
            {
                obstacleDetected = true;
            }
            else
            {
                obstacleDetected = false;
            }
        }
        else
        {
            obstacleDetected = false; // Reset when no obstacle is detected
        }

        DetectTrafficLightViolation();

        if (!isChasing)
        {
            Drive(); // Regular patrolling behavior
            return;
        }
        else
        {
            ChasePlayer(); // Chasing behavior
            copSiren.StartSiren();
        }
    }

    public void Drive()
    {
        if (obstacleDetected)
        {
            NavMovingSpeed = 0f;
            return;
        }

        NavMovingSpeed = 8f;
        
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;
            
            if (destinationDistance >= stopSpeed)
            {
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * NavMovingSpeed * Time.deltaTime);
            }
            else
            {
                destinationReached = true;
            }
        }
    }



    private void ChasePlayer()
    {
       if (!isChasing || playerToChase == null) return; 

        Vector3 direction = (playerToChase.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerToChase.position);
        float targetSpeed = chaseSpeed;

         if (distanceToPlayer < 20f) // Slow down as the cop gets closer
        {
            targetSpeed = Mathf.Lerp(0, chaseSpeed, distanceToPlayer / 15f);
        }

        // Check if the cop car is too close to the player
        if (distanceToPlayer > 8f) // Maintain at least 2 units distance
        {
            // Move towards the player
            transform.position += direction * targetSpeed * Time.deltaTime;

            // Rotate towards the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
        }
        // else
        // {
        //     // Apply force to the player to simulate collision impact
        //     Rigidbody playerRb = playerToChase.GetComponent<Rigidbody>();
        //     if (playerRb != null)
        //     {
        //         Vector3 pushDirection = (playerToChase.position - transform.position).normalized;
        //         playerRb.AddForce(pushDirection * chaseSpeed, ForceMode.Impulse);
        //     }
        // }
        if (distanceToPlayer > playerDetectionRadius * 1.5f) // Stop chasing if the player escapes
        {
            StopChase();
        }
    }

     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerCar") && !isCooldownActive) // Check if it's the player's car
        {
            CarContoller playerCar = collision.gameObject.GetComponent<CarContoller>();
            if (playerCar != null)
            {
                if (!isChasing)
                {
                    StartChase(playerCar.transform); 
                    isChasing = true;
                    copSiren.StartSiren();

                    Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                    Rigidbody copRb = GetComponent<Rigidbody>();

                    // if (playerRb != null && copRb != null)
                    // {
                    //     Vector3 collisionDirection = (collision.transform.position - transform.position).normalized;
                    //     playerRb.AddForce(collisionDirection * chaseSpeed, ForceMode.Impulse);
                    //     copRb.AddForce(-collisionDirection * chaseSpeed, ForceMode.Impulse);
                    // }
                }
                
            }
        }
    }

    public bool IsPlayerInDetectionRange(Transform playerTransform)
    {
        if (isCooldownActive) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
    if (distanceToPlayer <= detectionRange)
    {
        RaycastHit hitInfo;
        Vector3 directionToPlayer = (playerTransform.position - sensor.transform.position).normalized;

        if (Physics.Raycast(sensor.transform.position, directionToPlayer, out hitInfo, detectionRange))
        {
            if (hitInfo.transform == playerTransform)
            {
                Debug.Log("Player detected in detection range.");
                return true;
            }
            else
            {
                Debug.Log("Line of sight to player blocked by: " + hitInfo.transform.name);
            }
        }
    }
    else
    {
        Debug.Log("Player is out of detection range.");
    }

    return false;
    }


    private void DetectTrafficLightViolation()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        foreach (Collider obj in nearbyObjects)
        {
            if (obj.CompareTag("TrafficLight"))
            {
                TrafficLightController trafficLight = obj.GetComponent<TrafficLightController>();
                if (trafficLight != null && trafficLight.GetCurrentState() == 0) // Check for red light
                {
                    GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerCar");
                    if (playerObject != null)
                    {
                        Transform playerTransform = playerObject.transform;
                        Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();
                        if (playerRb != null && playerRb.velocity.magnitude > 1f)
                        {
                            Debug.Log("Player jumped the light! Starting chase.");
                            StartChase(playerTransform);
                            break;
                        }
                    }
                }
            }
        }
    }


    public void CheckTrafficLightViolation(int lightState, Transform playerTransform)
    {
        if (lightState == 0 && IsPlayerInDetectionRange(playerTransform))
        {
            Debug.Log("Traffic violation detected! Starting chase.");
            isChasing = true;
            StartChase(playerTransform);
        }
    }

    

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }
}
