using UnityEngine;

public class MovingPlatformPlayerStayTrigger : MonoBehaviour
{
    [SerializeField] MovingPlatform movingPlatform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.transform.SetParent(transform);
            movingPlatform.IsPlayerStayOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.transform.SetParent(null);
            movingPlatform.IsPlayerStayOn = false;
        }
    }
}
