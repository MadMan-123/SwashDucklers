using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    //reference to the launcher
    public Launcher launcher;
    public Transform boat;

    public void HoldPlayer(GameObject obj)
    {
        
    }

    IEnumerator StartLaunch(GameObject obj,float time = 5)
    {
        yield return new WaitForSeconds(time);
        launcher.LaunchObject(obj);
    }

   
}
