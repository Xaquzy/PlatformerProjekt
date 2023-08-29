using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAutoMove : MonoBehaviour
{
    [Header("Path to move along")]
    [SerializeField] private Vector3[] points;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private bool moving = true;
    [SerializeField] private float pauseTime = 1f;

    // Keep track of next place to move to
    private int targetIndex;
    private Vector3 targetPosition;

    // Used for pausing at each point
    private float currentPauseTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // If we don't have at least 2 points to move
        // between, the whole thing doesn't work, so
        // don't even try.
        if (points.Length < 2) return;

        // Initialize position
        transform.position = points[0];
        targetIndex = 1;
        targetPosition = points[targetIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // If we don't have at least 2 points to move
        // between, the whole thing doesn't work, so
        // don't even try.
        if (points.Length < 2) return;

        // When we arrive at the target point...
        if(transform.position == targetPosition)
        {
            // Start the pause timer
            currentPauseTime = pauseTime;
            // Swap to next target point
            targetIndex = (targetIndex + 1) % points.Length;
            targetPosition = points[targetIndex];
        }

        // If we are currently paused...
        if(currentPauseTime > 0)
        {
            // Let the timer tick down
            currentPauseTime -= Time.deltaTime;
        }

        // If we're supposed to move (and not currently paused)...
        if (moving && currentPauseTime <= 0)
        {
            // Move towards target point
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // The following three functions are used to control
    // whether or not the platform moves at all. They are
    // public, which means a button can be configured to
    // stop, start, or toggle the platform's movement.
    public void Toggle()
    {
        moving = !moving;
    }

    public void Activate()
    {
        moving = true;
    }

    public void Deactivate()
    {
        moving = false;
    }
}
