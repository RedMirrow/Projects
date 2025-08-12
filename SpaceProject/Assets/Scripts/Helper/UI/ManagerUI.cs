using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerUI : MonoBehaviour
{
    public Image heatImage;

    [SerializeField]
    private PlayerHeat heatSystem;

    public Image healthBar;
    public PlayerHealth playerHealth;

    public Image expBar;
    public Image expBar2;
    public PlayerStats playerDust;
    public CanvasGroup canvasSkillTree;
    public GameObject skillManager;

    public void Start() {

    }

    private void Update() {
        if (heatSystem != null) {
            heatImage.fillAmount = heatSystem.getCurrentHeat() / heatSystem.getHeatThreshold();
        }
        if (playerHealth != null)
        {
            healthBar.fillAmount = playerHealth.getHealth() / playerHealth.getMaxHealth();
        }
        if (playerDust != null)
        {
            expBar.fillAmount = playerDust.getCurrentDust() / playerDust.getDustForLvl();
            expBar2.fillAmount = playerDust.getCurrentDust() / playerDust.getDustForLvl();
        }
        if (Input.GetAxisRaw("SkillMenu") != 0) {
            canvasSkillTree.alpha = 1f;
            canvasSkillTree.interactable = true;
            Cursor.visible = true;
        }
        else {
            canvasSkillTree.alpha = 0f;
            canvasSkillTree.interactable = false;
            if (Time.timeScale != 0f && playerHealth.getHealth() > 0f) { Cursor.visible = false; }
            
        }
    }
}
