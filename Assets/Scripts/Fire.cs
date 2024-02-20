using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float timeToFireOn = 1f;
    [SerializeField] private float timeToFireOff = 2f;
    public enum State
    {
        Off,
        On,
        Hit
    }

    public State CurrentState { get; set; }
    public bool IsPlayerStay { get; set; }

    private void Start()
    {
        CurrentState = State.Off;
        IsPlayerStay = false;
    }

    public void TurnOn()
    {
        StartCoroutine(TurnOnAfterWaiting());
    }

    public IEnumerator TurnOnAfterWaiting()
    {
        CurrentState = State.Hit;
        yield return new WaitForSeconds(timeToFireOn);
        CurrentState = State.On;
    }

    public void TurnOff()
    {
        StartCoroutine(TurnOffAfterWaiting());
    }

    public IEnumerator TurnOffAfterWaiting()
    {
        yield return new WaitForSeconds(timeToFireOff);
        CurrentState = State.Off;
    }

    public bool IsFireOn()
    {
        return CurrentState == State.On;
    }
}
