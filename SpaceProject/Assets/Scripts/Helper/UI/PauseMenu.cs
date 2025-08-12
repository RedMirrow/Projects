using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isGameEnd;
    public static bool gamePaused = false;
    public GameObject pauseMenu;
    public string main;

    public void Start() { Cursor.visible = false; }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gamePaused)
                {
                    Resume(); Cursor.visible = false;
            }
                else { Pause(); Cursor.visible = true; }
            }
    }
    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        
    }
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        
    }
}
