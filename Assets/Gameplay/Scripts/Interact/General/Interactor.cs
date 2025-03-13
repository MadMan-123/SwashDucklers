using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] GameObject tempIndicator;
    [SerializeField] GameObject toolPU;
   
    private PlayerControler playerControler;
    private PlayerInput input;
    private InputAction interact;
    private InputAction dropItem;

    [SerializeField] private float slapForce = 5f;
    [SerializeField] private float slapRadius = 0.75f;
    [SerializeField] private float howMuchUp = 0.75f;
    [SerializeField] private float slapDamage = 5;

    [SerializeField] private GameObject smokeParticle;
    [SerializeField] PhysicMaterial slapMat;
    [SerializeField] private bool shouldDebug = false;
    bool isInteracting = false;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform vfxHolder;

    
    Collider[] colliders = new Collider[10];
    Collider[] prevColliders = new Collider[10];
    [SerializeField] private float howFar = 0.45f;
    [SerializeField] private Vector3 offset = new(0,-0.1f,0);
    [SerializeField] private float interactCooldown = 1f;
    void Start()
    {
        cam = Camera.main?.gameObject.transform;
        //smh tom, im not sure i get why were doing that tbh
        TryGetComponent(out playerControler);
        if (TryGetComponent(out input))
        {
            interact = input.actions["Interact"];
            interact.performed += ctx => TryInteract();
            dropItem = input.actions["DropItem"];
            dropItem.performed += ctx => TryDropItem();
        }
    }

    private void Update()
    {
        var count = Physics.OverlapSphereNonAlloc((transform.position + offset) + (transform.forward * howFar), slapRadius, colliders);
        var valid = colliders.Where(col => col!=null && TryGetComponent(out Interactable interactable)).ToList();
        if (valid.Count != 0)
        {
            for (int i = 0; i < valid.Count-1; i++) { valid[i].GetComponent<Interactable>().ToggleFlash(true); }

            for (int i = 0; i < prevColliders.Length; i++)
            {
                for (var j = 0; j < valid.Count-1; j++)
                {
                    if (prevColliders[i].gameObject.Equals(valid[j].gameObject)){break;}
                    if(j == valid.Count-1){ prevColliders[i].GetComponent<Interactable>().ToggleFlash(false);}
                }
                
            }
        }
        else
        {
            for (var i = 0; i < prevColliders.Length; i++)
            {
                if (prevColliders[i] != null && prevColliders[i].TryGetComponent(out Interactable interactable)) {interactable.ToggleFlash(false); }
            }
        }
        prevColliders = colliders;
    }

    private void TryInteract()
    {
        if (isInteracting) return;
        if (playerControler.interacting)
        {
            playerControler.interacting = false;
            playerControler.EnableMovement();
            //AreaImIn.InteractCancel();
            return;
        }

        playerControler.Rumble(playerControler.interactRumble);

        playerControler.interacting = true;
        playerControler.animator.SetBool("IsSlapping", true);
        playerControler.animator.CrossFade("Slap", 0.1f);

        //isInteracting = true;
        StartCoroutine(InteractCooldown(interactCooldown));
        if (TryGetComponent(out Inventory inv) && inv.TryPickUp())
        {
           return;
        }

        var count = Physics.OverlapSphereNonAlloc((transform.position + offset) + (transform.forward * howFar), slapRadius, colliders);

        GameObject tracked = null;
        List<Rigidbody> rigidBodies = new(10);
        //go through and check if we can interact
        for (int i = 0; i < count; i++)
        {
            //check if the collider has a rigidbody
            if (colliders[i].TryGetComponent(out Rigidbody rb))
            {
                rigidBodies.Add(rb);
            }

            if (tracked) continue;
            if (!colliders[i].TryGetComponent(out Interactable current))
                continue;
           
            
            tracked = colliders[i].gameObject;
        }

        foreach (var body in rigidBodies)
        {
            if(body.gameObject == gameObject || !body) continue;
            Slap(body.gameObject); 
        }
        if (tracked == null)
        {
            return;
        }


        
        if (TryGetComponent(out Inventory inventory) && tracked.TryGetComponent(out Interactable interact))
        {
            if(shouldDebug)
                print("Interacting with " + tracked.name);
            interact.Interact(inventory.item, gameObject);
        }
        

        
        //Reset the flag
        Invoke(nameof(ResetSlapAnim), 0.5f);
    }

    private void ResetSlapAnim()
    {
        playerControler.animator.SetBool("IsSlapping", false);
    }

    private void Slap(GameObject toSlap)
    {
        bool canSlapSfx = false;
        {
            
            float extraForce = 0f;
            if (!toSlap.TryGetComponent(out Rigidbody rb)) return; //if same object continue
            canSlapSfx = true;

            if (toSlap.TryGetComponent(out Health health))         //if has component health then
            {
                health.TakeDamage(gameObject,slapDamage);
                extraForce = (health.GetHealth() / health.GetMaxHealth()) * slapForce;
            }
            if (toSlap.TryGetComponent(out PlayerControler pc))
            {
                StartCoroutine(ReduceFriction(toSlap.gameObject,pc, (extraForce/5)));
                pc.Ragdoll(0.25f + 0.25f*extraForce,true);
                extraForce -= 15;
            }
            if (toSlap.TryGetComponent(out AIBrain brain))
            {
                //disable the agent and enable kinematic
                StartCoroutine(brain.ReenableAgent(brain.knockDownTime));
                brain.ChangeState(AIBrain.State.Chase);


                if(brain.TryGetComponent(out Inventory inv))
                {
                    inv.DropItem();
                }
                
            }
            rb.AddForce(((transform.forward ) * (slapForce + extraForce/5) )+ ((transform.up * howMuchUp) * slapForce / 5), ForceMode.Impulse);
            
            //much better, take the transform forward instead of vector3.forward, that way we can slap in any direction - MW
            var pos = transform.position +  transform.forward * 0.5f;
            var delta = cam.position - pos;
            var direction = Quaternion.LookRotation(delta);
            Instantiate(smokeParticle, pos, direction);
        }
        if(canSlapSfx)
            SoundManager.PlayAudioClip("Slap",transform.position + transform.forward,1f);
    }


    private IEnumerator InteractCooldown(float sec = 1f)
    {
        //wait for the cooldown
        isInteracting = false;
        yield return new WaitForSeconds(sec);
        isInteracting = false;
        
    }

    private void TryDropItem()
    {
        if(isInteracting) return;
        if (TryGetComponent(out Inventory inv))
        {
            inv.DropItem();
        }
        StartCoroutine(InteractCooldown(interactCooldown));
    }


   

#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if(!shouldDebug) return;
        //draw the force velocity
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, ((transform.forward + ((transform.up * 0.1f))) * slapForce ));
        //draw the sphere cast
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((transform.position + offset)+ (transform.forward * howFar), slapRadius);

    }
#endif

    IEnumerator ReduceFriction(GameObject player, PlayerControler pc, float slapForce)
    {
        pc.deceleration = 0.001f;
        PhysicMaterial temp = null;
        gameObject.GetComponent<CapsuleCollider>().material = slapMat;
        yield return new WaitForSeconds(1 + slapForce);
        pc.deceleration = 0.1f;
        gameObject.GetComponent<CapsuleCollider>().material = temp;
    }
}
