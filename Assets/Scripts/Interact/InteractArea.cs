using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractArea : MonoBehaviour
{
    [SerializeField] public bool needTool;
    
    [SerializeField] bool fasterWithTool;
    [SerializeField] public bool isTool;
    [SerializeField] public bool isStation;


    [SerializeField] public string toolUsed;
    [SerializeField] GameObject PopUp;
    [SerializeField] UnityEvent<GameObject,float> OnInteract;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<InteractComponent>().inArea = true;
            col.gameObject.GetComponent<InteractComponent>().AreaImIn = this;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<InteractComponent>().inArea = false;
            col.gameObject.GetComponent<InteractComponent>().AreaImIn = null;
        }
    }

    public void Interact(GameObject player)
    {
        FunctionIDO(false, player);
    }

    public void InteractWithTool(string tool, GameObject player)
    {
       if (toolUsed == tool && fasterWithTool)
       {
           FunctionIDO(true,player);
       }
       else
       {
           FunctionIDO(false,player);
       }

    }

    //Perhaps we could use a UnityEvent here as so designers and developers can add their own functions to the interactable objects with ease - MW
    //idk how i feel about the bool faster bit, for now the unity event will hold the speed but may change later - MW
    private void FunctionIDO(bool faster, GameObject player)
    {
        int speed = faster ? 1 : 3;
        //invoke the event and pass the source object as the player
        OnInteract?.Invoke(player, speed);

    }

    public void StartPopUp(GameObject source, float time)
    {
        IEnumerator pop = PopUpTest(source, time);
        StartCoroutine(pop);
    }
    
    IEnumerator PopUpTest(GameObject player, float sec)
    {
        PopUp.SetActive(true);
        player.GetComponent<PlayerControler>().disableMovement();
        yield return new WaitForSeconds(sec);
        PopUp.SetActive(false);
        player.GetComponent<PlayerControler>().interacting = false;
        player.GetComponent<PlayerControler>().enableMovement();
    }

    public void InteractCancel()
    {
        StopAllCoroutines();
        PopUp.gameObject.SetActive(false);
    }
}
