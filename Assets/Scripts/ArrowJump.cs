using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowJump : MonoBehaviour
{
    private const string IS_HIT = "IsHit";
    [SerializeField] private float jumpPower = 15f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Jump(jumpPower);
            animator.SetTrigger(IS_HIT);
        }
    }

}
