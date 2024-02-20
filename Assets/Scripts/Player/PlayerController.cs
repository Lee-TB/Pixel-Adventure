using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private float decreaseJumpPowerRate = 0.6f;
    [SerializeField] private int maxNumberOfJumps = 2;
    #endregion

    #region Private Fields
    private Rigidbody2D rb;
    private float coyoteTimer, coyoteTimerMax = 0.2f;
    private float jumpBufferTimer, jumpBufferTimerMax = 0.2f;
    private bool isFacingRight = true;
    private float immunityTime, immunityTimeMax = 2f;
    #endregion

    #region Public Properties
    public float Horizontal { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsWalled { get; set; } = false;
    public int NumberOfJumps { get; set; }
    public float MoveSpeedModifier { get; set; } = 1f;
    public float MoveSpeedModifierOrigin { get; set; } = 1f;
    public float SlideSpeedModifier { get; set; } = 1f;
    public float SlideSpeedModifierOrigin { get; set; } = 1f;
    public float HitTime { get; set; }
    public float HitTimeMax { get; set; } = 1f;
    #endregion

    #region  AnimationType
    public enum AnimationTriggerType
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
    public AnimationTriggerType TriggerType { get; set; }
    #endregion

    #region Player States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerDoubleJumpState DoubleJumpState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        DoubleJumpState = new PlayerDoubleJumpState(this, StateMachine);
        WallSlideState = new PlayerWallSlideState(this, StateMachine);
        WallJumpState = new PlayerWallJumpState(this, StateMachine);
        HitState = new PlayerHitState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;
        immunityTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void Move()
    {
        rb.velocity = new Vector2(Horizontal * moveSpeed * MoveSpeedModifier, rb.velocity.y);
    }

    public void Flip()
    {
        if (isFacingRight && rb.velocity.x < 0f || !isFacingRight && rb.velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180f, 0);
        }
    }

    public void Jump(float power)
    {
        rb.velocity = new Vector2(rb.velocity.x, power);
    }

    public void Dive(float power)
    {
        rb.velocity = new Vector2(rb.velocity.x, -power);
    }

    public void ReadyToJump()
    {
        if (IsGrounded)
        {
            coyoteTimer = coyoteTimerMax;
            NumberOfJumps = maxNumberOfJumps;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTimerMax;
        }
    }

    public void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed * SlideSpeedModifier);
    }

    public void WallJump(float jumpPower)
    {
        rb.velocity = isFacingRight ? new Vector2(-jumpPower, jumpPower)
                                    : new Vector2(jumpPower, jumpPower);
        Flip();
    }

    public void Hit(int damageAmount)
    {
        if (immunityTime <= 0f)
        {
            immunityTime = immunityTimeMax;
            StateMachine.ChangeState(HitState);
        }
    }

    public void KnockBack()
    {
        float power = 5f;
        rb.velocity = isFacingRight ? new Vector2(-power, power * 1.4f)
                                    : new Vector2(power, power * 1.4f);
    }

    public bool IsFall()
    {
        return rb.velocity.y < -0.1f;
    }

    public float GetCoyoteTimer()
    {
        return coyoteTimer;
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    public float GetJumpBufferTimer()
    {
        return jumpBufferTimer;
    }

    public float GetJumpPower()
    {
        return jumpPower;
    }

    public float GetDecreaseJumpPowerRate()
    {
        return decreaseJumpPowerRate;
    }
}
