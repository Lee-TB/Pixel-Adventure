using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    public event EventHandler OnBoxBreaked;

    [SerializeField] private Transform boxVisualTransform;
    [SerializeField] private int maxHP = 3;

    private Animator boxVisualAnimator;
    private int hp;
    private void Awake()
    {
        hp = maxHP;
        boxVisualAnimator = boxVisualTransform.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out Player player))
        {
            player.Jump();
            boxVisualAnimator.SetTrigger("IsHit");
            hp -= 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (hp == 0)
        {   // when Box is breaked
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
