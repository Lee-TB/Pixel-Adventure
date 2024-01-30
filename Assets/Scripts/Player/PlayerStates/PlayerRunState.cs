using UnityEngine;

public class PlayerRunState : PlayerState
{
  public PlayerRunState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
  {
  }

  public override void EnterState()
  {
    playerController.TriggerType = PlayerController.AnimationTriggerType.Run;
  }

  public override void ExitState()
  {
  }

  public override void FrameUpdate()
  {
    playerController.ReadyToJump();

    if (playerController.Horizontal == 0f)
    {
      playerStateMachine.ChangeState(playerController.IdleState);
    }

    if (playerController.GetJumpBufferTimer() > 0f && playerController.GetCoyoteTimer() > 0f)
    {
      playerStateMachine.ChangeState(playerController.JumpState);
    }

    if (playerController.IsFall())
    {
      playerStateMachine.ChangeState(playerController.FallState);
    }
  }

  public override void PhysicsUpdate()
  {
    playerController.Move();
    playerController.Flip();
  }
}
