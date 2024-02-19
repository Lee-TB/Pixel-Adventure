using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private const string IS_JUMP = "IsJump";
    [SerializeField] private float jumpPower = 15f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.Jump(jumpPower);
            animator.SetTrigger(IS_JUMP);
        }
    }
}
