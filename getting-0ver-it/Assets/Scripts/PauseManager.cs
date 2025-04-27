using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update() {
        // Using Escape key to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        if (pauseMenuUI != null) {
            pauseMenuUI.SetActive(false);
        }

        // Resume the game by setting time scale to 1
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause() {
        if (pauseMenuUI != null) {
            pauseMenuUI.SetActive(true);
        }

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
