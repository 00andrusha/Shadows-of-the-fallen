using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

 
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! HP Left: {currentHealth}");

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        Destroy(gameObject, 0.5f);
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