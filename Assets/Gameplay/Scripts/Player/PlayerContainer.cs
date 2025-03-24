using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    //reference to the launcher
    public Launcher launcher;
    //reference to the trigger area
    public TriggerArea triggerArea;
    //list of players
    public List<GameObject> players = new ();

    
    //this method is being called twice when it should only be called once
    public void HoldPlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player") || players.Contains(obj)) return;
       
        //TODO: GET SOME VALUE OF THE ACTUAL PLAYER COUNT
        if (players.Count >= 4)
        {
            //die
        }
        
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

    IEnumerator StartLaunch(GameObject obj,Rigidbody rb,float time = 3)
    {
        //wait for the time
        yield return new WaitForSeconds(time);
        
        //launch the object
        launcher.LaunchObject(obj);
        
        
        //enable the rigidbody
        rb.isKinematic = false;
        
        //remove the object from the list of players
        if(triggerArea.tracked.Contains(obj))
            triggerArea.tracked.Remove(obj);
        
        //disable the collider
        if (obj.TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(time - 1.5f);
        //enable the collider
        col.enabled = true;
        yield return new WaitForSeconds(time);

        //ensure the agent is ready
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
