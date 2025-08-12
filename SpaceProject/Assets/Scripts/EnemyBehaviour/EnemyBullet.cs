using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Shoots a bullet that goes directly towards the player with a spread
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    // Targetting component
    public Transform target;
    [SerializeField]
    private float range = 250f;
    [SerializeField]
    private float bulletSpeed = 60f;
    [SerializeField]
    private bool inRange = false;

    public Rigidbody model;

    void Start() {
        getTarget();
        float spread = UnityEngine.Random.Range(1.0f, 3.0f);
        InvokeRepeating("Shoot", 0.3f, spread);
        
    }
    void getTarget()
    {
        try
        {
            if (target == null) { target = GameObject.Find("Player").transform; }
        }
        catch (Exception e)
        {
            if (target == null) { Destroy(this.gameObject); }
        }
    }
    void Update()
    {
        if (target == null) { Destroy(this.gameObject); }
        if (target != null) {
            inRange = Vector3.Distance(transform.position, target.position) < range;
            if (inRange){ transform.LookAt(target); }
        }
        
    }

    void Shoot() {
        if (inRange)
        {
            Rigidbody bullet = (Rigidbody)Instantiate(model, transform.position + transform.forward, transform.rotation);
            bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bullet.gameObject, 4);
        }
    }
}
