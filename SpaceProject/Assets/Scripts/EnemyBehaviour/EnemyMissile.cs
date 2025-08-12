using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private EnemyHealth heatlh;
    [SerializeField] // The higher the value is the faster the ship will rotate
    private float turnRateFactor;
    [SerializeField]
    private float forwardSpeed ;
    [SerializeField]
    private float offSetRaycast = 3f;
    [SerializeField]
    private float lengthCast = 40f;

    void Start() { getTarget(); }

    // Update is called once per Time Unit
    void FixedUpdate()
    {
        getTarget();
        heatlh.TakeDmg(0.02f);
        Pathfinding();
        Move();
        
    }
    // Allowing the enemy unit to turn towards a direction/target
    void Turn()
    {
        // Stores the target position
        if (targetPos == null) { Destroy(this.gameObject); }
        else { 
            Vector3 position = targetPos.position - transform.position;
            // Rotation math to adjust the enemy ship's rotation over deltaTime
            Quaternion rotation = Quaternion.LookRotation(position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                turnRateFactor * Time.deltaTime);
        }
        

        
    }
    //If it fails to find a target, destroy this object to save frames
    void getTarget() {
        try
        {
            if (targetPos == null ) { 
                
                targetPos = GameObject.Find("Player").transform;
            }
        }
        catch (Exception e)
        {
            if (targetPos == null) { Destroy(this.gameObject); }
        }
    }

    // Allowing the enemy unit to move towards a direction/target
    void Move() { transform.position += transform.forward * Time.deltaTime * forwardSpeed; }

    // Basic Pathfinding module - throws raycasts in several directions to
    // avoid collisions with objects
    void Pathfinding()
    {
        RaycastHit hit;
        Vector3 castOffset = Vector3.zero;
        Vector3 castLeft = transform.position - transform.right * offSetRaycast;
        Vector3 castRight = transform.position + transform.right * offSetRaycast;
        Vector3 castUp = transform.position + transform.up * offSetRaycast;
        Vector3 castDown = transform.position - transform.up * offSetRaycast;
        if (Physics.Raycast(castLeft, transform.forward, out hit, lengthCast))
        { castOffset += Vector3.right; }
        else if (Physics.Raycast(castRight, transform.forward, out hit, lengthCast))
        { castOffset -= Vector3.right; }

        if (Physics.Raycast(castUp, transform.forward, out hit, lengthCast))
        { castOffset -= Vector3.up; }
        else if (Physics.Raycast(castDown, transform.forward, out hit, lengthCast))
        { castOffset -= Vector3.up; }

        if (castOffset != Vector3.zero)
        {
            transform.Rotate(castOffset * 2f * Time.deltaTime);
        }
        else { Turn(); }
        
    }
}
