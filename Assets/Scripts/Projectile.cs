using UnityEngine;

internal class Projectile : MonoBehaviour
{
    private Vector3 _currentVelocity;

    public void SetShotParameters(Vector3 shootPointForward, float shootingForce)
    {
        _currentVelocity = shootPointForward * 0.1f * shootingForce;
    }

    private void Update()
    {
        // Calculate the next position and velocity using physics (including gravity)
        transform.position += _currentVelocity * Time.fixedDeltaTime;
        _currentVelocity += Physics.gravity * 0.05f * Time.fixedDeltaTime;
    }
}