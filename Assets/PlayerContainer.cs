using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    //reference to the launcher
    public Launcher launcher;
    List<GameObject> players = new ();

    public void HoldPlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player") && !players.Contains(obj)) return;

        //this makes sense here
        obj.GetComponent<PlayerControler>().ToggleCamera(false);
        players.Add(obj);

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
        obj.GetComponent<PlayerControler>().ToggleCamera(true);
        
        //designed and implemented by daniel doyle (the doyleson (john swashduckler))
        yield return new WaitForSeconds(time);
        players.Remove(obj);

    }

   
}
