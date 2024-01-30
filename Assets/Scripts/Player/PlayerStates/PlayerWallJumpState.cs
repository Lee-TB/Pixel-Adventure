using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.Jump;
        playerController.WallJump(playerController.GetJumpPower() * 0.7f);
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
    }

    public override void PhysicsUpdate()
    {
    }
}
