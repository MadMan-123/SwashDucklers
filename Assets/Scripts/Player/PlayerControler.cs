using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class PlayerControler : MonoBehaviour
{

    //Pointer to input system
    private InputManager input = null;
    private Vector3 moveVector = Vector3.zero;
    private Rigidbody rb = null;
    private bool isGrounded = false;
    private bool isJumping = true;
    private bool isGliding = false;
    private float jumpTimer;
    private float glideTimer;
    private float glideHeight;
    private Rigidbody platform; //Rigidbody of object player is standing on if there is one
    private Vector3 relative0; //A vector representing 0 relative to whatever platform you are currently on
    private Vector3 PrevRelative0; //A vector representing the last platform you where on
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float deceleration;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpDuration;
    [SerializeField] float airControl;
    [SerializeField] float glideDuration;
    [SerializeField] float playerID;
    [SerializeField] AudioSource Quack;

    //On Awake
    private void Awake()
    {
        input = new InputManager();
        rb = GetComponent<Rigidbody>();
        relative0 = new Vector3(0.0f, 0.0f, 0.0f);
    }

    //On Enable
    private void OnEnable()
    {
        //Enable inputs
        input.Enable();

        //Movement
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;

        //Jump
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;

        //Interact
        input.Player.Interact.performed += OnInteractPerformed;
        input.Player.Interact.canceled += OnInteractCancelled;

        //Taunt
        input.Player.Taunt.performed += OnTauntPerformed;
        input.Player.Taunt.canceled += OnTauntCancelled;
    }

    //On Disable
    private void OnDisable()
    {
        //Disable inputs
        input.Disable();

        //Movement
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;

        //Jump
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;

        //Interact
        input.Player.Interact.performed -= OnInteractPerformed;
        input.Player.Interact.canceled -= OnInteractCancelled;

        //Taunt
        input.Player.Taunt.performed -= OnTauntPerformed;
        input.Player.Taunt.canceled -= OnTauntCancelled;
    }

    //On start
    private void Start()
    {

        //Temporary
        //Set material based on player id
        //In the future we can use simalar logic to set models
        switch (playerID)
        {
            case 0:
                //this is the single worst line of code ive ever written but it works for now and im not changing it because its 3am
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            case 1:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case 2:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case 3:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                break;
        }
    }

    //FixedUpdate
    void FixedUpdate()
    {

        //If on the ground
        if (isGrounded)
        {
            //add direction * acceleration to velocity
            rb.velocity = rb.velocity + (moveVector * acceleration);
        }
        else
        {
            //add direction * acceleration to velocity changed by the air control modifier
            rb.velocity = rb.velocity + (moveVector * acceleration *airControl);
        }

        PrevRelative0 = relative0;

        //If on moving object
        if (platform != null)
        {

            //Find 0 velocity relative to current platform
            relative0 = new Vector3(platform.velocity.x, platform.velocity.y, platform.velocity.z);
            
        }
        else
        {
            //Relative 0 is just 0
            relative0 = new Vector3(0.0f,0.0f,0.0f);

        }

        //If platform has changed, or changed direction
        if (PrevRelative0 != relative0)
        {
            //Remove speed of old patform and replace with speed of new platform
            rb.velocity = rb.velocity - PrevRelative0;
            rb.velocity = rb.velocity + relative0;
        }

        //Make sure velocity doesnt exceed maxSpeed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, relative0.x - maxSpeed, relative0.x + maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, relative0.z - maxSpeed, relative0.z + maxSpeed));

        //Rotate to face direction moving
        if (moveVector != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = rotation;

            //Code to come to a stop quicker if diffrent keys are being pressed than current acceleration
            //If moveVector X = 0
            if (moveVector.x == 0)
            {
                //If velocity X isnt 0
                if (rb.velocity.x != relative0.x)
                {
                    //If Velocity X is positive
                    if (Mathf.Sign(rb.velocity.x) == 1)
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity - new Vector3(deceleration, 0.0f, 0.0f);

                        //If lowered below 0
                        if (rb.velocity.x < relative0.x)
                        {
                            //Set X value to 0
                            rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z); ;
                        }
                    }
                    else  //If Velocity X is Negative
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity + new Vector3(deceleration, 0.0f, 0.0f);

                        //If raised below 0
                        if (rb.velocity.x > relative0.x)
                        {
                            //Set X value to 0
                            rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z); ;
                        }
                    }
                }
            }
            //If moveVector Z = 0
            if (moveVector.z == 0)
            {
                //If velocity Z isnt 0
                if (rb.velocity.z != relative0.z)
                {
                    //If Velocity z is positive
                    if (Mathf.Sign(rb.velocity.z) == 1)
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity - new Vector3(0.0f, 0.0f, deceleration);

                        //If lowered below 0
                        if (rb.velocity.z < relative0.z)
                        {
                            //Set z value to 0
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z); ;
                        }
                    }
                    else  //If Velocity Z is Negative
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, deceleration);

                        //If raised above 0
                        if (rb.velocity.z < 0f)
                        {
                            //Set z value to 0
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z); ;
                        }
                    }
                }
            }
        }
        else //If moveVector = 0 then no movement is being provided
        {
            //Code to come to a stop quicker if no movement keys are being pressed
            //If velocity X isnt 0
            if (rb.velocity.x != relative0.x)
            {
                //If Velocity X is positive
                if (Mathf.Sign(rb.velocity.x) == 1)
                {
                    //Lower velocity by deceleration value
                    rb.velocity = rb.velocity - new Vector3(deceleration, 0.0f, 0.0f);

                    //If lowered below 0
                    if (rb.velocity.x < relative0.x)
                    {
                        //Set X value to 0
                        rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z); ;
                    }
                }
                else  //If Velocity X is Negative
                {
                    //Lower velocity by deceleration value
                    rb.velocity = rb.velocity + new Vector3(deceleration, 0.0f, 0.0f);

                    //If raised below 0
                    if (rb.velocity.x > relative0.x)
                    {
                        //Set X value to 0
                        rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z); ;
                    }
                }
            }

            //If velocity Z isnt 0
            if (rb.velocity.z != relative0.z)
            {
                //If Velocity z is positive
                if (Mathf.Sign(rb.velocity.z) == 1)
                {
                    //Lower velocity by deceleration value
                    rb.velocity = rb.velocity - new Vector3(0.0f, 0.0f, deceleration);

                    //If lowered below 0
                    if (rb.velocity.z < relative0.z)
                    {
                        //Set z value to 0
                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z); ;
                    }
                }
                else  //If Velocity Z is Negative
                {
                    //Lower velocity by deceleration value
                    rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, deceleration);

                    //If raised above 0
                    if (rb.velocity.z < relative0.z)
                    {
                        //Set z value to 0
                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z); ;
                    }
                }
            }
        }

        //Jumping
        if (isJumping)
        {
            //Continue to jump
            rb.velocity = rb.velocity + new Vector3(relative0.x, jumpPower, relative0.z);

            //Decrease jump time
            jumpTimer = jumpTimer - 1 * Time.deltaTime;
            if (jumpTimer < 0)
            {
                isJumping = false;
                jumpTimer = jumpDuration;
            }
        }
        if (isGliding)
        {
            //Stay at current height
            rb.position = new Vector3(rb.position.x, glideHeight, rb.position.z);

            //Decrease jump time
            glideTimer = glideTimer - 1 * Time.deltaTime;
            if (glideTimer < 0)
            {
                isGliding = false;
                glideTimer = glideDuration;

                //Messy code for temporary animation
                this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }

    //Movement
    //Performed
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        //Setup Movevector to contain direction from input
        moveVector = value.ReadValue<Vector3>();

    }
    //Cancelled
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        //Setup Movevector to Zero
        moveVector = Vector3.zero;

    }

    //Jump
    //Performed
    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        //Jump behaviour
        //If on Ground
        if (isGrounded)
        {
           //Start to Jump (Inital jump is more powerful to make the arc nicer)
           rb.velocity = rb.velocity + new Vector3(0f, jumpPower * 1.5f, 0f);
           isGrounded = false;
           isJumping = true;
           jumpTimer = jumpDuration;
        }
        else
        {
            glideHeight = rb.position.y;
            isGliding = true;
            glideTimer = glideDuration;

            //Messy code for temporary animation
            this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = true;
        }
        
    }
    //Cancelled
    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        //Stop jumping or gliding
        isJumping = false;
        isGliding = false;

        //Messy code for temporary animation
        this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = false;
    }

    //Interact
    //Performed
    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        //Interact behaviour
    }
    //Cancelled
    private void OnInteractCancelled(InputAction.CallbackContext value)
    {
        //Stop Interact behaviour
    }

    //Taunt
    //Performed
    private void OnTauntPerformed(InputAction.CallbackContext value)
    {
        //Taunt behaviour
        if (Random.Range(1, 1000) < 1000)
        {
            Quack.pitch = Random.Range(1.0f, 2.0f);
        }
        else
        {
            Quack.pitch = Random.Range(0.0f, 1.0f);
        }
        Quack.Play(0);
        this.gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.5f, this.gameObject.transform.position.z);
    }
    //Cancelled
    private void OnTauntCancelled(InputAction.CallbackContext value)
    {
        //Stop Taunt behaviour
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    //Controler disconected
    private void OnDeviceLost()
    {

        Debug.Log("Device lost");

        //Disconect player
        Destroy(this.gameObject);//Bad idea but works for now
    }

    //On initial collision
    void OnCollisionEnter(Collision collision)
    {

        //For each collision
        foreach (ContactPoint contact in collision.contacts)
        {

            //If colliding with the object from above
            if (-45 <= Vector3.Angle(Vector3.up, contact.normal) && 45 >= Vector3.Angle(Vector3.up, contact.normal))
            {
                //The player is now grounded
                isGrounded = true;

                //Check if ground is a rigidbody
                platform = collision.gameObject.GetComponent<Rigidbody>();

                //If it is
                if (platform != null)
                {
                    rb.velocity = platform.velocity;
                }

            }

        }
    }

    //On continued collision
    void OnCollisionStay(Collision collision)
    {
        //For each collision
        foreach (ContactPoint contact in collision.contacts)
        {

            //Draw collision normals in gizmos
            Debug.DrawRay(contact.point, contact.normal, Color.white);

        }

    }

    //On leaving collision
    void OnCollisionExit(Collision collision)
    {

        //For each collision
        foreach (ContactPoint contact in collision.contacts)
        {

            //If colliding with the object from above
            if (-45 <= Vector3.Angle(Vector3.up, contact.normal) && 45 >= Vector3.Angle(Vector3.up, contact.normal))
            {
                //No longer on ground
                isGrounded = false;
                platform = null;
                relative0 = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

    }

}
