using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInteractScript : InteractArea
{
    [SerializeField]string myTool;
    
    //Clears whatevers in the players tool slot and replaces it with this
    public override void FunctionIDO(bool faster, GameObject player)
    {
        player.GetComponent<InteractComponent>().tool = myTool;
        //gameObject.SetActive(false); - Set false then destroy it? no need for this line if we destroy it MW
        Destroy(gameObject);
    }
}
