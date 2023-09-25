using UnityEngine;

public class CannonControl : MonoBehaviour
{
    // Reference to the cannon barrel
    public Transform cannonBarrel;

    public float rotationSpeed = 30.0f; // Adjust this to control the rotation speed
    public float verticalRotationSpeed = 20.0f; // Adjust this for vertical rotation
    

    private void Update()
    {
        // Get input from arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the rotation angles based on the input and speed
        float horizontalRotationAngle = horizontalInput * rotationSpeed * Time.deltaTime;
        float verticalRotationAngle = verticalInput * verticalRotationSpeed * Time.deltaTime;

        // Rotate the cannon around its up (Y) axis for horizontal rotation
        transform.Rotate(Vector3.up, horizontalRotationAngle);

        // Rotate the cannon barrel around its right (X) axis for vertical rotation
        cannonBarrel.Rotate(Vector3.right, -verticalRotationAngle); // Negative angle to invert vertical rotation
    }
}