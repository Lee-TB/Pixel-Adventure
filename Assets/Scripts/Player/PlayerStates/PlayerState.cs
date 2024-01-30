public abstract class PlayerState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;
    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine)
    {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
}
