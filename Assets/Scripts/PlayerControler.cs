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
    }

    //On Disable
    private void OnDisable()
    {
        //Disable inputs
        input.Disable();

        //Movement
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
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

    //Controler disconected
    private void OnDeviceLost()
    {
        
        Debug.Log("Device lost");

        //Disconect player
        Destroy(this.gameObject);//Bad idea but works for now
    }

}
