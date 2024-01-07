using UnityEngine;

public class CollectedAnimationExit : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Collected"))
        {
            animator.gameObject.SetActive(false);
        }
    }
}
