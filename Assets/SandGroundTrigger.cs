using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandGroundTrigger : MonoBehaviour
{
    private float previousMoveSpeedModifier;
    private float previousSlideSpeedModifier;
    private float slower = 0.5f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            previousMoveSpeedModifier = playerController.MoveSpeedModifier;
            previousSlideSpeedModifier = playerController.SlideSpeedModifier;

            playerController.MoveSpeedModifier *= slower;
            playerController.SlideSpeedModifier *= slower;
            Debug.Log("player slow: " + playerController.MoveSpeedModifier);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.MoveSpeedModifier = previousMoveSpeedModifier;
            playerController.SlideSpeedModifier = previousSlideSpeedModifier;
            Debug.Log("player fast: " + playerController.MoveSpeedModifier);
        }
    }
}
