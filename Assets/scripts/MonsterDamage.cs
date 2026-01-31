using UnityEngine;

public class monsterdamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth playerHealth;
    private Animator enemyAnim; // 1. Variable for the MONSTER'S animator

    void Start()
    {
        // 2. Find the Animator attached to this monster
        enemyAnim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 3. Play the monster's attack animation
            if (enemyAnim != null)
            {
                enemyAnim.SetTrigger("Attack");
            }

            // Deal damage to the player
            playerHealth.TakeDamage(damage);
        }
    }
}