using UnityEngine;
using UnityEngine.UI; // [CHANGE 1] Needed for UI

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;

    public Image healthBar; // [CHANGE 2] Drag your Red Bar Image here

    void Start()
    {
        health = maxHealth;

        // Optional: Ensure bar starts full
        if (healthBar != null) healthBar.fillAmount = 1f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // [CHANGE 3] Update the bar
        if (healthBar != null)
        {
            // We use (float) to prevent "Integer Division" errors
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

