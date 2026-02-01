using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections; // Required for Coroutines

public class LevelTeleporter : MonoBehaviour
{
    [Header("Settings")]
    public string sceneName; 
    public AudioSource teleportSound; // Drag your Door's AudioSource here

    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range. Press E to enter.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Start the sequence: Play sound -> Wait -> Teleport
            StartCoroutine(PlaySoundAndLoad());
        }
    }

    private IEnumerator PlaySoundAndLoad()
    {
        if (teleportSound != null)
        {
            teleportSound.Play();
            // This waits for the exact length of your audio clip
            yield return new WaitForSeconds(teleportSound.clip.length);
        }

        // Loads the scene after the wait is over
        SceneManager.LoadScene(sceneName);
    }
}