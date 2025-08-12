using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made with Unity 2022.3.36f1 in mind
/// Behaviour of rocks
/// Provides basic parameters
/// and breaking mechanism
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class RockCode : MonoBehaviour
{
    Rigidbody rb;
    public float initialForce = 10f;
    public float initialTorque = 10f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(Vector3.forward * initialForce);
        rb.AddTorque(Vector3.forward * initialTorque);
    }
}
