using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos;
    [SerializeField] // The higher the value is the faster the ship will rotate
    public float turnRateFactor = 0.7f;
    [SerializeField]
    private float forwardSpeed = 60f;
    [SerializeField]
    private float offSetRaycast = 3f;
    [SerializeField]
    private float lengthCast = 40f;

    [SerializeField]
    [Range(-0.2f, 4f)]
    private float minRandTrack = -0.2f;
    [SerializeField]
    [Range(-0.2f, 4f)]
    private float maxRandTrack = 1f;

    void Start() { getTarget(); 
        float trackingAccuracy = UnityEngine.Random.Range(minRandTrack, maxRandTrack);
        turnRateFactor += trackingAccuracy;
    }

    // Update is called once per Time Unit
    void FixedUpdate()
    {
        Pathfinding();
        Move();
    }
    // Allowing the enemy unit to turn towards a direction/target
    void Turn() {
        if (targetPos == null) { Destroy(this.gameObject); }
        // Stores the target position
        Vector3 position = targetPos.position - transform.position;

        // Rotation math to adjust the enemy ship's rotation over deltaTime
        Quaternion rotation = Quaternion.LookRotation(position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 
            turnRateFactor * Time.deltaTime);
    }

    void getTarget() {
        try
        {
            if (targetPos == null) { targetPos = GameObject.Find("Player").transform; }
            targetPos = GameObject.Find("Player").transform;
        }
        catch (Exception e)
        {
            if (targetPos == null) { Destroy(this.gameObject); }
        }
    }

    // Allowing the enemy unit to move towards a direction/target
    void Move() {
        // If the current position excceeds a threshold, remove the ship to save memory
        if (transform.position.x > 3500 || transform.position.x < -3500) { Destroy(gameObject); return; }
        if (transform.position.y > 3500 || transform.position.y < -3500) { Destroy(gameObject); return; }
        if (transform.position.z > 3500 || transform.position.z < -3500) { Destroy(gameObject); return; }
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
    }

    // Basic Pathfinding module - throws raycasts in several directions to
    // avoid collisions with objects
    void Pathfinding() {
        //If it fails to find a target, destroy this object to save frames
        getTarget();

        RaycastHit hit;
        Vector3 castOffset = Vector3.zero;
        Vector3 castLeft = transform.position - transform.right * offSetRaycast;
        Vector3 castRight = transform.position + transform.right * offSetRaycast;
        Vector3 castUp = transform.position + transform.up * offSetRaycast;
        Vector3 castDown = transform.position - transform.up * offSetRaycast;
        if (Physics.Raycast(castLeft, transform.forward, out hit, lengthCast)) 
        { castOffset += Vector3.right; }
        else if(Physics.Raycast(castRight, transform.forward, out hit, lengthCast)) 
            { castOffset -= Vector3.right; }

        if (Physics.Raycast(castUp, transform.forward, out hit, lengthCast))
        { castOffset -= Vector3.up; }
        else if(Physics.Raycast(castDown, transform.forward, out hit, lengthCast))
            { castOffset += Vector3.up; }

        if (castOffset != Vector3.zero) {
            transform.Rotate(castOffset * 4f * Time.deltaTime);
        }
        else { Turn(); }
    }
}
