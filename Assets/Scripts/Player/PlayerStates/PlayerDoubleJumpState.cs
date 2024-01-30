using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerState
{
    public PlayerDoubleJumpState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.DoubleJump;
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
