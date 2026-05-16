using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    private bool levelCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelCompleted) return;

        if (other.CompareTag("Player"))
        {
            levelCompleted = true;
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("LEVEL COMPLETED");

        // LEVEL 3 = GAME OVER
        if (currentIndex == 2)
        {
            Debug.Log("GAME OVER - YOU WIN 🎉");

            // Quit game (only works in build)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            return;
        }

        // Otherwise go to next level
        SceneManager.LoadScene(currentIndex + 1);
    }
}