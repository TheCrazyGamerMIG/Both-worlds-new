using UnityEngine;

public class DigParticle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float lifespan = 1f;
    public bool moveUp, moveLeft, moveRight, moveDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifespan--;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        if (moveUp)
        {
            rb.linearVelocity += Vector2.up * 1f;
        }
        if (moveLeft)
        {
            rb.linearVelocity += Vector2.left * 1f;
        }
        if (moveRight)
        {
            rb.linearVelocity += Vector2.right * 1f;
        }
        if (moveDown)
        {
            rb.linearVelocity += Vector2.down * 1f;
        }
    }
}
