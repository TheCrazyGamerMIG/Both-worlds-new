using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;

    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isGrounded;

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Get the SpriteRenderer component on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float inputMagnitude = Mathf.Abs(moveInput.x);

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        // Jump
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    // Collision detection for ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Check if we're colliding with the top of the object (ground)
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f) // If the normal is pointing mostly upward
                {
                    isGrounded = true;
                    animator.SetBool("isJumping", !isGrounded);
                    break;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Continue checking for ground contact while staying on ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f)
                {
                    isGrounded = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // When leaving ground, set isGrounded to false
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    // Input System Events
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Flip the character based on the X movement input
        // Only flip if the player is actually providing input (not 0)
        if (moveInput.x != 0)
        {
            // If the input is positive (right), flipX should be false.
            // If the input is negative (left), flipX should be true.
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
        if (context.canceled)
            jumpPressed = false;
    }
}
