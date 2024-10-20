using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    //Pointer to input system
    private InputManager input = null;
    private Vector3 moveVector = Vector3.zero;
    private Rigidbody rb = null;
    [SerializeField] float moveSpeed;
    [SerializeField] float playerID;
    [SerializeField] AudioSource Quack;

    //On Awake
    private void Awake()
    {
        input = new InputManager();
        rb = GetComponent<Rigidbody>();
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
                break;
            case 1:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                break;
            case 2:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                break;
            case 3:
                this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                break;
        }
    }

    //FixedUpdate
    void FixedUpdate()
    {
        //Time.deltaTime

        //Set velocity to direction * speed
        rb.velocity = moveVector * moveSpeed;

        //Rotate to face direction moving
        if (moveVector != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = rotation;
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
    }
    //Cancelled
    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        //Jump Interact behaviour
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
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y-0.5f, this.gameObject.transform.position.z);
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

}
