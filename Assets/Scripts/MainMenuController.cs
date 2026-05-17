using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Audio Configurations")]
    public AudioSource menuAudioSource; // Slot for your Audio Source component
    public AudioClip BackgroundMusic;    // Slot for your 'Background music' file

    private static MainMenuController musicInstance;

    void Awake()
    {
        // Simple, clean tracker
        if (musicInstance == null)
        {
            musicInstance = this;
            DontDestroyOnLoad(gameObject); // Keep the first manager alive across scenes!
        }
        else if (musicInstance != this)
        {
            // If we are a duplicate copy in LevelSelection, turn off our local audio immediately 
            // so we don't interfere with the music already playing, but leave the object alive for buttons!
            if (menuAudioSource != null)
            {
                menuAudioSource.enabled = false;
            }
        }
    }

    void Start()
    {
        // Only run the setup if we are the master global music player instance
        if (musicInstance == this)
        {
            if (menuAudioSource != null && BackgroundMusic != null)
            {
                menuAudioSource.clip = BackgroundMusic;
                menuAudioSource.loop = true;

                // Only start playing if it isn't already playing (stops restarting on scene load)
                if (!menuAudioSource.isPlaying)
                {
                    menuAudioSource.Play();
                }
            }
        }
    }

    // Activated by the "START GAME" button (Launches Level 1 directly)
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Activated by the "CHOOSE LEVEL" button (Opens selection screen)
    public void OpenLevelSelector()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    // Activated by individual Level Buttons on the selection screen
    public void LoadSpecificLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Activated by a "BACK" button to return to the Main Menu
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Activated by the "EXIT GAME" button
    public void ExitGame()
    {
        Debug.Log("Game Exited successfully.");
        Application.Quit();
    }
}