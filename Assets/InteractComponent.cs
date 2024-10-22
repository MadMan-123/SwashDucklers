using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] public string tool;
    [SerializeField] GameObject tempIndicator;
    [SerializeField] GameObject toolPU;

    [SerializeField] public bool inArea;
    [SerializeField] public InteractArea AreaImIn;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (tool != null)
            {
                Interact(tool);
            }
            else
            {
                Interact();
            }
        }
    }

    void Interact()
    {
        if (AreaImIn.isStation)
        {
            if(inArea)
            if (AreaImIn.needTool == false)
            {
                AreaImIn.Interact(this.gameObject);
                StartCoroutine(InteractTimer());
            }
            else if (AreaImIn.needTool == true)
            {
                WrongTool();
            }
        }
        if (AreaImIn.isTool)
        {
            AreaImIn.Interact(this.gameObject);
            StartCoroutine(InteractTimer());
        }
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
