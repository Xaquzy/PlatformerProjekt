using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    
    public Transform orientation;
    public Transform Player;
    public Transform PlayerObject;
    public Rigidbody rb;

    public float RotationSpeed;

    private void update()
    {
        // Rotate orientation
        Vector3 ViewDir = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        orientation.forward = ViewDir.normalized;

        // Rotate PlayerObject
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Vector3 InputDir = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

        if(InputDir != Vector3.zero)
            PlayerObject.forward = Vector3.Slerp(PlayerObject.forward, InputDir.normalized, Time.deltaTime * RotationSpeed);
    }
}
