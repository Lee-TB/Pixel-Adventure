using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private LayerMask groundLayer;

    private float jumpBufferTimer;
    private float jumpBufferTimerMax = 0.2f;
    private float coyoteTimer;
    private float coyoteTimerMax = 0.2f;

    public enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        DoubleJump,
        WallJump,
        Hit,
    }
    private State state;

    private bool isFacingRight = true;
    private float horizontal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        HandleMovement();
        Jumping();
    }

    private void HandleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal != 0f)
        {
            state = State.Run;
        }
        else
        {
            state = State.Idle;
        }

        // Flip
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Jumping()
    {
        if (IsGrounded())
        {
            coyoteTimer = coyoteTimerMax;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTimerMax;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }


        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }


        if (!IsGrounded())
        {
            if (rb.velocity.y > 0f)
                state = State.Jump;
            else if (rb.velocity.y < 0f)
            {
                state = State.Fall;
            }
        }
    }

    public State GetState()
    {
        return state;
    }

    public bool IsGrounded()
    {
        float distance = 0.2f;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, distance, groundLayer);

        Color color;
        if (raycastHit2D.collider != null)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.max, Vector3.down * (boxCollider2D.bounds.size.y + distance), color);
        return raycastHit2D.collider != null;
    }
}