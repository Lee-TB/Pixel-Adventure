using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float timeToFall = 0.5f;

    private const string IS_OFF = "IsOff";

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Invoke("TurnOff", timeToFall);
        }
    }

    private void TurnOff()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetTrigger(IS_OFF);
    }
}
