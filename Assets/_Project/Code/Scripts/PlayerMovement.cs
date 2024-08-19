using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private Rigidbody2D rb;
    private Animator anim;
    private float baseGravityScale;
    private GameObject weapon;

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
    [HideInInspector] public bool isDashing;
    [SerializeField] private float dashingForce = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 0f;
    [SerializeField] private BoxCollider2D hitBox;


    //GROUND SLAM
    [SerializeField] private bool isForceDown;
    [SerializeField] private float slamForce = 15f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseGravityScale = rb.gravityScale;
        hitBox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        weapon = transform.GetChild(0).gameObject;
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
            weapon.SetActive(false);
            anim.SetBool("isDashing", true);
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
            hitBox.isTrigger = true;
            rb.gravityScale = slamForce;
        }

        if (IsGrounded())
        {
            isForceDown = false;
            rb.gravityScale = baseGravityScale;
            hitBox.isTrigger = false;
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerMouseDiff = mousePosition - transform.position;

        if (isFacingRight && playerMouseDiff.x < 0f || !isFacingRight && playerMouseDiff.x > 0f)
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
        hitBox.isTrigger = true;
        rb.velocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        yield return new WaitForSeconds(dashingTime);
        anim.SetBool("isDashing", false);
        hitBox.isTrigger = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        weapon.SetActive(true);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {

        var enemy = collision.GetComponent<DamagableEntity>();
        if (enemy != null)
        {
            enemy.TakeDamage(dashDamage);
        }

    }*/
}
