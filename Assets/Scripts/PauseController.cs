using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("UI Reference")]
    // Drag your PauseMenuPanel into this slot in the Unity Inspector
    public GameObject pauseMenuPanel;

    // This tracks whether the game is currently paused or running
    private bool isPaused = false;

    void Update()
    {
        // This allows PC players to also hit the 'Escape' key on their keyboard to pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 1. Activated by your screen's "Pause Game" button
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // Shows your beautiful dark blue panel
        Time.timeScale = 0f;            // FREEZES the game world physics, time, and zombies completely
        isPaused = true;
    }

    // 2. Activated by your panel's "Resume" button
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // Hides your pause menu panel
        Time.timeScale = 1f;             // Restores time back to normal speed so the game continues
        isPaused = false;
    }

    // 3. Activated by your panel's "Restart" button
    public void RestartLevel()
    {
        Time.timeScale = 1f; // CRITICAL: Always unfreeze time before switching or reloading scenes!

        // This automatically checks the name of the current active scene and reloads it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 4. Activated by your panel's "Main Menu" button
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // CRITICAL: Unfreeze time so your main menu buttons work smoothly

        // Loads your Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }
}