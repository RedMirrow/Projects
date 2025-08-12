using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private void OnEnable()
    {
        SkillSlot.OnPerkPointSpent += HandlePerkPointsSpent;
    }
    private void OnDisable()
    {
        SkillSlot.OnPerkPointSpent -= HandlePerkPointsSpent;
    }

    private void HandlePerkPointsSpent(SkillSlot slots)
    {
        string skillName = slots.skillSO.skillName;
        

        switch (skillName) 
        {
            // Heat Capacity
            case "Battery - Tier 1":
                PlayerStats.Instance.UpdateHeatThreshold(0.1f);
                PlayerStats.Instance.UpdateMaxSystemPower(0.1f);
                break;
            case "Battery - Tier 2":
                PlayerStats.Instance.UpdateHeatThreshold(0.1f);
                PlayerStats.Instance.UpdateMaxSystemPower(0.1f);
                break;
            case "Battery - Tier 3":
                PlayerStats.Instance.UpdateHeatThreshold(0.1f);
                PlayerStats.Instance.UpdateMaxSystemPower(0.1f);
                break;
            case "Battery - Tier 4":
                PlayerStats.Instance.UpdateHeatThreshold(0.1f);
                PlayerStats.Instance.UpdateMaxSystemPower(0.1f);
                PlayerStats.Instance.UpdateCoolRateMult(0.1f);
                break;
            case "Battery - Tier 5":
                PlayerStats.Instance.UpdateHeatThreshold(0.1f);
                PlayerStats.Instance.UpdateMaxSystemPower(0.1f);
                PlayerStats.Instance.UpdateCoolRateMult(0.1f);
                break;
            // Boost Strength
            case "Engines - Tier 1":
                PlayerStats.Instance.UpdateSpeed(0.1f);
                PlayerStats.Instance.UpdateTurnRate(0.1f);
                break;
            case "Engines - Tier 2":
                PlayerStats.Instance.UpdateSpeed(0.08f);
                PlayerStats.Instance.UpdateTurnRate(0.1f);
                break;
            case "Engines - Tier 3":
                PlayerStats.Instance.UpdateSpeed(0.08f);
                PlayerStats.Instance.UpdateTurnRate(0.1f);
                break;
            case "Speed Freak":
                PlayerStats.Instance.UpdateSpeed(0.3f);
                PlayerStats.Instance.UpdateTurnRate(0.25f);
                PlayerStats.Instance.UpdateBoostStrength(0.5f);
                PlayerStats.Instance.UpdateMaxHealthMulti(-0.7f);
                break;
            // Hull + Juggernaut
            case "Hull - Tier 1":
                PlayerStats.Instance.UpdateMaxHealthMulti(0.1f);
                PlayerStats.Instance.UpdateBoostStrength(-0.08f);
                PlayerStats.Instance.UpdateSpeed(-0.08f);
                PlayerStats.Instance.UpdateTurnRate(-0.04f);
                break;
            case "Hull - Tier 2":
                PlayerStats.Instance.UpdateMaxHealthMulti(0.2f);
                PlayerStats.Instance.UpdateBoostStrength(-0.08f);
                PlayerStats.Instance.UpdateSpeed(-0.08f);
                PlayerStats.Instance.UpdateTurnRate(-0.04f);
                break;
            case "Hull - Tier 3":
                PlayerStats.Instance.UpdateMaxHealthMulti(0.3f);
                PlayerStats.Instance.UpdateBoostStrength(-0.08f);
                PlayerStats.Instance.UpdateSpeed(-0.16f);
                PlayerStats.Instance.UpdateTurnRate(-0.04f);
                break;
            case "Juggernaut Plating":
                // Adds the flat amount after the multiplied health,
                // as its meant to add 50, not 55 flat health
                PlayerStats.Instance.UpdateMaxHealthMulti(1f);
                PlayerStats.Instance.UpdateMaxHealth(50f);
                PlayerStats.Instance.UpdateBoostStrength(-0.2f);
                PlayerStats.Instance.UpdateSpeed(-0.5f);
                PlayerStats.Instance.UpdateTurnRate(-0.2f);
                break;

            // Durability
            case "Durability - Tier 1":
                PlayerStats.Instance.UpdateMaxHealth(10f);
                break;
            case "Durability - Tier 2":
                PlayerStats.Instance.UpdateMaxHealth(10f);
                break;
            case "Durability - Tier 3":
                PlayerStats.Instance.UpdateMaxHealth(10f);
                break;
            // Flat and Multiplier Damage
            case "Laser Damage - Tier 1":
                PlayerStats.Instance.UpdateFlatLaserDmgBoost(2f);
                break;
            case "Laser Damage - Tier 2":
                PlayerStats.Instance.UpdateFlatLaserDmgBoost(2f);
                break;
            case "Laser Damage - Tier 3":
                PlayerStats.Instance.UpdateFlatLaserDmgBoost(2f);
                break;
            case "Laser Upgrade - Tier 1":
                PlayerStats.Instance.UpdateLaserDmgMult(0.4f);
                PlayerStats.Instance.UpdateLaserHeatRateMult(0.1f);
                break;
            case "Laser Upgrade - Tier 2":
                PlayerStats.Instance.UpdateLaserDmgMult(0.5f);
                PlayerStats.Instance.UpdateLaserHeatRateMult(0.1f);
                break;
            case "Laser Upgrade - Tier 3":
                PlayerStats.Instance.UpdateLaserDmgMult(0.6f);
                PlayerStats.Instance.UpdateLaserHeatRateMult(0.1f);
                break;

            // Fire Rate
            case "Fire Rate - Tier 1":
                PlayerStats.Instance.UpdateTimeBetweenDmgTicks(0.1f);
                break;
            case "Fire Rate - Tier 2":
                PlayerStats.Instance.UpdateTimeBetweenDmgTicks(0.2f);
                break;
            case "Fire Rate - Tier 3":
                PlayerStats.Instance.UpdateTimeBetweenDmgTicks(0.3f);
                break;
            case "Fire Rate - Tier 4":
                PlayerStats.Instance.UpdateTimeBetweenDmgTicks(0.5f);
                break;
            // Laser Heat Cooling
            // Cumulative bonuses
            case "Heat Sinks - Tier 1":
                PlayerStats.Instance.UpdateCoolRateMult(0.1f);
                break;
            case "Heat Sinks - Tier 2":
                PlayerStats.Instance.UpdateCoolRateMult(0.15f);
                break;
            case "Heat Sinks - Tier 3":
                PlayerStats.Instance.UpdateCoolRateMult(0.2f);
                break;
            case "Heat Sinks - Tier 4":
                PlayerStats.Instance.UpdateCoolRateMult(0.3f);
                break;
            // Weapon Choices
            case "Longshot":
                PlayerStats.Instance.UpdateLaserRange(0.5f);
                break;
            case "Flamer Module":
                // TODO: Add Fire Effect
                PlayerStats.Instance.UpdateFlatLaserDmgBoost(4f);
                PlayerStats.Instance.UpdateLaserHeatRateMult(0.3f);
                break;
            case "Caustic Blaster":
                // TODO: Add Armour Reducing effect
                PlayerStats.Instance.UpdateLaserDmgMult(1.2f);
                break;
            // Boost Choices
            case "Heat Dissipation Upgrade":
                PlayerStats.Instance.UpdateLaserHeatRateMult(-0.25f);
                PlayerStats.Instance.UpdateBoostHeatRate(-0.25f);
                PlayerStats.Instance.choiceHeat = true;
                break;
            case "Microbot Centre":
                PlayerStats.Instance.heatSystem.minHeat = 0.3f;
                PlayerStats.Instance.choiceHeal = true;
                break;
            case "Agressive Protocols":
                // TODO: Add Armour Reducing effect
                PlayerStats.Instance.choiceDamage = true;
                break;
            case "Race-Gear Boosters":
                PlayerStats.Instance.UpdateBoostStrength(0.5f);
                break;

            default: 
                Debug.LogWarning("Unknown Skill name"); 
                break;
        }
    }
}
