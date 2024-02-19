using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudTerrainTrigger : MonoBehaviour
{
    private float slower = 0.01f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        BoggedDown(other);
        // StartCoroutine(Delay(() => s, 0.1f));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.MoveSpeedModifier = playerController.MoveSpeedModifierOrigin;
            playerController.SlideSpeedModifier = playerController.MoveSpeedModifierOrigin;
            Debug.Log("player out mud: " + playerController.MoveSpeedModifier);
        }
    }

    private void BoggedDown(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.MoveSpeedModifier *= slower;
            playerController.SlideSpeedModifier *= slower;
            Debug.Log("player in mud: " + playerController.MoveSpeedModifier);
        }
    }

    private IEnumerator Delay(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
