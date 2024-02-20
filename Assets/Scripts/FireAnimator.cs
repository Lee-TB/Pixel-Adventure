using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimator : MonoBehaviour
{
    [SerializeField] private Fire fire;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (fire.CurrentState)
        {
            case Fire.State.Off:
                animator.SetTrigger("IsOff");
                break;
            case Fire.State.Hit:
                animator.SetTrigger("IsHit");
                break;
            case Fire.State.On:
                animator.SetTrigger("IsOn");
                break;
        }
    }
}
