using System;
using System.Collections;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    [SerializeField] private int maxHP = 2;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float divePower = 5f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionHeight = 5f;

    private const string IS_HIT = "IsHit";
    private const string IS_DISAPPEAR = "IsDisappear";
    private Animator animator;
    private Behaviour[] behaviourArray;
    private SpriteRenderer spriteRenderer;
    private int hp;
    private void Awake()
    {
        HideBreakParts();
        animator = GetComponent<Animator>();
        behaviourArray = GetComponents<Behaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            hp--;
            animator.SetTrigger(IS_HIT);

            if (player.IsFall()) player.Jump(jumpPower);
            else player.Dive(divePower);

            StartCoroutine(Delay(() =>
            {
                if (hp == 0)
                {
                    DisableAllComponents();
                    ShowBreakParts();
                    Explosions();
                    DisappearBreakParts();
                }
            }, 0.04f));
        }
    }

    private IEnumerator Delay(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    private void DisableAllComponents()
    {
        spriteRenderer.enabled = false;
        foreach (var behaviour in behaviourArray)
        {
            behaviour.enabled = false;
        }
    }

    private void ShowBreakParts()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void HideBreakParts()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void DisappearBreakParts()
    {
        foreach (Transform child in transform)
        {
            Animator animator = child.GetComponent<Animator>();
            StartCoroutine(Delay(() => animator.SetTrigger(IS_DISAPPEAR), 5f));
        }
    }

    private void Explosions()
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(UnityEngine.Random.Range(-explosionRadius, explosionRadius), explosionHeight);
        }
    }
}
