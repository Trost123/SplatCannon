using UnityEngine;

public class RecoilAnimation : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera's transform
    public float recoilDuration = 0.1f;  // Duration of the recoil
    public float recoilStrength = 0.1f;  // Strength of the recoil

    private Vector3 originalPosition;  // Store the original position of the camera
    private Vector3 recoilOffset;  
    private float recoilTimer;  // Timer for the recoil duration

    private void Start()
    {
        // Store the original position of the camera
        originalPosition = cameraTransform.localPosition;
        recoilOffset = new Vector3(0, 0, recoilStrength);
    }

    public void ApplyRecoil()
    {
        // Apply recoil by moving the camera's position
        cameraTransform.localPosition += recoilOffset;

        // Start the recoil timer
        recoilTimer = recoilDuration;
    }

    private void Update()
    {
        // If the recoil timer is active, reduce the timer
        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;

            // Calculate the progress of the recoil animation (a value between 0 and 1)
            float progress = 1 - Mathf.Clamp01(recoilTimer / recoilDuration);

            // Lerp between the recoiled position and the original position based on progress and recoilTimer
            cameraTransform.localPosition = Vector3.Lerp(originalPosition + recoilOffset, originalPosition, progress);

            // Ensure that the recoil timer doesn't go below zero
            if (recoilTimer <= 0)
            {
                // Snap the position to the original position when the recoil is complete
                cameraTransform.localPosition = originalPosition;
            }
        }
    }
}