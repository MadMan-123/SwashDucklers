using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerControler : MonoBehaviour
{

    private Vector3 moveVector = Vector3.zero;
    [FormerlySerializedAs("rb")] public Rigidbody rigidbody = null;
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
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsSlapping = Animator.StringToHash("IsSlapping");
    [SerializeField] public CinemachineTargetGroup cameraTarget;
    [SerializeField] public float playerCameraWeight;
    [SerializeField] public float playerCameraRadius;
    //On Awake

    private void Awake()
    {
        cameraTarget.AddMember(transform, 3, 2.5f);
        //input = new InputManager();
        rigidbody = GetComponent<Rigidbody>();
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
        
        //Get the player colour and hat and set them
        
        litColor = playerID switch 
        {
            0 => PlayerStats.player1Color,
            1 => PlayerStats.player2Color,
            2 => PlayerStats.player3Color,
            3 => PlayerStats.player4Color,
            _ => Color.white
        };
       
        //Set the color of the body

        if (PlayerStats.Hatlist == null)
            return;
        //will the playerID align with the hat list? 
        //no thats not how it works maddox - sd
        //Im readding this so the hats dont brick the game
        switch (playerID)
        {
            case 0:
            if (PlayerStats.Hatlist != null)
            {
                if (PlayerStats.Hatlist[PlayerStats.player1Hat].model != null)
                {
                    hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player1Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player1Hat].position, transform.rotation, hatTransform);
                }
            }
            break;
        case 1:
            if (PlayerStats.Hatlist != null)
            {
                if (PlayerStats.Hatlist[PlayerStats.player2Hat].model != null)
                {
                    hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player2Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player2Hat].position, transform.rotation, hatTransform);
                }
            }
            break;
        case 2:
            if (PlayerStats.Hatlist != null)
            {
                if (PlayerStats.Hatlist[PlayerStats.player3Hat].model != null)
                {
                    hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player3Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player3Hat].position, transform.rotation, hatTransform);
                }
            }
            break;
        case 3:
            if (PlayerStats.Hatlist != null)
            {
                if (PlayerStats.Hatlist[PlayerStats.player4Hat].model != null)
                {
                    hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player4Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player4Hat].position, transform.rotation, hatTransform);
                }
            }
            break;
        }

        //get the hat
        //var hatPrefab = PlayerStats.Hatlist[playerID].model;

        //get the prefab position
        //var offset = PlayerStats.Hatlist[playerID].position;
        
        //Instantiate the hat
        //hat = Instantiate(hatPrefab, hatposition + offset, transform.rotation, hatTransform);
        

    }

    private void HandleInitalSpawn()
    {
        if (!spawned)
        {
            transform.rotation = Quaternion.LookRotation(spawnRotation, Vector3.up);
            transform.position = spawnpoint;
            spawned = true;
        }
    }

    private void ApplyMovement()
    {
        float currentAcceleration = isGrounded ? acceleration : acceleration * airControl;
        rigidbody.velocity += (moveVector * currentAcceleration);
        
        
    }

    //do we really need this? in the current state of the project - MW
    private void HandlePlatformMovment()
    {
        //store previous platform velocity
        PrevRelative0 = relative0;
        
        //set the reference velocity
        relative0 = platform != null ? platform.velocity : Vector3.zero;
        
        //adjust the velocity
        if (PrevRelative0 != relative0)
        {
            rigidbody.velocity -= PrevRelative0;
            rigidbody.velocity += relative0;
        }
        
    }

    private void ClampHorizontalVelocity()
    {
        var horizontalVelocity = rigidbody.velocity;
        horizontalVelocity.y = 0; // Keeps gravity unchanged
        horizontalVelocity += moveVector * acceleration;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
        
        rigidbody.velocity = new Vector3(horizontalVelocity.x, rigidbody.velocity.y, horizontalVelocity.z);
    }
    
    
    private void HandleRotationAndAnimation()
    {
        //Rotate to face direction moving
        //if the move vector is not zero
        if (Mathf.Abs(moveVector.magnitude) >= 0.01f)
        {
            //rotate the player
            transform.rotation = Quaternion.LookRotation(moveVector, Vector3.up);
            
            if (!animator.GetBool(IsWalking))
                animator.SetBool(IsWalking, true);
        }
        else
        {
            if (!animator.GetBool(IsSlapping))
                animator.CrossFade("Idle", 0.1f);
            
            animator.SetBool(IsWalking, true);
        }
    }


    private void ApplyDeceleration()
    {
       if(ShouldDecelerateOnAxis(moveVector.x, rigidbody.velocity.x, relative0.x)) 
       {
            rigidbody.velocity = DecelerateOnAxis(rigidbody.velocity, relative0,true);
       }

       if (ShouldDecelerateOnAxis(moveVector.z, rigidbody.velocity.z, relative0.z))
       {
           rigidbody.velocity = DecelerateOnAxis(rigidbody.velocity, relative0,false);
       }

       if (HasStopeed())
       {
           StopMovmentSound();
       }
    }

    private Vector3 DecelerateOnAxis(Vector3 rbVelocity, Vector3 relativeVelocity, bool isXAxis)
    {
        float currentValue = isXAxis ? rbVelocity.x : rbVelocity.z;
        float targetValue = isXAxis ? relativeVelocity.x : relativeVelocity.z;

        // Moving in positive direction, need to decrease
        if (currentValue > targetValue)
        {
            Vector3 decelerationVector = isXAxis ?
                new Vector3(deceleration, 0.0f, 0.0f) :
                new Vector3(0.0f, 0.0f, deceleration);
            
            rbVelocity -= decelerationVector;
            
            // Check if we overshot the target
            if((isXAxis ? rbVelocity.x : rbVelocity.z) < targetValue)
            {
                if (isXAxis)
                    rbVelocity.x = targetValue;
                else
                    rbVelocity.z = targetValue; // FIXED: Was not setting the value correctly
            }
        }
        // Moving in negative direction, need to increase
        else if (currentValue < targetValue)
        {
            Vector3 decelerationVector = isXAxis ?
                new Vector3(deceleration, 0.0f, 0.0f) :
                new Vector3(0.0f, 0.0f, deceleration);
            
            rbVelocity += decelerationVector;
            
            // Check if we overshot the target
            if((isXAxis ? rbVelocity.x : rbVelocity.z) > targetValue)
            {
                if (isXAxis)
                    rbVelocity.x = targetValue;
                else
                    rbVelocity.z = targetValue;
            }
        }
        
        return rbVelocity;
    }

    private bool HasStopeed()
    {
        return (Mathf.Approximately(rigidbody.velocity.x, relative0.x) && Mathf.Approximately(rigidbody.velocity.z, relative0.z));
    }

    private void StopMovmentSound()
    {
        if (isSoundPlaying)
        {
            audioSource.Stop();
            isSoundPlaying = false; // Mark sound as playing
        }
    }

    private bool ShouldDecelerateOnAxis(float moveVectorX, float velocityX, float relative0X) => (moveVectorX == 0 && Math.Abs(velocityX - relative0X) > 0.1f);
    
    private void Start()
    {
        SetPlayerColor(playerID);
    }
    
    //FixedUpdate
    void FixedUpdate()
    {
        HandleInitalSpawn();
        
        //if we can move
        if (canMove)
        {
            //apply proper movement
            ApplyMovement();

            //relative 
            HandlePlatformMovment();
            
            ClampHorizontalVelocity();

            HandleRotationAndAnimation();

            //apply deceleration
            ApplyDeceleration();


        }

    }


    void Update()
    {
        var main = movementVFX.main;
        
        if (Mathf.Abs(moveVector.magnitude) >= 0.1f) // Movement detected
        {
            if (!movementVFX.isPlaying) // If it's not playing, start it
            {
                if (!isSoundPlaying)
                {
                    audioSource.Play();
                    isSoundPlaying = true; // Mark sound as playing
                }
                main.loop = true;
                movementVFX.Play();
            }
        }
        else // If no movement, stop VFX
        {
            if (movementVFX.isPlaying)
            {
                audioSource.Stop();
                isSoundPlaying = false;
                main.loop = false;
                movementVFX.Stop();
            }
        }
    
        //Check if player ends up out of bounds somehow -SD
        //Hopefully never triggers
        if (this.transform.position.y < -10f)
        {
            Debug.LogError("Player fell out of bounds!");
            //Debug.Break();
            transform.position = spawnpoint;
        }

    }
    private void SetPlayerColor(int playerId)
    {

        //Body
        bodyRenderer.material.SetColor("_Color", litColor); //Light Color

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
                rigidbody.AddForce(0,force * 1.5f,0);
                isGrounded = false;
                isJumping = true;
                jumpTimer = jumpDuration;
            }
            else
            {
                glideHeight = rigidbody.position.y;
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
                    rigidbody.velocity = platform.velocity;
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
        rigidbody.freezeRotation = false;
        //rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;              //unlocks rotation on the z and x axis so it rolls about   : all below TS
        //rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        StartCoroutine(UndoRagdoll(ragdollTime));
        DisableMovement();      //could not possibly extrapolate what this does
    }
    private void UnRagdoll()
    {
        rigidbody.freezeRotation = true;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;                 //locks rotation on the z and x axis
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
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
        rigidbody.freezeRotation = false;
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
