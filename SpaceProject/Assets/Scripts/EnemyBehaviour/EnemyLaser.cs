using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float laserFiringTime = 0.4f;
    [SerializeField]
    private float maxRange = 300f;
    LineRenderer laserRender;
    bool canFire = false;
    void Awake()
    {
        laserRender = GetComponent<LineRenderer>();
    }

    void Start() { TurnOffLaser(); }
    void Update() { Fire(transform.forward * maxRange); }

    public void Fire(Vector3 targetPos)
    {
        if (canFire)
        {
            // start position
            laserRender.SetPosition(0, transform.position);
            // end position
            laserRender.SetPosition(1, targetPos);
            laserRender.enabled = true;
            canFire = false;
        }
        Invoke("TurnOffLaser", laserFiringTime);
    }

    void TurnOffLaser() {
        laserRender.enabled = false;
        canFire = true;
    }
}
