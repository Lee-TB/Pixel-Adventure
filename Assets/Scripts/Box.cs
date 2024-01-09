using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    public event EventHandler OnBoxBreaked;
    public event EventHandler OnBoxBouncing;
    public enum BoxType
    {
        Basic,
        Bouncing,
        Solid,
    }
    [SerializeField] private BoxType boxType;
    [SerializeField] private Transform boxVisualTransform;
    [SerializeField] private int maxHP = 1;

    private Animator boxVisualAnimator;
    private int hp;
    private bool isBreaked = false;

    private void Awake()
    {
        hp = maxHP;
        boxVisualAnimator = boxVisualTransform.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out Player player))
        {
            hp -= 1;
            boxVisualAnimator.SetTrigger("IsHit");

            float jumpPower = 10f;
            float divePower = 5f;

            switch (boxType)
            {
                case BoxType.Basic:
                    if (player.GetVelocity().y < 0f) player.Jump(jumpPower);
                    else if (player.GetVelocity().y > 0f) player.Dive(divePower);
                    break;

                case BoxType.Bouncing:
                    if (player.GetVelocity().y < 0f) player.Jump(jumpPower);
                    else if (player.GetVelocity().y > 0f) player.Dive(divePower);

                    OnBoxBouncing?.Invoke(this, EventArgs.Empty);
                    break;

                case BoxType.Solid:
                    break;
            }

            if (hp == 0 && !isBreaked)
            {
                boxVisualTransform.gameObject.SetActive(false);
                BoxCollider2D[] boxCollider2DArray = GetComponents<BoxCollider2D>();
                foreach (var boxCollider2D in boxCollider2DArray)
                {
                    Destroy(boxCollider2D);
                }

                OnBoxBreaked?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public BoxType GetBoxType()
    {
        return boxType;
    }
}
