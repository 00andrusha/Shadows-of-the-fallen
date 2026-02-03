using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTeleporter : MonoBehaviour
{
    [Header("Settings")]
    public string sceneName; 
    public AudioSource teleportSound; 

    [Header("UI Reference")]
    [Tooltip("Drag your 'Press E' Text or Image object here")]
    public GameObject interactionPopup; 

    private bool isPlayerInRange = false;

    private void Start()
    {
        // Ensure the popup is hidden when the game starts
        if (interactionPopup != null)
        {
            interactionPopup.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show the visual popup
            if (interactionPopup != null) interactionPopup.SetActive(true);
            Debug.Log("Player in range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide the visual popup
            if (interactionPopup != null) interactionPopup.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Hide the popup immediately so it's not floating there while the sound plays
            if (interactionPopup != null) interactionPopup.SetActive(false);
            
            StartCoroutine(PlaySoundAndLoad());
        }
    }

    private IEnumerator PlaySoundAndLoad()
    {
        if (teleportSound != null)
        {
            teleportSound.Play();
            yield return new WaitForSeconds(teleportSound.clip.length);
        }

        SceneManager.LoadScene(sceneName);
    }
}