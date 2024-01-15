using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private StartPoint startPoint = StartPoint.PointA;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopTimerMax = 2f;
    [SerializeField] private Mode mode = Mode.Auto;

    public bool IsPlayerStayOn { get; set; }

    private Vector2 targetPosition;
    private float stopTimer = 0f;
    private bool isCalledStop = false;

    public State state { get; private set; }
    public enum State
    {
        Off,
        ToRight,
        ToLeft,
    }
    public enum StartPoint
    {
        PointA,
        PointB,
        Middle,
    }
    public enum Mode
    {
        Auto,
        Stay,
    }

    private void Start()
    {
        InitiateStartPoint();
    }

    private void Update()
    {
        if (mode == Mode.Auto)
        {
            HandleAutoMovement();
        }
        else
        {
            HandlePlayerStayMovement();
        }
    }

    private void HandleAutoMovement()
    {
        if (stopTimer <= 0f)
        {
            platform.position = Vector2.MoveTowards(platform.position, targetPosition, Time.deltaTime * moveSpeed);
            UpdateState();
        }
        else
        {
            stopTimer -= Time.deltaTime;
        }

        if (Vector2.Distance(platform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            Stop();
        }

        if (Vector2.Distance(platform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            Stop();
        }

        if (Vector2.Distance(platform.position, pointA.position) > 0.1f && Vector2.Distance(platform.position, pointB.position) > 0.1f)
        {
            isCalledStop = false;
        }
    }

    private void HandlePlayerStayMovement()
    {
        platform.position = Vector2.MoveTowards(platform.position, targetPosition, Time.deltaTime * moveSpeed);

        if (Vector2.Distance(platform.position, pointA.position) > 0.1f && Vector2.Distance(platform.position, pointB.position) > 0.1f)
        {
            UpdateState();
        }
        else
        {
            state = State.Off;
        }

        if (IsPlayerStayOn)
        {
            if (startPoint == StartPoint.PointA)
            {
                targetPosition = pointB.position;
            }
            if (startPoint == StartPoint.PointB)
            {
                targetPosition = pointA.position;
            }
            if (startPoint == StartPoint.Middle)
            {
                targetPosition = pointB.position;
            }
        }
        else
        {
            if (startPoint == StartPoint.PointA)
            {
                targetPosition = pointA.position;
            }
            if (startPoint == StartPoint.PointB)
            {
                targetPosition = pointB.position;
            }
            if (startPoint == StartPoint.Middle)
            {
                targetPosition = (pointA.position + pointB.position) / 2;
            }
        }

    }

    private void InitiateStartPoint()
    {
        switch (startPoint)
        {
            case StartPoint.PointA:
                platform.position = pointA.position;
                targetPosition = pointB.position;
                break;
            case StartPoint.PointB:
                platform.position = pointB.position;
                targetPosition = pointA.position;
                break;
            case StartPoint.Middle:
                platform.position = (pointA.position + pointB.position) / 2;
                targetPosition = pointB.position;
                break;
        }
    }

    private void UpdateState()
    {
        if (targetPosition.x > transform.position.x)
        {
            state = State.ToRight;
        }
        else if (targetPosition.x < transform.position.x)
        {
            state = State.ToLeft;
        }
        else if (targetPosition.x == transform.position.x)
        {
            if (targetPosition.y > transform.position.y)
            {
                state = State.ToRight;
            }
            else if (targetPosition.y < transform.position.y)
            {
                state = State.ToLeft;
            }
        }
    }

    public void Stop()
    {
        if (!isCalledStop)
        {
            stopTimer = stopTimerMax;
            isCalledStop = true;
            state = State.Off;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
