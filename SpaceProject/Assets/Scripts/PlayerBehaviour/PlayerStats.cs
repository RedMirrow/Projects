using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Holds Player Stats and data
/// Holds 
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public ShipControl control;
    public PlayerShooting playerShoot;
    [SerializeField]
    public PlayerHeat heatSystem;
    

    public PlayerHealth playerHealth;
    public static event Action<int> OnLevelUp;

    [Header("=== Levelling Up ===")]
    [SerializeField]
    public float dustGained = 0f; // Exp
    [SerializeField]
    private float dustForLvl = 200f; // Exp needed
    [SerializeField]
    private float score = 0f; // Current Score
    [SerializeField]
    private float levelCostMultiplier = 1.5f;
    [SerializeField]
    private int level = 1; // Player level count
    public TMP_Text levelText;



    [Header("=== Improveable Stats ===")]
    [SerializeField]
    private float hpMultiplier = 1f; // HP multiplier
    [SerializeField]
    private float speedMultiplier = 1f; // Speed multiplier
    [SerializeField]
    private float turnRate = 90f;
    [SerializeField]
    private float powerMultiplier = 1f; // System Power multiplier
    [SerializeField]
    private float maxSystemPower = 55f; // The limit of the player's power bar - can be improved with battery upgrades
                                        // At base, 55 sec
    [SerializeField]
    private float systemPower = 55f; // If it goes to 0 the player loses the game
    [SerializeField]
    private float boostHeatRateMult = 1f; // Multiplier of boost heat generation
    [SerializeField]
    private float boostStrength = 1f; // Multiplier of boost strength
    [SerializeField]
    private float heatThresholdMult = 1f; // Multiplier of boost heat threshold
    [SerializeField]
    private float laserHeatRateMult = 1f; // Multiplier of laser heat generation
    [SerializeField]
    private float coolRateMult = 1f; // Multiplier of laser heat loss
    [SerializeField]
    private float laserDmgMult = 1f; // Multiplier of laser damage

    [Header("=== Boost Options ===")]
    [SerializeField]
    public bool choiceHeat = false; // Heat Reroute ability toggle
    [SerializeField]
    public bool choiceHeal = false; // Heal ability toggle
    [SerializeField]
    public bool choiceDamage = false; // Damage ability toggle

    private bool isDead = false;

    private void Awake() {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    public void OnEnable() {
        HealthObjects.OnObjectDestroyed += addDust;
    }
    public void OnDisabled() { HealthObjects.OnObjectDestroyed -= addDust; }

    public float getCurrentDust() { return dustGained; }
    public float GetMaxPow() { return maxSystemPower; }
    public float GetSystemPow() { return systemPower; }
    public float getDustForLvl() { return dustForLvl; }
    public int getLvl() { return level; }
    public float GetScore() { return score; }
    public void Start() { levelText.text = "Level: " + level; }
    // 'Exp' Gain method, accrues the score by the same amount
    public void addDust(float amount)
    {
        dustGained+= amount;
        score += amount;
        //Destroying any object adds 5sec to the power bar
        UpdatePower(5f);
        if (dustGained >= dustForLvl) { levelUp(); }
    }

    void levelUp() {
        // On levelup, take away needed exp/dust
        // increment the level
        // Heal player to their full hp
        // Call for the level up function (gain a perk point and multiply exp needed
        if (dustGained >= dustForLvl) {
            dustGained -= dustForLvl;
            dustForLvl = Mathf.RoundToInt(dustForLvl * levelCostMultiplier);
            FindObjectOfType<AudioManager>().Play("LevelUp");
            level++;
            OnLevelUp?.Invoke(1);
            playerHealth.health = playerHealth.maxHealth;
            levelText.text = "Level " + level;
        }
    }
    //Upgrading stats
    public void UpdateMaxHealth(float amount)
    // Flat Bonus
    {
        playerHealth.maxHealth += amount; 
        playerHealth.ChangeHealth(amount);
    }
    public void UpdateMaxHealthMulti(float amount)
    // Multiplier bonus
    {
        hpMultiplier += amount;
        playerHealth.maxHealth = playerHealth.maxHealth * hpMultiplier; 
        playerHealth.health += playerHealth.maxHealth * amount; 
    }
    public void UpdateMaxSystemPower(float amount)
    // Multiplier bonus
    {
        powerMultiplier += amount;
        maxSystemPower = maxSystemPower * powerMultiplier;
        UpdatePower(maxSystemPower * amount); // Allows to prolong the run by giving that one bit of more power in the tank to try to get more
    }
    public void UpdatePower(float amount)
    // Can both add and substract system power when called
    {
        
        systemPower += amount;
        // Preventing system power being greater than max power
        if (systemPower > maxSystemPower) { systemPower = maxSystemPower; }
        // Destroy the player ship should ship power reach 0,
        // the null check is to prevent the sound of the ship being destroyed from playing on repeat
        if (systemPower <= 0) {
            if (playerHealth != null) { playerHealth.ChangeHealth(-playerHealth.maxHealth); }
             
        }
    }
    public void UpdateSpeed(float amount) 
        // Can be both speed up and down
    {
        speedMultiplier += amount;
        control.speedLimit += control.speedLimit * amount;
        control.forwardSpeed += control.forwardSpeed * amount;
        control.sideSpeed += control.sideSpeed * amount;
        control.hoverSpeed += control.hoverSpeed * amount;
    }
    public void UpdateTurnRate(float amount)
    // Can be both speed up and down
    {
        turnRate += amount;
        control.lookRate += control.lookRate * amount;
    }
    public void UpdateBoostHeatRate(float amount)
    // Both cool and boost, cool values are -, heat are +
    // Multiplier bonus
    {
        boostHeatRateMult += boostHeatRateMult * amount;
        control.boostHeatRate += control.boostHeatRate * amount;
    }
    public void UpdateHeatThreshold(float amount)
    // Multiplier bonus
    {
        heatThresholdMult += amount;
        heatSystem.UpdateHeatThreshold(amount);
    }
    public void UpdateBoostStrength(float amount)
    // Multiplier bonus
    {
        boostStrength += amount;
        control.boostRate += control.boostRate * amount;
        control.boostLimit += (control.boostLimit * amount) / 2; // Improves the boost limit at half the effect
    }
    public void UpdateLaserHeatRateMult(float amount)
    // Multiplier bonus
    {
        laserHeatRateMult += amount;
        playerShoot.laserHeatRate += playerShoot.laserHeatRate * amount;
    }
    public void UpdateCoolRateMult(float amount)
    // Multiplier bonus
    {
        coolRateMult += amount;
        heatSystem.coolRate += heatSystem.coolRate * amount;
    }
    public void UpdateLaserDmgMult(float amount)
    // Multiplier bonus
    {
        laserDmgMult += amount;
        playerShoot.laserDmg += playerShoot.laserDmg * amount;
    }
    public void UpdateLaserRange(float amount)
    // Flat Bonus
    {
        playerShoot.hardpointRange += playerShoot.hardpointRange * amount;
    }
    public void UpdateFlatLaserDmgBoost(float amount)
    // Flat bonus
    {
        playerShoot.laserDmg += amount;
        
    }
    public void UpdateTimeBetweenDmgTicks(float amount)
    // Flat Bonus
    {
        playerShoot.timeBetweenDmgTicks += amount;
    }



}
