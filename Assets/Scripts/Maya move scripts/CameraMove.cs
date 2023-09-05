using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMove : MonoBehaviour
{
    [Tooltip("Object to follow")]
    public Transform follow;
    [Tooltip("Max speed")]
    public float speed;
    [Tooltip("Time to move to a new location")]
    public float cinematicMoveDuration;

    // Variables for keeping track of how and where the camera should move
    private CameraMode mode = CameraMode.follow;
    private Vector3 offset;
    private Vector3 target;
    private float currentSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // We are not right on top of our target - start by calculating an offset
        offset = transform.position - follow.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Choose between two modes of movement - only the follow mode does anything in the Update function
        switch(mode) {
            case CameraMode.follow:
                Follow();
                break;
            case CameraMode.cinematic:
                break;
        }
    }

    // Follow the player
    private void Follow()
    {
        // We do not move right on top of target, but to an offset position
        target = follow.position + offset;

        // Calculate speed based on distance to target - this makes the camera
        // stop smoothly when approaching its target position.
        float distance = (target - transform.position).magnitude;
        currentSpeed = Mathf.Lerp(0, speed, distance / speed);

        // Move smoothly towards target
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    // Swap to cinematic mode
    public void Cinematic(Vector3 targetPosition, float stayDuration)
    {
        mode = CameraMode.cinematic;

        // DoCinematic will run "in parallel" with the normal Update loop
        StartCoroutine(DoCinematic(targetPosition + offset, stayDuration));
    }

    // Looks at a different area for the given duration
    IEnumerator DoCinematic(Vector3 targetPosition, float stayDuration)
    {
        // Deactivate robot
        RobotMove robot = follow.gameObject.GetComponent<RobotMove>();
        robot.Deactivate();

        // Move to position
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while(timeElapsed < cinematicMoveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / cinematicMoveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // Stay a while
        yield return new WaitForSeconds(stayDuration);

        // Move back to player
        timeElapsed = 0;
        startPosition = transform.position;
        targetPosition = follow.position + offset;
        while (timeElapsed < cinematicMoveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / cinematicMoveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // Reactivate player + normal camera mode
        mode = CameraMode.follow;
        robot.Activate();
    }

    // Used for specifying camera behavior
    enum CameraMode
    {
        follow,
        cinematic
    }
}
