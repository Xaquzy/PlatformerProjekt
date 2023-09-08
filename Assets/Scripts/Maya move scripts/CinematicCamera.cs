using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    public Camera mainCamera;
    public float stayDuration = 1f;

    [HideInInspector]
    public Vector3 targetPosition;

    public void Activate()
    {
        if (mainCamera)
        {
            mainCamera.GetComponent<CameraMove>().Cinematic(targetPosition, stayDuration);
        }
    }
}
