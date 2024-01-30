using UnityEngine;

public class SpikesTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.StateMachine.ChangeState(player.HitState);
        }
    }
}
