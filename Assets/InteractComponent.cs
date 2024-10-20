using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] GameObject tool;
    [SerializeField] public bool inArea;
    [SerializeField] GameObject tempIndicator;
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
            if(tool == null)
            {
                Interact();
            }
            else
                Interact(tool);
        }
    }

    void Interact()
    {
        if (inArea)
        {
            StartCoroutine(InteractTimer());
            AreaImIn.FunctionIDO();
        }
    }

    void Interact(GameObject tool)
    {

    }

    IEnumerator InteractTimer()
    {
        tempIndicator.SetActive(true);
        yield return new WaitForSeconds(2);
        tempIndicator.SetActive(false);
    }
}
