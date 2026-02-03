using UnityEngine;

public class monsterdamage : MonoBehaviour
{
    public int damage = 1;
    public float attackRate = 1.0f; // How often (in seconds) the monster attacks
    private float nextAttackTime = 0f; // Timer tracker

    public PlayerHealth playerHealth;
    private Animator enemyAnim;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();

        if (playerHealth == null)
        {
            // Ideally, handle the case where the player might not exist yet
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
    }

    // CHANGE 1: Use OnCollisionStay2D instead of Enter
    // This runs every single frame the colliders are touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // CHANGE 2: The Cooldown Check
            // Only attack if current time is later than the allowed next attack time
            if (Time.time >= nextAttackTime)
            {
                PerformAttack();

                // Reset the timer
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void PerformAttack()
    {
        // 1. Play animation
        if (enemyAnim != null)
        {
            // Resetting trigger ensures it doesn't get stuck if called rapidly
            enemyAnim.ResetTrigger("Attack");
            enemyAnim.SetTrigger("Attack");
        }

        // 2. Deal damage
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}