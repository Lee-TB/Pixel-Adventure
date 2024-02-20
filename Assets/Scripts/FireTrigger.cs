using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    [SerializeField] private Fire fire;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fire.TurnOn();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && fire.IsFireOn())
        {
            playerController.Hit(1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fire.TurnOff();
        }
    }
}
