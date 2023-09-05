using System.Collections;
using UnityEngine;

public class RampRotate : MonoBehaviour
{
    public float rotationDuration;
    public float rotationDegrees;

    Quaternion targetRotation;
    Quaternion nextRotation;
    Quaternion initialRotation;

    private bool rotating = false;

    void Start()
    {
        initialRotation = transform.localRotation;
        targetRotation = transform.localRotation;
        SetNextRotation();

        // When activated, move camera here
        CinematicCamera cam = GetComponent<CinematicCamera>();
        if (cam)
        {
            cam.targetPosition = transform.position;
        }
    }

    void SetNextRotation()
    {
        nextRotation = targetRotation * Quaternion.Euler(0f, 0f, rotationDegrees);
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
        if (!rotating)
        {
            // Swap to next target rotation...
            targetRotation = nextRotation;
            SetNextRotation();
            // ...and rotate!
            StartCoroutine(Rotate90());
        }
    }
}
