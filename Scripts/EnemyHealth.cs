using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        /* play death animation if you have one
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }
        */

        // disable instead of destroy (for pooling)
        gameObject.SetActive(false);
    }
}
