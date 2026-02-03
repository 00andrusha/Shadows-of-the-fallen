using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;

    public Image healthBar;
    
    // --- NEW STUFF ---
    public AudioSource playerAudioSource; 
    public AudioClip gruntSound;
    // -----------------

    void Start()
    {
        health = maxHealth;
        if (healthBar != null) healthBar.fillAmount = 1f;

        // Automatically try to find the AudioSource if you forgot to drag it in
        if (playerAudioSource == null) playerAudioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Play the grunt sound
        if (playerAudioSource != null && gruntSound != null)
        {
            playerAudioSource.PlayOneShot(gruntSound);
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / maxHealth;
        }

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Color originalColor = rend.material.color;
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
        }
    }
}