using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trunkRoll : MonoBehaviour
{
    //Essentials
    public float speed;
    public float distance;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = startPos;
        v.z += distance * Mathf.Sin(Time.deltaTime * speed);
        transform.position = v;

    }
}
