using UnityEngine;
using UnityEngine.Serialization;

public class CannonShooting : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab to be instantiated
    [SerializeField] private Transform shootPoint; // The point from which the projectile is fired
    [SerializeField] private LineRenderer trajectoryLine; // LineRenderer to render the trajectory curve

    [Header("Shooting Settings")]
    [SerializeField, Range(5.0f, 20.0f)] private float minShootingForce = 5.0f; // Minimum shooting force
    [SerializeField, Range(5.0f, 20.0f)] private float maxShootingForce = 20.0f; // Maximum shooting force
    [SerializeField, Range(1.0f, 10.0f)] private float shootingForceIncrement = 2.0f; // Amount to increase/decrease the force
    [SerializeField] private int maxPoints = 100; // Maximum number of points in the trajectory curve

    [Header("Recoil Animations")]
    [SerializeField] private RecoilAnimation recoilAnimation; // Reference to the RecoilAnimation component
    [SerializeField] private RecoilAnimation barrelRecoilAnimation; // Reference to the RecoilAnimation component

    private float shootingForce = 20.0f; // Initial shooting force
    private bool canAdjustShootingForce = true;
    private bool isIncreasingForce = false;
    private bool isDecreasingForce = false;

    private Vector3 previousPosition;
    private Vector3 previousVelocity;

    private void Start()
    {
        // Configure the LineRenderer settings for the trajectory
        trajectoryLine.positionCount = 0;
        trajectoryLine.startWidth = 0.05f;
        trajectoryLine.endWidth = 0.05f;

        previousPosition = Vector3.zero;
        previousVelocity = Vector3.zero;
    }

    private void Update()
    {
        // Update the trajectory line based on the current shooting direction
        UpdateTrajectoryLine();

        // Handle shooting when the player presses the spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Adjust shooting force using Q and E buttons
        if (canAdjustShootingForce)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isIncreasingForce = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                isDecreasingForce = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                isIncreasingForce = false;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                isDecreasingForce = false;
            }
        }

        if (isIncreasingForce)
        {
            shootingForce = Mathf.Clamp(shootingForce + shootingForceIncrement * Time.deltaTime, minShootingForce, maxShootingForce);
        }
        else if (isDecreasingForce)
        {
            shootingForce = Mathf.Clamp(shootingForce - shootingForceIncrement * Time.deltaTime, minShootingForce, maxShootingForce);
        }
    }

    private void UpdateTrajectoryLine()
    {
        Vector3 currentPosition = shootPoint.position;
        Vector3 currentVelocity = shootPoint.forward * shootingForce * 0.1f;

        // Check if currentPosition or currentVelocity has changed
        bool positionChanged = currentPosition != previousPosition;
        bool velocityChanged = currentVelocity != previousVelocity;

        if (!positionChanged && !velocityChanged)
        {
            return;
        }

        previousPosition = currentPosition;
        previousVelocity = currentVelocity;

        trajectoryLine.positionCount = maxPoints;
        trajectoryLine.SetPosition(0, shootPoint.transform.position);

        float timeStep = Time.fixedDeltaTime; // Adjust this value as needed

        for (int i = 1; i < maxPoints; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                // Calculate the next position using the current velocity
                currentPosition += currentVelocity * timeStep;

                // Calculate the next velocity including the effect of gravity
                currentVelocity += Physics.gravity * 0.05f * timeStep;
            }

            // Set the current position in the trajectory line
            trajectoryLine.SetPosition(i, currentPosition);

            // Exit the loop if the projectile goes below the ground
            if (currentPosition.y < -2.5f)
            {
                trajectoryLine.positionCount = i;
                break;
            }
        }
    }

    private void Shoot()
    {
        // Instantiate a new projectile from the prefab at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        // Call the method to set the direction and power of the shot
        if (projectileComponent != null)
        {
            projectileComponent.SetShotParameters(shootPoint.forward, shootingForce);
        }

        // Destroy the projectile after a certain time (adjust this as needed)
        Destroy(projectile, 10.0f);

        // Apply recoil animations
        recoilAnimation.ApplyRecoil();
        barrelRecoilAnimation.ApplyRecoil();
    }
}
