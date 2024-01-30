using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IS_IDLE = "IsIdle";
    private const string IS_RUN = "IsRun";
    private const string IS_JUMP = "IsJump";
    private const string IS_FALL = "IsFall";
    private const string IS_DOUBLE_JUMP = "IsDoubleJump";
    private const string IS_WALL_SLIDE = "IsWallSlide";
    private const string IS_HIT = "IsHit";


    [SerializeField] private PlayerController playerController;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (playerController.TriggerType)
        {
            case PlayerController.AnimationTriggerType.Idle:
                animator.SetTrigger(IS_IDLE);
                break;
            case PlayerController.AnimationTriggerType.Run:
                animator.SetTrigger(IS_RUN);
                break;
            case PlayerController.AnimationTriggerType.Jump:
                animator.SetTrigger(IS_JUMP);
                break;
            case PlayerController.AnimationTriggerType.Fall:
                animator.SetTrigger(IS_FALL);
                break;
            case PlayerController.AnimationTriggerType.DoubleJump:
                animator.SetTrigger(IS_DOUBLE_JUMP);
                break;
            case PlayerController.AnimationTriggerType.WallSlide:
                animator.SetTrigger(IS_WALL_SLIDE);
                break;
            case PlayerController.AnimationTriggerType.Hit:
                animator.SetTrigger(IS_HIT);
                break;
        }
    }

}
