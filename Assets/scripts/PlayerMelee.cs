using UnityEngine;
using System.Collections.Generic;

public class PlayerMelee : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform attackPoint;

    [Header("Combat Settings")]
    public float attackRange = 0.5f;
    public int damage = 40;
    public float cooldownTime = 0.5f;
    public LayerMask enemyLayers;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
        {
            Attack();
            nextAttackTime = Time.time + cooldownTime;
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is missing! Please assign it in the Inspector.");
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        if (hitColliders.Length == 0)
        {
            Debug.Log("Attacked, but hit nothing. Check AttackRange or Layers.");
        }

        HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

        foreach (Collider2D hit in hitColliders)
        {
           
            Debug.Log("Hit collider: " + hit.name);

            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null && !hitEnemies.Contains(enemy.gameObject))
            {
                hitEnemies.Add(enemy.gameObject);
                Debug.Log($"Applied damage to {enemy.name}");
                enemy.TakeDamage(damage);
            }
            else if (enemy == null)
            {
                // DEBUG 4: We hit a collider, but it didn't have the script!
                Debug.LogWarning($"Hit {hit.name}, but could not find 'EnemyHealth' script on it or its parents.");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}