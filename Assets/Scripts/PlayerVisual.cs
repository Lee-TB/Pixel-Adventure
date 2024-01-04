using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IS_IDLE = "IsIdle";
    private const string IS_RUN = "IsRun";
    private const string IS_JUMP = "IsJump";
    private const string IS_FALL = "IsFall";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (player.GetState())
        {
            case Player.State.Idle:
                animator.SetTrigger(IS_IDLE);
                break;
            case Player.State.Run:
                animator.SetTrigger(IS_RUN);
                break;
            case Player.State.Jump:
                animator.SetTrigger(IS_JUMP);
                break;
            case Player.State.DoubleJump:
                break;
            case Player.State.WallJump:
                break;
            case Player.State.Fall:
                animator.SetTrigger(IS_FALL);
                break;
            case Player.State.Hit:
                break;
        }
    }
}
