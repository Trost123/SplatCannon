using UnityEngine;

internal class Projectile : MonoBehaviour
{
    private Vector3 _currentVelocity;
    private float _collisionDistance; // Maximum distance of the raycast
    
    private int _collisionCount;
    private int _maxCollisionCount = 3;
    

    private void Start()
    {
         _collisionDistance = transform.localScale.x * 0.5f;
    }

    public void SetShotParameters(Vector3 shootPointForward, float shootingForce)
    {
        _currentVelocity = shootPointForward * 0.1f * shootingForce;
    }

    private void Update()
    {
        // Calculate the next position and velocity using physics (including gravity)
        transform.position += _currentVelocity * Time.fixedDeltaTime;
        _currentVelocity += Physics.gravity * 0.05f * Time.fixedDeltaTime;

        CollisionCheckRaycast();
    }
    
    private void CollisionCheckRaycast()
    {
        // Get the direction of the projectile's velocity
        Vector3 rayDirection = _currentVelocity;

        // Cast a ray in the forward direction of the projectile
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDirection, out hit, _collisionDistance))
        {
            // Check if the hit object is tagged as "Plane"
            if (hit.collider.CompareTag("Wall"))
            {
                _currentVelocity = Vector3.Reflect(_currentVelocity, hit.normal);
                
                // Apply velocity damping (adjust the value as needed)
                float dampingFactor = 0.8f; // You can adjust this value
                _currentVelocity *= dampingFactor;
                
                DecalSystem decalSystem = hit.collider.gameObject.GetComponent<DecalSystem>();
                
                //If the wall has DecalSystem component
                if (decalSystem != null)
                {
                    // Convert hit.point to local space of the plane game object
                    Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);

                    // Call the DrawSplat method with localPoint coordinates
                    decalSystem.DrawSplat(localPoint.x, localPoint.z);
                }

                _collisionCount++;

                if (_collisionCount == _maxCollisionCount)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}