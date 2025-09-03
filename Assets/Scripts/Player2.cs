using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private int playerHP = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by Input System (auto-assigned per player)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (jumpPressed && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        jumpPressed = false;

        if (playerHP <= 0)
        {
            Destroy(gameObject); //Or a proper action.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hazard")
        {
            playerHP -= 15;
            print("Damage taken! HP is " + playerHP + "/100");
        }
    }
}
