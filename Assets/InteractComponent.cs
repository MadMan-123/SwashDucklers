using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] public string tool;
    [SerializeField] GameObject tempIndicator;
    [SerializeField] GameObject toolPU;

    [SerializeField] public bool inArea;    
    [SerializeField] public InteractArea AreaImIn;
    private PlayerControler playerControler;
    private PlayerInput input;
    private InputAction interact;
    [SerializeField] private float slapForce = 10;
    [SerializeField] private float slapRadius = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
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
            playerControler.enableMovement();
            //AreaImIn.InteractCancel();
            return;
        }
        
        playerControler.interacting = true;

        if (!inArea)
        {
            Slap();
            return;
        }
        if (!String.IsNullOrEmpty(tool))
        {
            Interact(tool);
        }
        else
        {
            Interact();
        }
    }

    private void Slap()
    {
        Collider[] colliders = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(transform.position + transform.forward, slapRadius, colliders);
        bool canSlapSfx = false;
        //for each collider in colliders, if can get rigidbody, add force
        for (int i = 0; i < count; i++)
        {
            if (colliders[i].gameObject == gameObject || !colliders[i].TryGetComponent(out Rigidbody rb)) continue;
            canSlapSfx = true;
            rb.AddForce(((transform.forward + ((transform.up * 0.1f))) * slapForce ), ForceMode.Impulse);
        }
        if(canSlapSfx)
            AudioManager.PlayAudioClip("Slap",transform.position + transform.forward,1f);
    }

    void Interact()
    {
       if(!AreaImIn) return; 
        if (AreaImIn.isStation)
        {
            if (inArea && !AreaImIn.needTool)
            {
                AreaImIn.Interact(this.gameObject);
                StartCoroutine(InteractTimer());
            }
            else 
            {
                WrongTool();
            }
        }

        if (!AreaImIn.isTool) return;
        AreaImIn.Interact(gameObject);
        StartCoroutine(InteractTimer());
    }

    void Interact(string tool)
    {
        if (inArea)
        {
            if (!AreaImIn.needTool)
            {
                AreaImIn.InteractWithTool(tool, gameObject);
                StartCoroutine(InteractTimer());
            }
            else if (tool == AreaImIn.toolUsed)
            {
                AreaImIn.InteractWithTool(tool, gameObject);
                StartCoroutine(InteractTimer());
            }
            else
            {
                WrongTool();
            }
        }
    }

    IEnumerator WrongTool()  //Testing
    {
        for (int i = 0; i < 3; i++)
        {
            toolPU.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            toolPU.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator InteractTimer()
    {
            tempIndicator.SetActive(true);
            yield return new WaitForSeconds(2);
            tempIndicator.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        //draw the force velocity
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, ((transform.forward + ((transform.up * 0.1f))) * slapForce ));
        //draw the sphere cast
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward, slapRadius);

    }
}
