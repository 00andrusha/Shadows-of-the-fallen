using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 1; //sets the players movement speed on the ground
    [Space(5)]

    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f; //sets how high the player can jump

    private float jumpBufferCounter = 0; //stores the jump button input
    [SerializeField] private float jumpBufferFrames; //sets the max amount of frames the jump buffer input is stored

    private float coyoteTimeCounter = 0; //stores the Grounded() bool
    [SerializeField] private float coyoteTime; //sets the max amount of frames the Grounded() bool is stored

    private int airJumpCounter = 0; //keeps track of how many times the player has jumped in the air
    [SerializeField] private int maxAirJumps; //the max no. of air jumps

    private float gravity; //stores the gravity scale at start
    [Space(5)]

    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint; //point at which ground check happens
    [SerializeField] private float groundCheckY = 0.2f; //how far down from ground chekc point is Grounded() checked
    [SerializeField] private float groundCheckX = 0.5f; //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private LayerMask whatIsGround; //sets the ground layer
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed; //speed of the dash
    [SerializeField] private float dashTime; //amount of time spent dashing
    [SerializeField] private float dashCooldown; //amount of time between dashes
    [SerializeField] GameObject dashEffect;
    private bool canDash = true, dashed;
    [Space(5)]

    [Header("Combat Settings")]
    [SerializeField] private Transform attackPoint; // The position where the attack circle appears
    [SerializeField] private float attackRange = 0.5f; // The radius of the attack circle
    [SerializeField] private int damage = 40; // Damage dealt to enemies
    [SerializeField] private LayerMask enemyLayers; // Which layers count as enemies
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private AudioSource swordAttackSound; // <--- NEW: Added AudioSource for the swing
    private float timeSinceAttack;

    private PlayerStateList pState;
    private Animator anim;
    private Rigidbody2D rb;

    //Input Variables
    private float xAxis;
    private bool attack = false;


    //creates a singleton of the PlayerController
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing) return;

        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        attack = Input.GetMouseButtonDown(0);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // ---------------------------------------------------------
    // INTEGRATED ATTACK LOGIC START
    // ---------------------------------------------------------
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;

        // Check input and cooldown
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;

            // 1. Play Animation
            anim.SetTrigger("Attacking");

            // 2. Play Sword Sound (New)
            if (swordAttackSound != null)
            {
                swordAttackSound.pitch = Random.Range(0.9f, 1.1f); // Added a slight pitch variation
                swordAttackSound.PlayOneShot(swordAttackSound.clip);
            }

            // 3. Safety Check
            if (attackPoint == null)
            {
                Debug.LogError("Attack Point is missing! Please assign it in the Inspector.");
                return;
            }

            // 4. Detect Enemies
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

            // 5. Apply Damage
            foreach (Collider2D hit in hitColliders)
            {
                EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

                if (enemy != null && !hitEnemies.Contains(enemy.gameObject))
                {
                    hitEnemies.Add(enemy.gameObject);
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    // Visualize the Attack Range in Editor
    void OnDrawGizmosSelected()
    {
        // Draw Ground Checks (Existing)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckY);
        Gizmos.DrawLine(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), groundCheckPoint.position + new Vector3(groundCheckX, 0, 0) + Vector3.down * groundCheckY);
        Gizmos.DrawLine(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0) + Vector3.down * groundCheckY);

        // Draw Attack Range (New)
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
    // ---------------------------------------------------------
    // INTEGRATED ATTACK LOGIC END
    // ---------------------------------------------------------

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
                pState.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            }
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            pState.jumping = false;
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter = jumpBufferCounter - Time.deltaTime * 10;
        }
    }
}