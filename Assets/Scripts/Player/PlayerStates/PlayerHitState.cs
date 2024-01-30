using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    private float hitTime;

    public PlayerHitState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.Hit;
        playerController.KnockBack();
        hitTime = 1f;
    }

    public override void ExitState()
    {
        playerController.TriggerType = PlayerController.AnimationTriggerType.Idle;
    }

    public override void FrameUpdate()
    {
        if (hitTime < 0f)
        {
            playerStateMachine.ChangeState(playerController.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        hitTime -= Time.fixedDeltaTime;
    }
}
