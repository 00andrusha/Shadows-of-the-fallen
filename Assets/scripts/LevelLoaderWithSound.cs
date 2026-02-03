using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoaderWithSound : MonoBehaviour
{
    public AudioSource audioSource;

    // Connect this function to your Button
    public void PlaySoundAndLoad()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        // 1. Play sound if it exists
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 2. Wait 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // 3. Load the scene named "SampleScene"
        // Make sure "SampleScene" is inside quotes " "
        SceneManager.LoadScene("Ok");
    }
}