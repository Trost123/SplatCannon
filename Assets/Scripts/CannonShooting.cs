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

    const float trajectoryCalculationTimeStep = 0.05f;
    
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

        // Handle shooting when the player presses the fire button (e.g., spacebar)
        if (Input.GetButtonDown("Fire1"))
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
        Vector3 currentVelocity = shootPoint.forward * shootingForce;

        Vector3[] trajectoryPoints = new Vector3[maxPoints];
        int pointIndex = 0;

        // Calculate the initial trajectory point and add it to the array
        trajectoryPoints[pointIndex] = currentPosition;
        pointIndex++;

        for (float time = 0; time < maxPoints; time += trajectoryCalculationTimeStep)
        {
            // Calculate the next position and velocity using physics (including gravity)
            currentPosition += currentVelocity * trajectoryCalculationTimeStep;
            currentVelocity += Physics.gravity * trajectoryCalculationTimeStep;

            // Store the position in the trajectoryPoints array
            trajectoryPoints[pointIndex] = currentPosition;
            pointIndex++;

            // Exit the loop if the projectile goes below the ground
            if (currentPosition.y < 0)
            {
                break;
            }
        }

        // Set the positions of the trajectory line
        trajectoryLine.positionCount = pointIndex;
        trajectoryLine.SetPositions(trajectoryPoints);
    }

    private void Shoot()
    {
        // Instantiate a new projectile from the prefab at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the projectile and apply a force
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootPoint.forward * shootingForce;
        }

        // Destroy the projectile after a certain time (adjust this as needed)
        Destroy(projectile, 10.0f);
    }
}