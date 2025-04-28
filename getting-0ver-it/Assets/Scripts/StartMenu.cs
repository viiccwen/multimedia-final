using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void Continue()
    {
        PauseManager.loadGame = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void Options()
    {
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
