using UnityEngine;

public class CannonShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab to be instantiated
    public Transform shootPoint; // The point from which the projectile is fired
    public LineRenderer trajectoryLine; // LineRenderer to render the trajectory curve

    public float minShootingForce = 5.0f; // Minimum shooting force
    public float maxShootingForce = 20.0f; // Maximum shooting force
    public float shootingForceIncrement = 2.0f; // Amount to increase/decrease the force
    public int maxPoints = 100; // Maximum number of points in the trajectory curve

    private float shootingForce = 10.0f; // Initial shooting force
    private bool canAdjustShootingForce = true;
    private bool isIncreasingForce = false;
    private bool isDecreasingForce = false;

    private void Start()
    {
        // Configure the LineRenderer settings for the trajectory
        trajectoryLine.positionCount = 0;
        trajectoryLine.startWidth = 0.05f;
        trajectoryLine.endWidth = 0.05f;
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
        trajectoryLine.positionCount = maxPoints;
        trajectoryLine.SetPosition(0, shootPoint.transform.position);

        Vector3 currentPosition = shootPoint.position;
        Vector3 currentVelocity = shootPoint.forward * shootingForce * 0.1f;

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
    }
}