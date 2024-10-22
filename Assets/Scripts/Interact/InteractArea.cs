using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour
{
    [SerializeField] public bool needTool;
    [SerializeField] bool fasterWithTool;
    [SerializeField] public bool isTool;
    [SerializeField] public bool isStation;


    [SerializeField] public string toolUsed;
    [SerializeField] GameObject PopUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<InteractComponent>().inArea = true;
            col.gameObject.GetComponent<InteractComponent>().AreaImIn = this;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<InteractComponent>().inArea = false;
            col.gameObject.GetComponent<InteractComponent>().AreaImIn = null;
        }
    }

    public void Interact(GameObject player)
    {
        FunctionIDO(false, player);
    }

    public void Interact(string tool, GameObject player)
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

    public virtual void FunctionIDO(bool faster, GameObject player)
    {
        int speed = faster ? 1 : 3;
        StartCoroutine(PopUpTest(speed, player));
    }

    IEnumerator PopUpTest(int sec, GameObject player)
    {
        PopUp.SetActive(true);
       StartCoroutine(player.GetComponent<PlayerControler>().disableMovement());
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
