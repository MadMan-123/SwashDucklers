using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    //reference to the launcher
    public Launcher launcher;
    List<GameObject> players = new ();

    
    //this method is being called twice when it should only be called once
    
    public void HoldPlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player") || players.Contains(obj)) return;
        
        players.Add(obj);
        print("Start Launch");
        //this makes sense here
        if(obj.TryGetComponent(out PlayerControler controler))
        {
            //disable the camera
            controler.ToggleCamera(false);
            //disable the player
            controler.canMove = false;
        }

        //disable the rigidbody
        if (obj.TryGetComponent(out Rigidbody rb))
        {   
            rb.isKinematic = true;
        }

        var routine = StartLaunch(obj,rb);
        
        StartCoroutine(routine);     
    }

    IEnumerator StartLaunch(GameObject obj,Rigidbody rb,float time = 5)
    {
        yield return new WaitForSeconds(time);
        launcher.LaunchObject(obj);
        rb.isKinematic = false;
        yield return new WaitForSeconds(time);

        if (obj.TryGetComponent(out PlayerControler controler))
        {
            controler.ToggleCamera(true);
            controler.canMove = true;
            rb.isKinematic = false; 
        }
        //designed and implemented by daniel doyle (the doyleson (john swashduckler))
        yield return new WaitForSeconds(1);
        players.Remove(obj);
        
    }

   
}
