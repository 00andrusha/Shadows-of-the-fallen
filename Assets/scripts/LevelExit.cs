using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing levels

public class LevelExit : MonoBehaviour
{
    public string nextLevelName = "Level2"; // Type the EXACT name of your next scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}