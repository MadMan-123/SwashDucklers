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

        if (!String.IsNullOrEmpty(tool))
        {
            Interact(tool);
        }
        else
        {
            Interact();
        }
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
        AreaImIn.Interact(this.gameObject);
        StartCoroutine(InteractTimer());
    }

    void Interact(string tool)
    {
        if (inArea)
        {
            if (!AreaImIn.needTool)
            {
                AreaImIn.Interact(tool, this.gameObject);
                StartCoroutine(InteractTimer());
            }
            else if (tool == AreaImIn.toolUsed)
            {
                AreaImIn.Interact(tool, this.gameObject);
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
}
