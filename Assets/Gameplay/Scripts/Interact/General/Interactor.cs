using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

    [SerializeField] private float slapForce = 5f;
    [SerializeField] private float slapRadius = 0.75f;
    [SerializeField] private float howMuchUp = 0.75f;
    [SerializeField] private float slapDamage = 5;

    [SerializeField] private GameObject smokeParticle;
    [SerializeField] PhysicMaterial slapMat;
    [SerializeField] private bool shouldDebug = false;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform vfxHolder;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main?.gameObject.transform;
        vfxHolder = GameObject.FindWithTag("VFXHolder").transform; //Sorry about this maddox, gonna move it to the gameManager when thats done, TS
        TryGetComponent(out playerControler);
        if (TryGetComponent(out input))
        {
            interact = input.actions["Interact"];
            interact.performed += ctx => TryInteract();
        }
    }
    
    private void TryInteract()
    {
        if (playerControler.interacting)
        {
            playerControler.interacting = false;
            playerControler.EnableMovement();
            //AreaImIn.InteractCancel();
            return;
        }
        
        playerControler.interacting = true;
        playerControler.animator.SetBool("IsSlapping", true);
        playerControler.animator.CrossFade("Slap", 0.1f);

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
    Collider[] colliders = new Collider[10];
    [SerializeField] private float howFar = 0.45f;
    [SerializeField] private Vector3 offset = new(0,-0.1f,0);

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
                extraForce -= 21;
            }
            if (toSlap.TryGetComponent(out AIBrain brain))
            {
                //disable the agent and enable kinematic
                StartCoroutine(brain.ReenableAgent(brain.knockDownTime));
                brain.ChangeState(AIBrain.State.Chase);

                var current = GetComponent<Inventory>();  
                //see if we can take the item from the brain
                if (brain.inventory && brain.inventory.item && (current.item == null || current.item.type == Item.Type.NoItem))
                {
                    current.AddItem(brain.inventory.item.gameObject);
                }
                else if (current.item)
                {
                    brain.inventory.TakeItem(current);
                }
                
            }
            rb.AddForce(((transform.forward ) * (slapForce + extraForce/5) )+ ((transform.up * howMuchUp) * slapForce / 5), ForceMode.Impulse);
            
            var pos = transform.position +  Vector3.forward*0.5f;
            Vector3 lookDir = cam.position - pos;
            Quaternion direction = Quaternion.LookRotation(lookDir);
            Instantiate(smokeParticle, pos, direction);
        }
        if(canSlapSfx)
            SoundManager.PlayAudioClip("Slap",transform.position + transform.forward,1f);
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
