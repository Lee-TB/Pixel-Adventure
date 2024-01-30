using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        playerController.IsWalled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerController.IsWalled = false;
    }
}
