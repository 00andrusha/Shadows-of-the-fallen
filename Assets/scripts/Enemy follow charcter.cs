using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed = 3f;
    public float stoppingDistance = 0.5f; // Stop right before touching (prevents glitching)
    public float aggroRange = 10f; // How close player must be to start chase

    [Header("Settings")]
    public bool isGhost = false; // Check this if enemy can fly through walls

    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 1. AUTO-FIND PLAYER
        // This is crucial for procedural levels. It looks for the tag "Player".
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Safety check: if player died or isn't found, do nothing
        if (player == null) return;

        // 2. CHECK DISTANCE
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < aggroRange && distanceToPlayer > stoppingDistance)
        {
            ChasePlayer();
            FlipSprite();
        }
    }

    void ChasePlayer()
    {
        if (isGhost)
        {
            // FLYING MOVEMENT (Moves strictly towards player X and Y)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // WALKING MOVEMENT (Only moves on X axis, stays on floor)
            // We create a target position that matches the Player's X, but keeps Enemy's own Y.
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    void FlipSprite()
    {
        if (spriteRenderer == null) return;

        // If player is to the RIGHT, face RIGHT (flipX = false usually, depends on your art)
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        // If player is to the LEFT, face LEFT
        else if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    // VISUAL DEBUG: Draw red circle in Editor to see Aggro Range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}