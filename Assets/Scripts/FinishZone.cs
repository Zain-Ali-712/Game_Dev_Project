using UnityEngine;

public class FinishZone : MonoBehaviour
{
    // A safeguard to make sure the win sequence only fires ONCE 
    // even if the player touches the zone multiple times
    private bool levelCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelCompleted) return;

        // Check if the object entering the zone is labeled as the Player
        if (other.CompareTag("Player"))
        {
            levelCompleted = true;
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        Debug.Log("PLAYER HIT THE FINISH LINE!");

        // Find your LevelManager brain sitting in the scene
        LevelManager manager = Object.FindFirstObjectByType<LevelManager>();

        if (manager != null)
        {
            // Trigger your custom 3-second splash text + summary panel sequence!
            manager.PlayerWon();
        }
        else
        {
            Debug.LogError("Oops! Couldn't find the _LevelManager object in this scene. Make sure it has the LevelManager script attached!");
        }
    }
}