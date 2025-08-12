using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string newGameScene;
    public string credits;
    public string options;
    public string howTo;
    public string main;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame() {
        SceneManager.LoadScene(newGameScene);
    }
    public void HowTo() { SceneManager.LoadScene(howTo); }
    public void Options() { SceneManager.LoadScene(options); }
    public void Credits() { SceneManager.LoadScene(credits); }
    public void Menu() { SceneManager.LoadScene(main); }
    public void Quit() { Application.Quit(); }
}
