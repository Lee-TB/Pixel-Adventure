using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float wallSlidingSpeed = 2f;

    private float jumpBufferTimer;
    private float jumpBufferTimerMax = 0.2f;
    private float coyoteTimer;
    private float coyoteTimerMax = 0.2f;
    private int jumpNumberMax = 2;
    private int jumpNumber;

    public enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        DoubleJump,
        WallSlide,
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
        Movement();
        Jumping();
        WallSliding();
        WallJumping();
        if (Input.GetButtonUp("Jump"))
        {
            jumpNumber -= 1;

        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        if (state != State.WallJump && state != State.WallSlide)
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        // Flip
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        // Player State
        if (IsGrounded())
        {
            if (horizontal != 0f)
                state = State.Run;
            else
                state = State.Idle;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void Jumping()
    {
        if (IsGrounded())
        {
            coyoteTimer = coyoteTimerMax;
            jumpNumber = jumpNumberMax;
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

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f); // Decrease jump power when quick jump
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }

        // Ground jumping
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        // Double jumping
        if (jumpNumber > 0 && jumpBufferTimer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);

            if (jumpNumber != jumpNumberMax)
            {
                state = State.DoubleJump;
            }
        }

        // Player State
        if (!IsGrounded())
        {
            if (rb.velocity.y > 0f && state != State.DoubleJump && state != State.WallJump)
                state = State.Jump;
            else if (rb.velocity.y < 0f)
            {
                state = State.Fall;
            }
        }
    }

    private void WallSliding()
    {
        if (IsFacingWall() && horizontal != 0f && !IsGrounded())
        {
            rb.velocity = new Vector2(0f, Mathf.Max(-wallSlidingSpeed, rb.velocity.y));
            state = State.WallSlide;
        }
    }

    private void WallJumping()
    {
        if (state == State.WallSlide)
        {
            coyoteTimer = coyoteTimerMax;
            jumpNumber = jumpNumberMax;
        }
        else
            coyoteTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTimerMax;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }

        Vector2 wallJumpDirection;
        if (isFacingRight)
            wallJumpDirection = Vector2.left;
        else
            wallJumpDirection = Vector2.right;

        if (state == State.WallSlide && coyoteTimer > 0f && jumpBufferTimer > 0f && jumpNumber > 0)
        {
            rb.velocity = new Vector2(jumpPower * 0.5f * wallJumpDirection.x, jumpPower);
            Flip();
            state = State.WallJump;
        }
    }

    public State GetState()
    {
        return state;
    }

    public bool IsGrounded()
    {
        float distance = 0f;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y), boxCollider2D.bounds.extents, 0f, Vector2.down, distance, groundLayer);

        Color color;
        if (raycastHit2D.collider != null)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }
        Debug.DrawRay(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y), Vector3.down * (boxCollider2D.bounds.extents.y + distance), color);

        return raycastHit2D.collider != null;
    }

    public bool IsFacingWall()
    {
        float distance = 0.1f;
        Vector2 direction;

        if (isFacingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, direction, distance, wallLayer);

        Color color;
        if (raycastHit2D.collider != null)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }

        Debug.DrawRay(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.center.y), direction * (boxCollider2D.bounds.extents.x + distance), color);
        return raycastHit2D.collider != null;
    }
}
