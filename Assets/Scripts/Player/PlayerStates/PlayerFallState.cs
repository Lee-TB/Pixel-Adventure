using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.Fall;
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        if (playerController.IsGrounded)
        {
            playerStateMachine.ChangeState(playerController.IdleState);
        }

        if (Input.GetButtonDown("Jump") && playerController.NumberOfJumps > 0)
        {
            playerStateMachine.ChangeState(playerController.DoubleJumpState);
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
