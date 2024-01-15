using UnityEngine;

public class MovingPlatformVisual : MonoBehaviour
{
    [SerializeField] MovingPlatform movingPlatform;
    private Animator animator;
    private const string IS_OFF = "IsOff";
    private const string IS_ON_LEFT = "IsOnLeft";
    private const string IS_ON_RIGHT = "IsOnRight";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (movingPlatform.state)
        {
            case MovingPlatform.State.ToLeft:
                animator.SetTrigger(IS_ON_LEFT);
                break;
            case MovingPlatform.State.ToRight:
                animator.SetTrigger(IS_ON_RIGHT);

                break;
            case MovingPlatform.State.Off:
                animator.SetTrigger(IS_OFF);
                break;
        }
    }
}
