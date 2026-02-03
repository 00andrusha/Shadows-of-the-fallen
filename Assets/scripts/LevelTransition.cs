using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    [Header("UI Reference")]
    public CanvasGroup whiteScreenGroup; // Drag your White Panel here
    public float fadeDuration = 1.5f;    // How fast it fades to white

    public void TriggerNextLevel()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        // 1. Fade the screen to White
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Lerp from 0 (invisible) to 1 (fully white)
            whiteScreenGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        // Force alpha to 1 just in case
        whiteScreenGroup.alpha = 1;

        // 2. Load the Next Scene
        // Make sure scenes are in File -> Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels in Build Settings! Restarting or quitting...");
            // Optional: SceneManager.LoadScene(0); // Loop back to menu
        }
    }
}
