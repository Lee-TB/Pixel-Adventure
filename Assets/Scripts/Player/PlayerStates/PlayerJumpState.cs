
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.Jump;
        playerController.Jump(playerController.GetJumpPower());
        playerController.NumberOfJumps -= 1;
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        if (playerController.IsFall())
        {
            playerStateMachine.ChangeState(playerController.FallState);
        }

        if (Input.GetButtonDown("Jump") && playerController.NumberOfJumps > 0)
        {
            playerStateMachine.ChangeState(playerController.DoubleJumpState);
        }

        if (Input.GetButtonUp("Jump"))
        {
            playerController.Jump(playerController.GetJumpPower() * playerController.GetDecreaseJumpPowerRate());
        }

        if (playerController.IsWalled && playerController.Horizontal != 0f)
        {
            playerStateMachine.ChangeState(playerController.WallSlideState);
        }
    }

    public override void PhysicsUpdate()
    {
        playerController.Move();
        playerController.Flip();
    }
}
