using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CrackAndLoad : MonoBehaviour
{
    [Header("Settings")]
    public Image flashPanel;
    public string sceneName = "Attack scene 1";
    public float fadeDuration = 1.5f;

    // This function MUST be called by the Signal Receiver
    public void BreakAndGo()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        // 1. Activate Panel
        if (flashPanel != null)
        {
            flashPanel.gameObject.SetActive(true);
            flashPanel.canvasRenderer.SetAlpha(0f);
            flashPanel.CrossFadeAlpha(1f, fadeDuration, false);
        }

        // 2. Wait
        yield return new WaitForSeconds(fadeDuration);

        // 3. Load
        SceneManager.LoadScene(sceneName);
    }
}