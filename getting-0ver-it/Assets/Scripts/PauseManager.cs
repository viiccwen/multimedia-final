using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Transform playerPos, playerBodyPos;
    public static bool isPaused = false;
    public static bool loadGame = false;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!loadGame) return;

        // Load game data here
        GameData data = SaveSystem.LoadData();

        if (data == null) {
            Debug.Log("No Data Found.");
            return;
        }
        
        Vector3 newPos = new Vector3(data.pos[0], data.pos[1], data.pos[2]);
        playerPos.position = newPos;

        Timer timer = FindObjectOfType<Timer>();
        if (timer != null) {
            timer.setElapsedTime(data.time);
        }

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null) {
            scoreManager.setScore(data.score);
        }

        loadGame = false;
    }

    void Update() {
        // Using Escape key to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
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
        isPaused = false;
    }

    public void Pause() {
        if (pauseMenuUI != null) {
            pauseMenuUI.SetActive(true);
        }

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void saveGame() {
        Debug.Log("saving game...");
        SaveSystem.SaveData(playerBodyPos, this.GetComponent<Timer>(), this.GetComponent<ScoreManager>());
    }

    public void LoadMainMenu() {
        Time.timeScale = 1f; // Ensure time scale is reset when loading a new scene
        isPaused = false;
        saveGame();
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame() {
        saveGame();
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
