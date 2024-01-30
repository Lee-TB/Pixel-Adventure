using UnityEngine;

public class PlayerIdleState : PlayerState
{
  public PlayerIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
  {
  }

  public override void EnterState()
  {
    playerController.TriggerType = PlayerController.AnimationTriggerType.Idle;
  }

  public override void ExitState()
  {
  }

  public override void FrameUpdate()
  {
    playerController.ReadyToJump();

    if (playerController.Horizontal != 0f)
    {
      playerStateMachine.ChangeState(playerController.RunState);
    }

    if (playerController.GetJumpBufferTimer() > 0f && playerController.GetCoyoteTimer() > 0f)
    {
      playerStateMachine.ChangeState(playerController.JumpState);
    }
  }

  public override void PhysicsUpdate()
  {
  }
}
