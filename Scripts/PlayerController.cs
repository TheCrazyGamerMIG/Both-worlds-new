using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [Header("Player Info")]
    public string playerName = "Player";
    public int maxHealth = 100;
    private int currentHealth;
    private int kills;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool canDoubleJump;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public int baseDamage = 1;                 // base attack damage
    private float attackMultiplier = 1f;       // can be boosted by powerups
    public Transform attackPoint;              // point from which attacks are detected
    public float attackRange = 0.5f;           // attack radius
    public LayerMask enemyLayer;               // layers that can be attacked
    public Animator anim;                       // attack animation

    [Header("Respawn Settings")]
    public Transform respawnPoint;             // where the player respawns
    public float respawnDelay = 2f;            // delay before respawn

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Check if grounded for jumping logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // ---------------- Input System ----------------
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = false; // reset double jump
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false; // consume double jump
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // ---------------- Attack System ----------------
    public void Attack()
    {
        anim.SetTrigger("Attack");
        DealDamage();
    }

    private void DealDamage()
    {
        int damage = Mathf.RoundToInt(baseDamage * attackMultiplier);

        // Detect all enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if enemy is a PlayerController
            var playerEnemy = enemy.GetComponent<PlayerController>();
            if (playerEnemy != null)
            {
                playerEnemy.TakeDamage(damage);
                continue;
            }

            // Check if enemy has EnemyHealth
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        Debug.Log($"{playerName} attacked for {damage} damage!");
    }

    public void SetAttackBoost(float multiplier, float duration)
    {
        StartCoroutine(AttackBoostRoutine(multiplier, duration));
    }

    private IEnumerator AttackBoostRoutine(float multiplier, float duration)
    {
        attackMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        attackMultiplier = 1f; // reset after duration
    }

    // ---------------- Health & Respawn ----------------
    public int GetHealth() => currentHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"{playerName} took {damage} damage. Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"{playerName} healed {amount}. Health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{playerName} died!");
        gameObject.SetActive(false);
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        gameObject.SetActive(true);
        Debug.Log($"{playerName} respawned!");
    }

    // ---------------- Power-up Support ----------------
    public void EnableDoubleJump(bool enabled)
    {
        canDoubleJump = enabled;
    }

    public void SetSpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeed /= multiplier;
    }

    // ---------------- IPlayerController Implementation ----------------
    public string GetPlayerName() => playerName;

    public void AddKill() => kills++;

    public int GetKills() => kills;

    // ---------------- Debugging ----------------
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
