using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int currentHealth = 100;
    public TMP_Text healthText;

    [Header("UI Panel References")]
    public GameObject splashTextObject;
    public TMP_Text splashTextMesh;
    public GameObject resultPanel;

    [Header("Summary Text Reference")]
    public TMP_Text summaryStatsText;

    [Header("Level Configuration")]
    public int currentLevelNumber = 1;

    [Header("Audio Configurations")]
    public AudioSource backgroundMusicSource; // For looping background music (Backup player)
    public AudioSource sfxSource;             // For playing victory/loss/hit effects

    [Tooltip("Drag your 'Background music' file here as a backup layout")]
    public AudioClip BackgroundMusic;

    [Tooltip("Drag your 'Game win' file here")]
    public AudioClip GameWin;

    [Tooltip("Drag your 'Game loss' file here")]
    public AudioClip GameLoss;

    [Tooltip("Drag your 'Enemy attack' file here")]
    public AudioClip EnemyAttack;

    private bool isGameOver = false;

    void Start()
    {
        UpdateHealthUI();

        if (splashTextObject != null) splashTextObject.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);

        Time.timeScale = 1f;

        // Smart management system handles background music safely!
        ManageBackgroundMusicTransitions();
    }

    void ManageBackgroundMusicTransitions()
    {
        // UPDATED FOR UNITY 6: Replaced FindObjectOfType with Object.FindFirstObjectByType
        MainMenuController globalMusicManager = Object.FindFirstObjectByType<MainMenuController>();

        if (globalMusicManager != null)
        {
            // If global music manager is found, ensure its source is enabled and playing
            if (globalMusicManager.menuAudioSource != null)
            {
                globalMusicManager.menuAudioSource.enabled = true;
                if (!globalMusicManager.menuAudioSource.isPlaying && BackgroundMusic != null)
                {
                    globalMusicManager.menuAudioSource.clip = BackgroundMusic;
                    globalMusicManager.menuAudioSource.loop = true;
                    globalMusicManager.menuAudioSource.Play();
                }
            }

            // Turn off this level's separate backup track player so they don't clash
            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.Stop();
            }
        }
        else
        {
            // Testing directly inside a scene editor loop floor
            if (backgroundMusicSource != null && BackgroundMusic != null)
            {
                backgroundMusicSource.clip = BackgroundMusic;
                backgroundMusicSource.loop = true;
                backgroundMusicSource.Play();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isGameOver) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (sfxSource != null && EnemyAttack != null)
        {
            sfxSource.PlayOneShot(EnemyAttack);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null && currentHealth > 0)
            {
                playerAnim.SetTrigger("Hit");
            }
        }

        if (currentHealth <= 0)
        {
            PlayerLost();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health = " + currentHealth;
            healthText.color = (currentHealth <= 30) ? Color.red : Color.green;
        }
    }

    public void PlayerWon()
    {
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(WinSequence());
    }

    void PlayerLost()
    {
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(LossSequence());
    }

    IEnumerator WinSequence()
    {
        PlayerPrefs.SetString("Level" + currentLevelNumber + "_Status", "<color=green>WIN</color>");
        PlayerPrefs.SetInt("Level" + currentLevelNumber + "_Played", 1);
        PlayerPrefs.Save();

        StopAllGlobalAndLocalBackgroundMusic();

        if (sfxSource != null && GameWin != null) sfxSource.PlayOneShot(GameWin);

        Time.timeScale = 0f;

        splashTextMesh.text = "HURRAH! YOU WIN";
        splashTextMesh.color = Color.green;
        splashTextObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        splashTextObject.SetActive(false);

        if (summaryStatsText != null)
        {
            summaryStatsText.text = "<b><color=green>STAGE CLEAR!</color></b>\n\n" +
                                    "Remaining HP: " + currentHealth + "/100\n" +
                                    "Status: Survivor";
        }

        resultPanel.SetActive(true);
    }

    IEnumerator LossSequence()
    {
        PlayerPrefs.SetString("Level" + currentLevelNumber + "_Status", "<color=red>FAIL</color>");
        PlayerPrefs.SetInt("Level" + currentLevelNumber + "_Played", 1);
        PlayerPrefs.Save();

        StopAllGlobalAndLocalBackgroundMusic();

        if (sfxSource != null && GameLoss != null) sfxSource.PlayOneShot(GameLoss);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Die");
            }

            MonoBehaviour controller = player.GetComponent("PlayerController") as MonoBehaviour;
            if (controller != null) controller.enabled = false;
        }

        splashTextMesh.text = "ALAS! YOU LOST";
        splashTextMesh.color = Color.red;
        splashTextObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1.5f);

        splashTextObject.SetActive(false);

        if (summaryStatsText != null)
        {
            summaryStatsText.text = "<b><color=red>YOU DIED</color></b>\n\n" +
                                    "Remaining HP: 0/100\n" +
                                    "Status: Bitten";
        }

        resultPanel.SetActive(true);
    }

    void StopAllGlobalAndLocalBackgroundMusic()
    {
        if (backgroundMusicSource != null) backgroundMusicSource.Stop();

        // UPDATED FOR UNITY 6: Replaced FindObjectOfType with Object.FindFirstObjectByType
        MainMenuController globalMusicManager = Object.FindFirstObjectByType<MainMenuController>();
        if (globalMusicManager != null && globalMusicManager.menuAudioSource != null)
        {
            globalMusicManager.menuAudioSource.Stop();
        }
    }

    public void MoveToNextLevel()
    {
        Time.timeScale = 1f;
        if (currentLevelNumber == 1) SceneManager.LoadScene("Level2");
        else if (currentLevelNumber == 2) SceneManager.LoadScene("Level3");
        else if (currentLevelNumber == 3) SceneManager.LoadScene("Summary Scene");
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        StopAllGlobalAndLocalBackgroundMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        StopAllGlobalAndLocalBackgroundMusic();
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToSummarySceneManually()
    {
        Time.timeScale = 1f;
        StopAllGlobalAndLocalBackgroundMusic();
        SceneManager.LoadScene("Summary Scene");
    }
}