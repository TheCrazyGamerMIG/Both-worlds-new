using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Components")]
    public Animator anim;
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }
    public void DealDamage()
    {
        // detect enemies in range
        // detect enemies in range at the moment of swing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            Debug.Log("Sword hit " + hitEnemies.Length + " enemy(s).");

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit enemy: " + enemy.name);
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Sword swing missed.");
        }
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
