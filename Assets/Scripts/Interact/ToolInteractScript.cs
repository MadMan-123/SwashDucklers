using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInteractScript : InteractArea
{
    [SerializeField]string myTool;
    
    //Clears whatevers in the players tool slot and replaces it with this
    //changed the system so it not just holds a function the on interact will call - MW
    public void SetAndRemoveItem(GameObject player,float speed)
    {
        if (player.TryGetComponent(out InteractComponent comp))
        {
            comp.tool = myTool;
            comp.inArea = false;
        }
        //gameObject.SetActive(false); - Set false then destroy it? no need for this line if we destroy it MW
        Destroy(gameObject);
    }
}
