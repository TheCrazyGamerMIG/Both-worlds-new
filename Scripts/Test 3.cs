using UnityEngine;

public class Test3 : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float waitTimeAtPoints = 1f;

    private Vector3 targetPosition;
    private bool isMovingToB = true;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    void Start()
    {
        if (pointA != null && pointB != null)
        {
            targetPosition = pointB.position;
        }
        else
        {
            Debug.LogWarning("Patrol points not assigned! Enemy will not move.");
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoints)
            {
                isWaiting = false;
                waitTimer = 0f;
                SwitchTarget();
            }
            return;
        }

        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isWaiting = true;
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        FlipSpriteBasedOnDirection();
    }

    private void SwitchTarget()
    {
        isMovingToB = !isMovingToB;
        targetPosition = isMovingToB ? pointB.position : pointA.position;
    }

    private void FlipSpriteBasedOnDirection()
    {
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (targetPosition.x < transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
    */
}
