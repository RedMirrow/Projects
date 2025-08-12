using UnityEngine;

/// <summary>
/// Made with Unity 2022.3.36f1 in mind
/// This code is for the camera to follow the player, given offsets and a speed at which to smooth the movement
/// </summary>

public class FollowPlayer : MonoBehaviour
{
    //Follows the target of the camera
    public Transform target;
    public float smoothSpeed = 0.25f; // The speed at which the camera adjusts to player position
    public Vector3 locationOffset;
    public Vector3 rotationOffset;

    void FixedUpdate() // Is performed within deltaTime, not on per frame basis
    {
        // Stops the camera should the follow target be removed
        if (target!=null)
        {
            Vector3 desiredPosition = target.position + target.rotation * locationOffset;
            // Linear interpolation between the current and target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            Quaternion desiredrotation = target.rotation * Quaternion.Euler(rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
            transform.rotation = smoothedrotation;
        }
        
    }
}
