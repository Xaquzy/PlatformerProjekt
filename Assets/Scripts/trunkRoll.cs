
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class trunkRoll : MonoBehaviour

{
    public Transform startPoint;
    public Transform endPoint;
    public float speed;

    private Vector3 targetPosition;
    private bool moveToEnd = true;

    void Start()
    {
        targetPosition = endPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            if (moveToEnd)
            {
                transform.position = startPoint.position;
            }
            else
            {
                targetPosition = endPoint.position;
            }

            //MovingToEnd = !MovingToEnd;
        }


    }
}