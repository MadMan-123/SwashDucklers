using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Kraken : MonoBehaviour
{
    //This script is for controlling the kraken body and animations of the cosmetic tentacles
    //Wait til all intro anims are done before moving eye

    [SerializeField] private Animator tFront;
    [SerializeField] private Animator tBack;
    private void OnEnable()
    {
        tFront.SetTrigger("Spawn");
        tBack.SetTrigger("Spawn");
    }

    public IEnumerator KrakenDead()
    {
        yield return new WaitForSeconds(1f);
        tFront.SetTrigger("KrakenDead");
        tBack.SetTrigger("KrakenDead");
    }
    private void OnDisable()
    {
        
    }
    
}
