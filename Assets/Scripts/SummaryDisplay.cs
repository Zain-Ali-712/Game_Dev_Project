using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SummaryDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text overallStatsText;

    [Header("Audio Configurations (Editor Testing Backup)")]
    [Tooltip("Optional: Add an AudioSource to this object and link it here so music plays if you test this scene directly!")]
    public AudioSource localBackupSource;
    public AudioClip BackgroundMusic;

    void Start()
    {
        // Unlock mouse cursor visibility so players can interact with the system buttons cleanly
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisplayFinalResults();

        // 🎵 RESTART THE BACKGROUND MUSIC FOR THE SUMMARY SCENE 🎵
        ManageSummaryMusic();
    }

    void ManageSummaryMusic()
    {
        // UPDATED FOR UNITY 6: Replaced FindObjectOfType with Object.FindFirstObjectByType
        MainMenuController globalMusicManager = Object.FindFirstObjectByType<MainMenuController>();

        if (globalMusicManager != null && globalMusicManager.menuAudioSource != null)
        {
            // Turn its audio source component back on, reset the track, and loop it!
            globalMusicManager.menuAudioSource.enabled = true;

            if (BackgroundMusic != null)
            {
                globalMusicManager.menuAudioSource.clip = BackgroundMusic;
            }

            globalMusicManager.menuAudioSource.loop = true;
            globalMusicManager.menuAudioSource.Play();
        }
        else
        {
            // Fallback: If testing this scene by itself, play the local backup configuration
            if (localBackupSource != null && BackgroundMusic != null)
            {
                localBackupSource.clip = BackgroundMusic;
                localBackupSource.loop = true;
                localBackupSource.Play();
            }
        }
    }

    void DisplayFinalResults()
    {
        string reportCard = "<align=center><b><size=50><color=#FFCC00>FINAL MISSION REPORT</color></size></b>\n";
        reportCard += "<size=28><color=#FFFFFF>RECON RUN ANALYSIS</color></size></align>\n\n\n";

        // --- TRACK LEVEL 1 RECORD ---
        if (PlayerPrefs.GetInt("Level1_Played", 0) == 1)
        {
            string status = PlayerPrefs.GetString("Level1_Status", "Not Played");
            reportCard += "   • <b>LEVEL 01 STATUS:</b> " + status + "\n\n";
        }
        else
        {
            reportCard += "   • <b>LEVEL 01 STATUS:</b> <color=#EEEEEE>Skipped / Not Played</color>\n\n";
        }

        // --- TRACK LEVEL 2 RECORD ---
        if (PlayerPrefs.GetInt("Level2_Played", 0) == 1)
        {
            string status = PlayerPrefs.GetString("Level2_Status", "Not Played");
            reportCard += "   • <b>LEVEL 02 STATUS:</b> " + status + "\n\n";
        }
        else
        {
            reportCard += "   • <b>LEVEL 02 STATUS:</b> <color=#EEEEEE>Skipped / Not Played</color>\n\n";
        }

        // --- TRACK LEVEL 3 RECORD ---
        if (PlayerPrefs.GetInt("Level3_Played", 0) == 1)
        {
            string status = PlayerPrefs.GetString("Level3_Status", "Not Played");
            reportCard += "   • <b>LEVEL 03 STATUS:</b> " + status + "\n\n";
        }
        else
        {
            reportCard += "   • <b>LEVEL 03 STATUS:</b> <color=#EEEEEE>Skipped / Not Played</color>\n\n";
        }

        if (overallStatsText != null)
        {
            overallStatsText.text = reportCard;
        }
    }

    // Assign this directly to your Return to Main Menu button onClick profile event block
    public void ClearDataAndReturnToMenu()
    {
        // UPDATED FOR UNITY 6: Replaced FindObjectOfType with Object.FindFirstObjectByType
        MainMenuController globalMusicManager = Object.FindFirstObjectByType<MainMenuController>();
        if (globalMusicManager != null)
        {
            Destroy(globalMusicManager.gameObject);
        }

        PlayerPrefs.DeleteAll(); // Wipes current score matrix clear for structural safety
        SceneManager.LoadScene("Main Menu");
    }
}