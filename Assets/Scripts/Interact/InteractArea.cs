using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour
{

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

    public void FunctionIDO()
    {
        StartCoroutine(PopUpTest());
    }

    IEnumerator PopUpTest()
    {
        PopUp.SetActive(true);
        yield return new WaitForSeconds(3);
        PopUp.SetActive(false);
    }
}
