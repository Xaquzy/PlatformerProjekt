using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaninFollow : MonoBehaviour
{
    

    // Movement
    public GameObject Kanin;
    public GameObject Player;
    public float speed = 0;

    // Animation
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Follow
        Kanin.transform.position = Vector3.MoveTowards(Kanin.transform.position, Player.transform.position, speed);

        //Same rotation
        transform.rotation = Player.transform.rotation;

        //Animation
        //if (speed > 0)
        //{
           // animator.SetBool("NAVN", true);
        //}
        
        //else
        //{
           // animator.SetBool("NAVN", false);
        //}

        
    }
}
