using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine.SceneManagement;
using Cinemachine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerControler : MonoBehaviour
{

    private Vector3 moveVector = Vector3.zero;
    public Rigidbody rb = null;
    private bool isGrounded = false;
    //private bool isJumping = true;
    //private bool isGliding = false;
    private float jumpTimer;
    private float glideTimer;
    private float glideHeight;
    private PlayerInput playerInput;
    private InputAction displayNames;
    private Rigidbody platform; //Rigidbody of object player is standing on if there is one
    private Vector3 relative0; //A vector representing 0 relative to whatever platform you are currently on
    private Vector3 PrevRelative0; //A vector representing the last platform you where on
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float vMaxSpeed;
    [SerializeField] public float deceleration;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpDuration;
    [SerializeField] float airControl;
    [SerializeField] float glideDuration;
    [SerializeField] int playerID; //Why the fuck is an id a float, its never going to need to be a real number - MW
    [SerializeField] public bool interacting;
    [SerializeField] AudioSource Quack;
    [SerializeField] AudioSource audioSource;
    [SerializeField] public ParticleSystem movementVFX;
    [SerializeField] private Transform modelTransform;
    [SerializeField] float bumpForce;
    [SerializeField] float bumpForceUp;
    [SerializeField] float ragdollTime=5f;
    [SerializeField] Transform hatTransform;
    public bool isSoundPlaying = false;
    public Vector3 spawnpoint;
    public Vector3 spawnRotation;
    public Color litColor;
    public Color darkColor;
    public Animator animator;
    public bool canMove = true;
    private bool spawned = false;
    private Renderer beakRenderer;
    private Renderer bodyRenderer;
    private GameObject hat;
    private Vector3 hatposition;
    private Coroutine vfxFadeCoroutine;
    [SerializeField] private TextMeshProUGUI idText;
    //Reference to gamepad for rumble -SD
    public bool isGamepad = false;
    public Gamepad pad;

    //Rumble values -SD
    [Serializable] public struct RumbleVariables
    {
        [SerializeField][Range(0.0f, 1.0f)] public float lowFRumble;
        [SerializeField][Range(0.0f, 1.0f)] public float highFRumble;
        [SerializeField] public float rumbleLength;
    }

    [Header("Rumble Settings")]
    [Header("Interact Rumble")]
    [SerializeField] public RumbleVariables interactRumble;
    [Header("OnHit Rumble")]
    [SerializeField] public RumbleVariables onHitRumble;
    [Header("Item stolen Rumble")]
    [SerializeField] public RumbleVariables itemStolenRumble;

    private static readonly int Color1 = Shader.PropertyToID("_Color");
    [SerializeField] public CinemachineTargetGroup cameraTarget;
    [SerializeField] public float playerCameraWeight;
    [SerializeField] public float playerCameraRadius;
    //On Awake
    private void Awake()
    {
        cameraTarget.AddMember(transform, 3, 2.5f);
        //input = new InputManager();
        rb = GetComponent<Rigidbody>();
        relative0 = new Vector3(0.0f, 0.0f, 0.0f);

        playerInput = GetComponent<PlayerInput>();
        playerID = playerInput.playerIndex;
        displayNames = playerInput.actions["DisplayNames"];
        displayNames.performed += ctx => StartCoroutine(PlayerNameFade2(false, 1f));
        //Check type on input, used for rumble -SD
        if (playerInput.devices[0] is Gamepad)
        {
            Debug.Log("Gamepad");
            isGamepad = true;
            pad = (Gamepad)playerInput.devices[0];
        }
        else
        {
            Debug.Log("Keyboard");
            isGamepad = false;
        }

        // Cache model renderers
        //modelTransform = transform.GetChild(2).GetChild(0);

        beakRenderer = modelTransform.GetChild(0).GetComponent<Renderer>();
        bodyRenderer = modelTransform.GetChild(1).GetComponent<Renderer>();

        hatposition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        StartCoroutine(PlayerNameFade2(true,2f));
        //Get Color and hat
        switch (playerID)
        {
            case 0:
                litColor = PlayerStats.player1Color;
                if (PlayerStats.Hatlist != null)
                {
                    if (PlayerStats.Hatlist[PlayerStats.player1Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player1Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player1Hat].position, transform.rotation, hatTransform);
                    }
                }
                break;
            case 1:
                litColor = PlayerStats.player2Color;
                if (PlayerStats.Hatlist != null)
                {
                    if (PlayerStats.Hatlist[PlayerStats.player2Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player2Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player2Hat].position, transform.rotation, hatTransform);
                    }
                }
                break;
            case 2:
                litColor = PlayerStats.player3Color;
                if (PlayerStats.Hatlist != null)
                {
                    if (PlayerStats.Hatlist[PlayerStats.player3Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player3Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player3Hat].position, transform.rotation, hatTransform);
                    }
                }
                break;
            case 3:
                litColor = PlayerStats.player4Color;
                if (PlayerStats.Hatlist != null)
                {
                    if (PlayerStats.Hatlist[PlayerStats.player4Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player4Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player4Hat].position, transform.rotation, hatTransform);
                    }
                }
                break;
        }

    }

    //On start
    private void Start()
    {

        SetPlayerColor(playerID);

    }
    void Update()
    {
        var main = movementVFX.main;
        
        if (moveVector != Vector3.zero) // Movement detected
                                        
        {
            if (!movementVFX.isPlaying) // If it's not playing, start it
            {
                
                main.loop = true;
                movementVFX.Play();
            }
        }
        else // If no movement, stop VFX
        {
            if (movementVFX.isPlaying)
            {
                
                main.loop = false;
                movementVFX.Stop();
            }
        }
    
    }
    private void SetPlayerColor(int playerId)
    {

        //Body
        bodyRenderer.material.SetColor("_Color", litColor); //Light Color
        //bodyRenderer.material.SetColor("_1st_ShadeColor", darkColor); //Shaded Color

        //Beak and legs
        //beakRenderer.material.SetColor("_BaseColor", secondaryColor); //Light Color
        //beakRenderer.material.SetColor("_1st_ShadeColor", secondaryColor); //Shaded Color

        //for (int i = 0; i < bodyRenderer.material.shader.GetPropertyCount(); i++)
        //{
        //    Debug.Log(bodyRenderer.material.shader.GetPropertyName(i));
        //}



        //Debug.Log(bodymaterial.color);
        //Debug.Log(LitColor);
    }
    //FixedUpdate
    void FixedUpdate()
    {

        //Sean:This should really be in start but transform.positon doesnt work in there for whatever reason
        if (spawned == false)
        {
            transform.position = spawnpoint;
            transform.rotation = Quaternion.LookRotation(spawnRotation, Vector3.up);
            spawned = true;
        }

        //commented out cuz was flooding the console - MW
        //Debug.Log(moveVector);

        if (canMove)
        {
           
            //If on the ground
            if (isGrounded)
            {
                //add direction * acceleration to velocity
                rb.velocity += (moveVector * acceleration);

                // Debug.Log(rb.velocity);
            }
            else
            {
                //add direction * acceleration to velocity changed by the air control modifier
                rb.velocity += (moveVector * (acceleration * airControl));
            }

            PrevRelative0 = relative0;

            //If on moving object
            if (platform != null)
            {

                //Find 0 velocity relative to current platform
                relative0 = platform.velocity;

            }
            else
            {
                //Relative 0 is just 0
                relative0 = new Vector3(0.0f, 0.0f, 0.0f);

            }

            //If platform has changed, or changed direction
            if (PrevRelative0 != relative0)
            {
                //Remove speed of old patform and replace with speed of new platform
                rb.velocity -= PrevRelative0;
                rb.velocity += relative0;
            }

        
            Vector3 horizontalVelocity = rb.velocity; 
            horizontalVelocity.y = 0; // Keeps gravity unchanged
            horizontalVelocity += moveVector * acceleration;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
                
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            
            //Debug.Log(rb.velocity);

            //Rotate to face direction moving
            if (moveVector != Vector3.zero && canMove)
            {
                Quaternion rotation = Quaternion.LookRotation(moveVector, Vector3.up);
                transform.rotation = rotation;

                if (!animator.GetBool("IsWalking"))
                    //animator.CrossFade("IsWalking")
                    animator.SetBool("IsWalking", true);

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
                                rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z);
                                
                            }
                        }
                        else //If Velocity X is Negative
                        {
                            //Lower velocity by deceleration value
                            rb.velocity += new Vector3(deceleration, 0.0f, 0.0f);

                            //If raised below 0
                            if (rb.velocity.x > relative0.x)
                            {
                                //Set X value to 0
                                rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z);
                                
                                ;
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
                                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z);
                            }
                        }
                        else //If Velocity Z is Negative
                        {
                            //Lower velocity by deceleration value
                            rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, deceleration);

                            //If raised above 0
                            if (rb.velocity.z > relative0.z)
                            {
                                //Set z value to 0
                                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z);
                            }
                        }
                    }
                }
            }
            else //If moveVector = 0 then no movement is being provided
            {
                if (!animator.GetBool("IsSlapping"))
                    animator.CrossFade("Idle", 0.1f);
                animator.SetBool("IsWalking", false);


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
                            rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z);
                            ;  if (isSoundPlaying)
                            {
                                audioSource.Stop();

                                isSoundPlaying = false; // Mark sound as playing
                            }
                        }
                    }
                    else //If Velocity X is Negative
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity + new Vector3(deceleration, 0.0f, 0.0f);

                        //If raised below 0
                        if (rb.velocity.x > relative0.x)
                        {
                            //Set X value to 0
                            rb.velocity = new Vector3(relative0.x, rb.velocity.y, rb.velocity.z);
                            if (isSoundPlaying)
                            {
                                audioSource.Stop();

                                isSoundPlaying = false; // Mark sound as playing
                            }
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
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z);
                            ;  if (isSoundPlaying)
                            {
                                audioSource.Stop();

                                isSoundPlaying = false; // Mark sound as playing
                            }
                        }
                    }
                    else //If Velocity Z is Negative
                    {
                        //Lower velocity by deceleration value
                        rb.velocity = rb.velocity + new Vector3(0.0f, 0.0f, deceleration);

                        //If raised above 0
                        if (rb.velocity.z > relative0.z)
                        {
                            //Set z value to 0
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, relative0.z);
                            if (isSoundPlaying)
                            {
                                audioSource.Stop();

                                isSoundPlaying = false; // Mark sound as playing
                            }
                          
                        }
                    }
                }
            }
        }
      
    }

    //Movement
    public void OnMovement(InputAction.CallbackContext value)
    {
        var main = movementVFX.main;
        
        if (value.performed && canMove) //Performed
        {

            if (!isSoundPlaying)
            {
                audioSource.Play();
                isSoundPlaying = true; // Mark sound as playing

            }

            //Setup Move vector to contain direction from input
            moveVector = value.ReadValue<Vector3>();

        
            if (value.performed && canMove) // Movement started
            {
                if (!isSoundPlaying)
                {
                    audioSource.Play();
                    isSoundPlaying = true; // Mark sound as playing
                }
                
                moveVector = value.ReadValue<Vector3>();

            }
        }
        else if (value.canceled )// Movement stopped
        {
            moveVector = Vector3.zero;
            
        }
    }


    //Jump
    public void OnJump(InputAction.CallbackContext value)
    {
        /*if (value.performed) //Performed
        {
            //Jump behaviour
            //If on Ground
            if (isGrounded)
            {
                animator.CrossFade("Jump", 0.1f);

                //calculate force needed to jump to desired height for how everlong the duration is - MW
                float force = ((2 * jumpPower / jumpDuration) - 9.81f * jumpDuration); 
                //Start to Jump (Inital jump is more powerful to make the arc nicer)
                rb.AddForce(0,force * 1.5f,0);
                isGrounded = false;
                isJumping = true;
                jumpTimer = jumpDuration;
            }
            else
            {
                glideHeight = rb.position.y;
                isGliding = true;
                glideTimer = glideDuration;
            }
        }
        else if (value.canceled) //Cancelled
        {
            //Stop jumping or gliding
            isJumping = false;
            isGliding = false;

        }*/

    }

    //Interact
    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.performed) //Performed
        {

            //Interact behaviour

        }
        else if (value.canceled) //Cancelled
        {
            //Stop Interact behaviour
        }

    }

    //Taunt
    public void OnTaunt(InputAction.CallbackContext value)
    {

        if (value.performed) //Performed
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
            //changing localScale.y /2 to 0.5f has 0 difference and does not fix the qwack clipping issue - MW
            var localScale = transform.localScale;
            localScale = new Vector3(localScale.x, localScale.y / 2, localScale.z);
            
            
            transform.localScale = localScale;
            //ensuring the player is not placed below the ground is what we want to do here. -MW
            
            
            //if the scale is 1, and we are dividing by 2 (or multiply 0.5f) then moving down by 0.5f is the same as moving down by half the scale,
            //which is causing the clipping issue.
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        }
        else if (value.canceled) //Cancelled
        {
            //Stop Taunt behaviour
            gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);
        }
      
    }

    //Pause
    public void OnPause(InputAction.CallbackContext value)
    {

        if (value.performed && canMove) //Performed
        {

            //Disabled for demo day -SD
            //SceneManager.LoadScene("Character Select"); //GM: returns to the "menu test" screen
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            //Same thing as above but only works on keyboard -SD
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Character Select"); //GM: returns to the "menu test" screen
            }

        }
        else if (value.canceled) //Cancelled
        {

            //Setup Move vector to Zero
            //moveVector = Vector3.zero;

        }

    }

    //Controler disconected
    public void OnDeviceLost()
    {

        Debug.Log("Device lost");

        //Disconect player
        Destroy(gameObject);//Bad idea but works for now
    }

    //On initial collision
    void OnCollisionEnter(Collision collision)
    {
        //For each collision
        for (var i = 0; i < collision.contacts.Length; i++)
        {
            var contact = collision.contacts[i];
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
        for (var index = 0; index < collision.contacts.Length; index++)
        {
            var contact = collision.contacts[index];
            //Draw collision normals in gizmos
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    //On leaving collision
    void OnCollisionExit(Collision collision)
    {
        //For each collision
        for (var index = 0; index < collision.contacts.Length; index++)
        {
            var contact = collision.contacts[index];
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

    public void DisableMovement()
    {
        //playerInput.DeactivateInput(); // for the pump we want to read if the player is mashing the button
        //Debug.Log("Input disabled"); 
        canMove = false; //new implementation
        
    }
    public void EnableMovement()
    {
        //playerInput.ActivateInput();
        //Debug.Log("Input enabled");
        canMove = true;
    }
    public IEnumerator TempDisableMovement(float time)
    {
        DisableMovement();
        yield return new WaitForSeconds(time);
        EnableMovement();
    }
    public void Ragdoll()
    {
        rb.freezeRotation = false;
        //rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;              //unlocks rotation on the z and x axis so it rolls about   : all below TS
        //rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        StartCoroutine(UndoRagdoll(ragdollTime));
        DisableMovement();      //could not possibly extrapolate what this does
    }
    private void UnRagdoll()
    {
        rb.freezeRotation = true;
        //rb.constraints = RigidbodyConstraints.FreezeRotationZ;                 //locks rotation on the z and x axis
        //rb.constraints = RigidbodyConstraints.FreezeRotationX;
        transform.rotation = Quaternion.LookRotation(spawnRotation, Vector3.up);    //resets the rotation to normal, fix spawnRotation to maybe a temp value
        EnableMovement();    //i wonder what this does
    }

    private IEnumerator UndoRagdoll(float RagdollTime)         //just a timer really
    {
        yield return new WaitForSecondsRealtime(RagdollTime);
        UnRagdoll();
        //Rumble
        if (isGamepad)
        {
            pad.SetMotorSpeeds(0, 0);
        }
    }
    public void Ragdoll(float customTime, bool addToBase)
    {
        if (addToBase) { customTime += ragdollTime; }
        rb.freezeRotation = false;
        StartCoroutine(UndoRagdoll(customTime));
        DisableMovement();

        //Rumble
        Rumble(onHitRumble);

    }

    public void Rumble(RumbleVariables rumbleVariable)
    {
        //Rumble
        if (isGamepad)
        {
            pad.SetMotorSpeeds(rumbleVariable.lowFRumble, rumbleVariable.highFRumble);
            StartCoroutine(RumbleStop(rumbleVariable.rumbleLength));
        }

    }
    private IEnumerator RumbleStop(float Time)         //just a timer really
    {
        yield return new WaitForSecondsRealtime(Time);

        pad.SetMotorSpeeds(0, 0);
    }

    public void ToggleCamera(bool value)
    {
        if (value)
        {
            cameraTarget.AddMember(
                transform, 
                playerCameraWeight,
                playerCameraRadius
                );
        }
        else
        {
            cameraTarget.RemoveMember(transform);
        }
    }

    private IEnumerator PlayerNameFade2(bool justOut, float time = 5f)
    {
        idText.gameObject.SetActive(true);
        idText.text = $"PLAYER {playerID + 1}";
        if (!justOut)
        {
            idText.CrossFadeAlpha(1f, 0.5f,false );
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        idText.CrossFadeAlpha(0,time,false);
        yield return new WaitForSeconds(time);
        idText.gameObject.SetActive(false);
    }
}
