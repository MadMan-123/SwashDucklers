using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Kraken : MonoBehaviour
{
    //This script is for controlling the kraken body and animations of the cosmetic tentacles


    [SerializeField] GameObject eye;
    [SerializeField] GameObject eyeTarget;
    [SerializeField] PlayerManager pm;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnEnable()
    {
        //KrakenWater();
    }

    private void OnDisable()
    {
        //NormalWater();
    }

    public void EyeFollow()
    {
        for (int i = 0;i < pm.players.Count;i++)
        {
            if(pm.players[i].TryGetComponent(out Health hp))
            {
                var temp = hp.GetHealth();
               if (temp > eyeTarget.GetComponent<Health>().GetHealth())
                {
                    eyeTarget = pm.players[i];
                }
            }
        }
        transform.LookAt(eyeTarget.transform.position);
        //var lookPos = eyeTarget.transform.position - transform.position;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * damping);
        EyeFollow();
    }
}
