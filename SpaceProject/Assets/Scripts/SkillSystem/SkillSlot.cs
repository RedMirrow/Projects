using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillSlot : MonoBehaviour
{
    // What skills can and cannot be held together
    // For balancing reasons
    public List<SkillSlot> prerequisiteSkillSlot;

    public skillSO skillSO;
    public Image icon;

    public int currentLvl;
    public bool isUnlocked;
    public TMP_Text skillLeveltext;
    public Button skillBtn;

    public static event Action<SkillSlot> OnPerkPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;

    private void OnValidate()
    {
        if (skillSO != null && skillLeveltext != null)
        {
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        icon.sprite = skillSO.skillIcon;

        if (isUnlocked)
        {
            skillBtn.interactable = true;
            icon.color = Color.white;
            skillLeveltext.text = currentLvl.ToString() + "/" + skillSO.maxLvl;
        }
        else
        {
            skillBtn.interactable = false;
            skillLeveltext.text = "N/A";
            icon.color = Color.grey;
        }
    }
    public void TryUpgradeSkill()
    {
        if (isUnlocked && currentLvl < 1)
        {
            currentLvl++;
            OnPerkPointSpent?.Invoke(this);
            UpdateUI();

            if (currentLvl >= skillSO.maxLvl) {
                OnSkillMaxed?.Invoke(this);
            }
        }
    }
    public bool CanUnlockSkill()
    {
        foreach (SkillSlot slot in prerequisiteSkillSlot) {
            if (!slot.isUnlocked || slot.currentLvl < slot.skillSO.maxLvl) { return false; }
        }
        int count = 0;
        return true;
    }
    public void Unlock() { 
        isUnlocked = true;
        UpdateUI(); 
    }
}
