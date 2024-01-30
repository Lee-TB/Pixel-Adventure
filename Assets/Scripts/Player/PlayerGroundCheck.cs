using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        playerController.IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerController.IsGrounded = false;
    }
}
