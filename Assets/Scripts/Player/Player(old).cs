using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private int maxHealthPoints = 3;
    private int healthPoints;

    public enum AnimationState
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
    private AnimationState state;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private float jumpBufferTimer, jumpBufferTimerMax = 0.2f;
    private float coyoteTimer, coyoteTimerMax = 0.2f;
    private int jumpNumber, jumpNumberMax = 2;
    private bool isFacingRight = true;
    private float horizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        state = AnimationState.Idle;
        healthPoints = maxHealthPoints;
    }

    private void Update()
    {
        Debug.Log(state);
        HandleMovement();
        HandleJumping();
        HandleWallSliding();
        HandleWallJumping();
        if (Input.GetButtonUp("Jump"))
        {
            jumpNumber -= 1;
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void HandleMovement()
    {
        if (state != AnimationState.WallJump && state != AnimationState.WallSlide)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        // Flip
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        // Player State
        if (IsGrounded())
        {
            if (horizontal != 0f)
                state = AnimationState.Run;
            else
                state = AnimationState.Idle;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void HandleJumping()
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
                state = AnimationState.DoubleJump;
            }
        }

        // Player State
        if (!IsGrounded())
        {
            if (rb.velocity.y > 0f && state != AnimationState.DoubleJump && state != AnimationState.WallJump)
                state = AnimationState.Jump;
            else if (rb.velocity.y < 0f)
            {
                state = AnimationState.Fall;
            }
        }
    }

    private void HandleWallSliding()
    {
        if (IsFacingWall() && horizontal != 0f && !IsGrounded())
        {
            rb.velocity = new Vector2(0f, Mathf.Max(-wallSlidingSpeed, rb.velocity.y));
            state = AnimationState.WallSlide;
        }
    }

    private void HandleWallJumping()
    {
        if (state == AnimationState.WallSlide)
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

        Vector2 wallJumpDirection = isFacingRight ? Vector2.left : Vector2.right;

        if (state == AnimationState.WallSlide && coyoteTimer > 0f && jumpBufferTimer > 0f && jumpNumber > 0)
        {
            rb.velocity = new Vector2(jumpPower * 0.5f * wallJumpDirection.x, jumpPower);
            Flip();
            state = AnimationState.WallJump;
        }
    }

    public AnimationState GetState()
    {
        return state;
    }

    public void SetState(AnimationState state)
    {
        this.state = state;
    }

    public bool IsGrounded()
    {
        float distance = 0f;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y), boxCollider2D.bounds.extents, 0f, Vector2.down, distance, groundLayer);

        Color color = raycastHit2D.collider != null ? Color.green : Color.red;
        Debug.DrawRay(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y), Vector3.down * (boxCollider2D.bounds.extents.y + distance), color);

        return raycastHit2D.collider != null;
    }

    public bool IsFacingWall()
    {
        float distance = 0.1f;
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, direction, distance, wallLayer);

        Color color = raycastHit2D.collider != null ? Color.green : Color.red;
        Debug.DrawRay(new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.center.y), direction * (boxCollider2D.bounds.extents.x + distance), color);

        return raycastHit2D.collider != null;
    }

    public void Jump(float power)
    {
        rb.velocity = new Vector2(rb.velocity.x, power);
    }

    public void JumpByJumpBufferTimer()
    {
        jumpBufferTimer = jumpBufferTimerMax;
    }

    public void Dive(float power)
    {
        rb.velocity = new Vector2(rb.velocity.x, -power);
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public bool IsJumpOn()
    {
        return Mathf.Abs(rb.velocity.y) > wallSlidingSpeed + 1f;
    }

    public void Hit(int damage)
    {
        healthPoints -= damage;
        state = AnimationState.Hit;
        Vector2 direction = isFacingRight ? Vector2.left : Vector2.right;
        float power = 10f;
        rb.velocity = new Vector2(direction.x * 10f, power);
    }
}
