using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private Rigidbody2D rb;
    private float baseGravityScale;

    [SerializeField] private float speed = 8f;

    //JUMP
    [SerializeField] private float jumpingForce = 16f;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool canDoubleJump;

    //DASH
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 0f;

    //GROUND SLAM
    [SerializeField] private bool isForceDown;
    [SerializeField] private float slamForce = 15f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && !Input.GetKey(KeyCode.Space))
        {
            canDoubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded() || canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingForce);

                canDoubleJump = !canDoubleJump;
            }
                
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) && !IsGrounded())
        {
            isForceDown = true;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        float velocityX = !isForceDown ? horizontal * speed : 0;

        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if (isForceDown)
        {
            rb.gravityScale = slamForce;
        }

        if (IsGrounded())
        {
            isForceDown = false;
            rb.gravityScale = baseGravityScale;
        }
    }

    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        canDash = true;
        return isGrounded;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
