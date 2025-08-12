using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    // Sets up an array to hold skill data to apply to the player
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int unspentPoint;
    public int spentPoint;

    private void Start()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillBtn.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdatePerkPoints(0);
    }
    private void CheckAvailablePoints(SkillSlot slot) {
        if (unspentPoint > 0) { slot.TryUpgradeSkill(); }
    }
    private void OnEnable() {
        SkillSlot.OnPerkPointSpent += HandlePerkPointsSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
        PlayerStats.OnLevelUp += UpdatePerkPoints;
    }
    private void OnDisable()
    {
        SkillSlot.OnPerkPointSpent -= HandlePerkPointsSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
        PlayerStats.OnLevelUp -= UpdatePerkPoints;
    }
    private void HandleSkillMaxed(SkillSlot slots) {
        foreach (SkillSlot slot in skillSlots)
        {
            if (!slot.isUnlocked && slot.CanUnlockSkill()) { slot.Unlock(); }
            
        }
    }
    private void HandlePerkPointsSpent(SkillSlot slots) {
        if (unspentPoint > 0) { UpdatePerkPoints(-1); }
    }
    public void UpdatePerkPoints(int amount)
    {
        unspentPoint += amount;
        pointsText.text = "Perk Points: " + unspentPoint;
    }
}
