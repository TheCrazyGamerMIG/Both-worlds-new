using UnityEngine;
using System;

/// <summary>
/// Controls enemy behavior: patrolling between assigned points,
/// detecting players, chasing, attacking, and handling death.
/// Attach to enemy prefab with SpriteRenderer, Animator, and Collider2D.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Enemy Data")]
    [SerializeField] private EnemyTypeData typeData; // restored field for inspector
    private int currentHealth;

    [Header("Patrol Settings")]
    private Transform[] patrolPoints;
    private Transform currentTarget;

    [Header("Player Detection")]
    private Transform targetPlayer;
    private float lastAttackTime;

    [Header("Internal")]
    private bool isDead;

    // Event for GameManager to subscribe when enemy dies
    public event Action<string> OnEnemyDeath;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Ensure proper sorting/layer
        spriteRenderer.sortingLayerName = "Characters";
        spriteRenderer.sortingOrder = 2;

        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }

    private void Start()
    {
        if (typeData == null)
        {
            Debug.LogError($"{gameObject.name} has no EnemyTypeData assigned!");
            return;
        }

        if (patrolPoints != null && patrolPoints.Length >= 2)
            currentTarget = patrolPoints[0];
        else
            Debug.LogWarning($"{gameObject.name} has insufficient patrol points assigned.");
    }

    private void Update()
    {
        if (isDead || typeData == null) return;

        DetectPlayer();

        if (targetPlayer != null)
        {
            HandleChaseAndAttack();
        }
        else
        {
            HandlePatrol();
        }
    }

    /// <summary>
    /// Assigns type data to the enemy.
    /// Must be called before Update runs to avoid null issues.
    /// </summary>
    public void SetEnemyTypeData(EnemyTypeData data)
    {
        typeData = data;
        currentHealth = typeData.maxHealth;

        if (spriteRenderer != null)
            spriteRenderer.color = typeData.enemyColor;

        if (animator != null && typeData.overrideAnimatorController != null)
            animator.runtimeAnimatorController = typeData.overrideAnimatorController;
    }

    /// <summary>
    /// Assign patrol points to this enemy.
    /// </summary>
    public void AssignPatrolPoints(Transform[] points)
    {
        if (points == null || points.Length < 2)
        {
            Debug.LogWarning("Enemy needs at least 2 patrol points.");
            return;
        }

        patrolPoints = points;
        currentTarget = patrolPoints[0];
    }

    private void HandlePatrol()
    {
        if (patrolPoints == null || patrolPoints.Length < 2 || currentTarget == null) return;

        float step = typeData.moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, step);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.2f)
        {
            int nextIndex = (Array.IndexOf(patrolPoints, currentTarget) + 1) % patrolPoints.Length;
            currentTarget = patrolPoints[nextIndex];
        }

        animator.SetBool("IsWalking", true);
    }

    private void DetectPlayer()
    {
        targetPlayer = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, typeData.detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                targetPlayer = hit.transform;
                break;
            }
        }
    }

    private void HandleChaseAndAttack()
    {
        if (targetPlayer == null) return;

        float distance = Vector2.Distance(transform.position, targetPlayer.position);

        if (distance > typeData.attackRange)
        {
            float step = typeData.moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, step);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);

            if (Time.time > lastAttackTime + typeData.attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                // TODO: Deal damage to player
                targetPlayer.GetComponent<IPlayerController>()?.TakeDamage(typeData.attackDamage);
            }
        }
    }

    /// <summary>
    /// Apply damage to the enemy. Pass the playerName that dealt it.
    /// </summary>
    public void TakeDamage(int damage, string playerName)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Injured");

        if (currentHealth <= 0)
        {
            Die(playerName);
        }
    }

    private void Die(string killerName)
    {
        isDead = true;
        animator.SetTrigger("Death");

        OnEnemyDeath?.Invoke(killerName);

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        if (typeData == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, typeData.detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, typeData.attackRange);
    }
}
