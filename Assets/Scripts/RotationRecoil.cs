using UnityEngine;

public class RotationRecoil : RecoilAnimation
{
    private Quaternion originalRotation; // Store the original rotation of the object

    private void Start()
    {
        // Store the original rotation of the object
        originalRotation = objectTransform.localRotation;
    }

    public override void ApplyRecoil()
    {
        // Apply recoil by slightly rotating the object
        Vector3 randomTiltDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Quaternion recoilRotation = Quaternion.Euler(randomTiltDirection * recoilStrength);
        objectTransform.localRotation *= recoilRotation;

        // Start the recoil timer
        recoilTimer = recoilDuration;
    }

    public void Update()
    {
        // If the recoil timer is active, reduce the timer
        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;

            // Calculate the progress of the recoil animation (a value between 0 and 1)
            float progress = 1 - Mathf.Clamp01(recoilTimer / recoilDuration);

            // Lerp between the current rotation and the original rotation
            objectTransform.localRotation = Quaternion.Slerp(objectTransform.localRotation, originalRotation, progress);

            // Ensure that the recoil timer doesn't go below zero
            if (recoilTimer <= 0)
            {
                // Snap the rotation to the original rotation when the recoil is complete
                objectTransform.localRotation = originalRotation;
            }
        }
    }
}