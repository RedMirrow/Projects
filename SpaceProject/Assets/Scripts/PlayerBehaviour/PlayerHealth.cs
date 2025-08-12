using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private PlayerHeat heatSystem;
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private Timer timerCode;
    public TMP_Text gameEndText;
    public TMP_Text scoreText;
    public float maxHealth = 50f;
    public float health = 50f;
    public float timeSinceLastHit = 0f;
    public string main = "Menu";
    void FixedUpdate() { TimeSinceHit();  }
    public void ChangeHealth(float amount)
    {
        // If you would take damage whilst blocking, dont take damage but accrue some heat
        if (amount < 0 && heatSystem.blocking) { health = health; heatSystem.currentHeat += (-amount * 0.5f) * Time.deltaTime; }
        else { health += amount; }

        if (health > maxHealth) { health = maxHealth; }
        if (amount < 0) { timeSinceLastHit = 2f; }
        if (health < 0) {
            FindObjectOfType<AudioManager>().Play("PlayerDestroyed");
            Time.timeScale = 0f;
            gameEndText.text = "You Lost!";
            scoreText.text = "Score: " + stats.GetScore();
            Cursor.visible = true;
            timerCode.GameEndScreen();
            Destroy(this.gameObject);
        }

    }
    void TimeSinceHit() { timeSinceLastHit -= 0.2f * Time.deltaTime;
        if (timeSinceLastHit < 0) { timeSinceLastHit = 0f; }
    }
    public float getHealth() { return health; }
    public float getMaxHealth() { return maxHealth; }
}
