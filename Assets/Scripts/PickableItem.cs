using UnityEngine;

public class PickableItem : MonoBehaviour
{
    private const string IS_COLLECTED = "IsCollected";
    [SerializeField] LayerMask pickableLayer;
    private Animator animator;
    private new Renderer renderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (IsPlayerPickup())
        {
            animator.SetTrigger(IS_COLLECTED);
        }
    }

    private bool IsPlayerPickup()
    {
        float radius = 0.25f;
        Vector2 direction = Vector2.zero;
        float distance = 0f;

        RaycastHit2D raycastHit2D = Physics2D.CircleCast(renderer.bounds.center, radius, direction, distance, pickableLayer);

        return raycastHit2D.collider != null;
    }
}
