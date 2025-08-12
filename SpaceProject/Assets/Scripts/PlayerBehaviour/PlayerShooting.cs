using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made with Unity 2022.3.36f1 in mind
/// This code is for shooting lasers using LineRenderer tools
/// Provides a means for the player to shoot a laser's
/// Provides a basic 'heat' system that can be seen as a magazine
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    //References to Camera and the given player 
    public Camera cam;
    public GameObject player;
    [SerializeField]
    private PlayerHeat heatSystem;

    // Hardpoints - Where lasers can shoot from
    [Header("=== Hardpoint ===")]
    [SerializeField]
    private Transform[] hardpoints;
    [SerializeField]
    private LayerMask shootableMask; // Potential target
    [SerializeField]
    public float hardpointRange = 300f; // Range of the weapon platform
    [SerializeField]
    private bool targetInRange = false;

    // Laser settings
    [Header("=== Laser ===")]
    [SerializeField]
    private LineRenderer[] lasers; // Laser beams
    [SerializeField]
    private ParticleSystem laserHitParticles; // Laser contact particles
    [SerializeField]
    public float laserDmg = 2f; // Damage dealt by the laser
    [SerializeField]
    public float timeBetweenDmgTicks = 0.15f;
    private float currentTimeBetweenDmgTicks = 0f;

    [SerializeField]
    public float laserHeatRate = 0.08f; // Heat build up per laser
    
    [SerializeField]
    public bool firing = false;

    public void Awake() { cam = Camera.main; }



    // Updates the firing state
    private void Update() { 
        OnFire();
        HandleLaserFiring(); 
    }

    private void HandleLaserFiring() {
        // If firing and not overheated fire
        if (firing && !heatSystem.overHeat) { FireLaser(); }
        // Else start cooling
        else {
            // Disables all of the lasers of the object if not firing
            foreach (var laser in lasers) {
                laser.gameObject.SetActive(false);
            }
            heatSystem.Cool();
        }
    }
    //Applies damage to the hit object
    void ApplyDamage(HealthObjects healthObjects) {
        currentTimeBetweenDmgTicks += Time.deltaTime;
        if (currentTimeBetweenDmgTicks > timeBetweenDmgTicks) {
            currentTimeBetweenDmgTicks = 0f;
            healthObjects.TakeDmg(laserDmg);
        }
        heatSystem.HeatLaser(laserHeatRate); // Calls for the heat system

    }

    // Laser Firing
    void FireLaser()
    {
        // Creates a RaycastHit
        RaycastHit hitInfo;
        // If the lasers' rays hit an object, direct the lasers to the object
        if (TargetInfo.IsTargetInRange(cam.transform.position, 
            cam.transform.forward,out hitInfo, hardpointRange, shootableMask))
        {
            targetInRange = true;
            foreach (var laser in lasers)
            {
                Vector3 localHitPosition = laser.transform.InverseTransformPoint(hitInfo.point);
                laser.gameObject.SetActive(true);
                laser.SetPosition(1, localHitPosition);
                if (hitInfo.collider.GetComponent<HealthObjects>())
                {
                    Instantiate(laserHitParticles, hitInfo.point, Quaternion.LookRotation(hitInfo.point));
                    ApplyDamage(hitInfo.collider.GetComponent<HealthObjects>());
                    
                }
            }
            

        }
        // If the laser fails to find a valid target
        // It still shoots up to the weapon range
        else {
            foreach (var laser in lasers)
            {
                targetInRange = false;
                laser.gameObject.SetActive(true);
                Vector3 destination = new Vector3(0, 0, hardpointRange);
                laser.SetPosition(1, destination);
                heatSystem.HeatLaser(laserHeatRate); // Calls for the heat system
            }
        }
        

    }
    

    // Checks whether the fire button is pressed
    public void OnFire() {
        if (Input.GetAxisRaw("LaserFire") > 0) { firing = true; }
        else { firing = false; }
        
    }
}
