using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth1 : MonoBehaviour
{
    [Header("Health Stats")]
    public int maxHealth = 1500;
    private int currentHealth;

    [Header("UI Reference")]
    public Slider healthBar; 

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            healthBar.gameObject.SetActive(true); 
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            
            // FORCING THE UI TO REDRAW INSTANTLY
            Canvas.ForceUpdateCanvases(); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        if (GetComponent<Collider2D>()) GetComponent<Collider2D>().enabled = false;
        
        Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs) r.enabled = false;

        GameObject loader = GameObject.FindGameObjectWithTag("LevelLoader");
        if (loader != null)
        {
            loader.GetComponent<LevelTransition>().TriggerNextLevel();
        }

        Destroy(gameObject, 2.0f);
    }
}