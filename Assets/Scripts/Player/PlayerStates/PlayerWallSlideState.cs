using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.WallSlide;
        playerController.NumberOfJumps = 2;
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        if (playerController.IsGrounded && playerController.Horizontal == 0f)
        {
            playerStateMachine.ChangeState(playerController.IdleState);
        }

        if (!playerController.IsWalled
            || playerController.Horizontal == 0f
            || playerController.Horizontal > 0f && !playerController.IsFacingRight()
            || playerController.Horizontal < 0f && playerController.IsFacingRight())
        {
            playerStateMachine.ChangeState(playerController.FallState);
        }

        if (Input.GetButtonDown("Jump"))
        {
            playerStateMachine.ChangeState(playerController.WallJumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (playerController.IsWalled)
        {
            playerController.WallSlide();
        }
    }
}
