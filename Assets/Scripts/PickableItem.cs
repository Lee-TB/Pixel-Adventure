using UnityEngine;

public class PickableItem : MonoBehaviour
{
    private const string IS_COLLECTED = "IsCollected";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger(IS_COLLECTED);
        }
    }
}
