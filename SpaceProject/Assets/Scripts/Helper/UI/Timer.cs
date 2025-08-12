using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Timer class, if the timer reaches 0 the player wins
/// Also handles the power bar drain and the power bar lose condition
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;
    private float timeValue = 900f; // 15Min timer
    public Image powerBar;
    public bool timeLeft = true;
    public TMP_Text timeText;
    public TMP_Text gameEndText;
    public TMP_Text scoreText;
    public CanvasGroup gameEndScreen;
    public void Start() {
        Time.timeScale = 1f;
        gameEndScreen.alpha = 0f;
        gameEndScreen.interactable = false;
        powerBar.fillAmount = stats.GetSystemPow() / stats.GetMaxPow();
    }
    // Update is called once time unit independent of frame speed
    void Update()
    {
        // If the game is still running
        if (timeLeft) { 
            timeValue -= Time.deltaTime;
            stats.UpdatePower(-1f * Time.deltaTime);
        }
        DisplayTimeLeft(timeValue);
        powerBar.fillAmount = stats.GetSystemPow() / stats.GetMaxPow();
        // If the game has ended due to timer running out
        if (timeValue <= 0) {
            Time.timeScale = 0f;
            timeLeft = false; 
            timeValue = 0;
            gameEndText.text = "You Won!";
            scoreText.text = "Score: " + stats.GetScore();
        }
        // If the game has ended due to timer running out
        

    }

    public void GameEndScreen() {
        gameEndScreen.alpha = 1f;
        gameEndScreen.interactable = true;
    }
    void DisplayTimeLeft(float timeToShow) {
        float minutes = Mathf.FloorToInt(timeToShow / 60);
        float seconds = Mathf.FloorToInt(timeToShow % 60);

        timeText.text = "Time left: " + minutes + ":" + seconds;
    }
}
