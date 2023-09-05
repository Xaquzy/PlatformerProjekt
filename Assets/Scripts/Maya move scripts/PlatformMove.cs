using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Vector3 relativeMovement;
    public float moveSpeed = 0.5f;

    private Vector3 targetPosition;
    private Vector3 nextPosition;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        targetPosition = transform.position;
        nextPosition = targetPosition + relativeMovement;
    }

    // Update is called once per frame
    void Update()
    {
        // Always be moving toward target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void Activate()
    {
        // Swap target position
        (nextPosition, targetPosition) = (targetPosition, nextPosition);

        // Summon camera to look while the platform slides into place
        CinematicCamera cam = GetComponent<CinematicCamera>();
        if(cam)
        {
            cam.targetPosition = targetPosition;
        }
    }
}
