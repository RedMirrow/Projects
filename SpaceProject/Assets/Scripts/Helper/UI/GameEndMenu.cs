using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndMenu : MonoBehaviour
{
    public bool isGameEnd;
    public static bool gamePaused = false;
    public GameObject gameEndMenu;
    public string main;


    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(main);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void Pause()
    {
        gameEndMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true; 
        Cursor.visible = true;
    }
    public void Resume()
    {
        gameEndMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.visible = false;
    }
}
