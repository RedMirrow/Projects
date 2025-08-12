using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeat : MonoBehaviour
{
    [SerializeField]
    private ShipControl controlSystem;
    [SerializeField]
    private PlayerShooting weaponSystem;
    [SerializeField]
    private ParticleSystem shieldParticles; // Shield particles
    [SerializeField]
    private ParticleSystem ventParticles; // Venting particles
    [SerializeField]
    private Transform location;
    public float coolRate = 0.3f; // Heat loss rate
    public float currentHeat = 0f; // Current heat state
    public float minHeat = 0f; // Minimum heat state (can be adjusted by some perks)
    public bool overHeat = false; // Should stop the player from boosting if true
    public float heatThreshold = 3f; // The system heat capacity
    public bool blocking = false; // Whether the shield is up
    public bool venting = false; // Whether the heat is being vented
    public float blockHeatRate = 0.2f;

    public void Start() { location = GetComponent<Transform>(); }
    // HeatBuildup from the boost
    public void HeatBoost(float boostHeatRate)
    {
        // As long as current heat is less than threshold one can boost the ship
        if (controlSystem.boosting && currentHeat < heatThreshold)
        {
            currentHeat += controlSystem.boostHeatRate * Time.deltaTime;
            // A check once heat was added to see whether the engine overheats
            
            if (currentHeat >= heatThreshold)
            { overHeat = true; controlSystem.boosting = false; }
        }
    }
    // Heating up the laser - if it goes or exceed the threshold, disable firing
    public void HeatLaser(float laserHeatRate)
    {
        // As long as current heat is less than threshold one can beam the lasers
        if (weaponSystem.firing && currentHeat < heatThreshold)
        {
            currentHeat += laserHeatRate * Time.deltaTime;
            // A check once heat was added to see whether the weapon overheats
            
            if (currentHeat >= heatThreshold)
            { overHeat = true; weaponSystem.firing = false; }
        }
    }
    // Cooling the engines reloading
    public void Cool()
    {
        
        // If the boost cool down to at least half from the
        // overheat you can boost again
        if (overHeat)
        {
            if (currentHeat / heatThreshold <= 0.5f)
            {
                overHeat = false;
            }
        }
        // If you are not firing or boosting and the system has heat,
        // cool it down based on time units
        if (!controlSystem.boosting && !weaponSystem.firing && !blocking && currentHeat > minHeat)
        {
            currentHeat -= coolRate * Time.deltaTime;
            if (currentHeat < minHeat) { currentHeat = minHeat; }
        }
    }
    private void OverheatSound() {
        if (currentHeat > heatThreshold * 0.85) { FindObjectOfType<AudioManager>().Play("Overheat"); }
        else { return; }
    }
    public void Update() {ShieldUp(); OverheatSound();}
    public void ShieldUp() {
        if (Input.GetAxisRaw("LaserFire") < 0)
        {
            if (!overHeat) {
                blocking = true;
                if (blocking && currentHeat < heatThreshold)
                {
                    Instantiate(shieldParticles, location.position, location.rotation);
                    currentHeat += blockHeatRate * Time.deltaTime;
                    // A check once heat was added to see whether the weapon overheats
                    if (currentHeat >= heatThreshold)
                    { overHeat = true; blocking = false; }
                }
            }
            
        }
        else { blocking = false; }
        
    }
    // Getters needed for boost bar system
    public float getHeatThreshold() { return heatThreshold; }
    public float getCurrentHeat() { return currentHeat; }
    // Adjusts the heat threshold
    public void UpdateHeatThreshold(float amount)
    // Multiplier bonus
    {
        heatThreshold += heatThreshold * amount;
    }
}
