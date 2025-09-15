using UnityEngine;

public class SmogTravel : MonoBehaviour
{
    public float lifeSpan = 1000f;
    public bool moveUp, moveDown, moveLeft, moveRight;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveUp)
        {
            rb.linearVelocity = new Vector2(0,1f);
        }
        if (moveDown)
        {
            rb.linearVelocity = new Vector2(0, -1f);
        }
        if (moveLeft)
        {
            rb.linearVelocity = new Vector2(-1f, 0);
        }
        if (moveRight)
        {
            rb.linearVelocity = new Vector2(1f, 0);
        }
        lifeSpan--;
        if (lifeSpan<=0)
        {
            Destroy(gameObject);
        }
    }
}
