using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRotation : MonoBehaviour
{
    [Tooltip("Duration of rotation in seconds")]
    public float rotationDuration;

    // Rotation variables for swapping between two rotations.
    private Quaternion targetRotation;
    private Quaternion nextTargetRotation;
    private Quaternion initialRotation;

    // Keep track of whether we are mid-rotation
    private bool rotating = false;

    void Start()
    {
        // When game loads, whatever rotation we were at is fine.
        targetRotation = transform.localRotation;
        initialRotation = transform.localRotation;
        // 90 degree rotation when button is pressed
        nextTargetRotation = targetRotation * Quaternion.Euler(0f, 90f, 0f);

        // When activated, move camera here
        CinematicCamera cam = GetComponent<CinematicCamera>();
        if (cam)
        {
            cam.targetPosition = transform.position;
        }
    }

    // Rotates smoothly over a set period of time (rotationDuration)
    IEnumerator Rotate90()
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = transform.localRotation;
        while (timeElapsed < rotationDuration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = targetRotation;
        rotating = false;
    }

    // This function is run when someone presses the corresponding button
    [ContextMenu("Activate")]
    public void Activate()
    {
        // Only rotate if we are not already mid-rotation
        if(!rotating)
        {
            // Swap to next target rotation...
            (nextTargetRotation, targetRotation) = (targetRotation, nextTargetRotation);
            // ...and rotate!
            StartCoroutine(Rotate90());
        }
    }
}
