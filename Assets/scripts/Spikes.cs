
using UnityEngine; // This must be at the top!

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // Search for the PlayerHealth script on the object we hit
        PlayerHealth healthScript = collision.collider.GetComponent<PlayerHealth>();

        if (healthScript != null) 
        {
            // We pass the player's current health as the damage amount
            // This instantly resets it to 0 and triggers your UI/FX logic
            healthScript.TakeDamage(healthScript.health);
        }
    }
}