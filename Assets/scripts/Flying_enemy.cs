using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5f;
    public float stopDistance = 2f; // Stop before hitting the player (so it can shoot/bite)
    public float aggroRange = 15f;  // How close player must be to start chasing

    [Header("References")]
    public Transform Player;        // We will auto-find this

    void Start()
    {
        // Auto-find the player if not assigned manually
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
        }
    }

    void Update()
    {
       
        if (Player == null) return;

      
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer < aggroRange)
        {   
            // If player is to the Right
            if (Player.position.x > transform.position.x)
            {
                // Point normally (Right)
                transform.localScale = new Vector3(1, 1, 1);
            }
            // If player is to the Left
            else
            {
                // Flip the sprite (Left)
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (distanceToPlayer > stopDistance)
            {
              
                transform.position = Vector3.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}