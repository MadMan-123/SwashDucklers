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
        if (!obj.CompareTag("Player")) return;

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
        players.Remove(obj);
        obj.GetComponent<PlayerControler>().ToggleCamera(true);

    }

   
}
