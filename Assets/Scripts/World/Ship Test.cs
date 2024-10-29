using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shiptest : MonoBehaviour
{

    private Rigidbody rb = null;
    bool direction;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxRotation;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        direction = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Ship will
        rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, acceleration);

        //Clamp speed to Maxspeed
        rb.velocity = new Vector3(0.0f,0.0f, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

/*        Vector3 euler = transform.rotation.eulerAngles;
        
        //Fix rotation
        
        if (euler.z < -maxRotation || euler.z > maxRotation)
        {
            rb.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

        }*/
        //when at max go up

    }
}
