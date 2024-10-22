using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTest : MonoBehaviour
{

    private Rigidbody rb = null;
    bool direction;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        direction = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Platform will move back and forth
        if (direction)
        {
            rb.velocity = rb.velocity - new Vector3(0.0f, 0.0f, acceleration);
        }
        else
        {
            rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, acceleration);
        }

        if (rb.position.z <= 5.0f && direction == true)
        {
            direction = false;
        }
        else if (rb.position.z >= 10.0f && direction == false)
        {
            direction = true;
        }

        //Clamp speed to Maxspeed
        rb.velocity = new Vector3(0.0f,0.0f, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

    }
}
