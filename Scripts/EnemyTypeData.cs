using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeData", menuName = "Scriptable Objects/EnemyTypeData")]
public class EnemyTypeData : ScriptableObject
{
    [Header("General Settings")]
    public string enemyName;
    public int maxHealth;
    public float moveSpeed;
    public float patrolRange;

    [Header("Combat Settings")]
    public int attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float detectionRange;

    [Header("Visual Settings")]
    public Color enemyColor;

    [Header("Animation Settings")]
    [Tooltip("Assign an Animator Override Controller that customizes animations for this enemy type.")]
    public AnimatorOverrideController overrideAnimatorController;

    [Tooltip("Optional default sprite if this enemy has a unique look.")]
    public Sprite defaultSprite;
}
