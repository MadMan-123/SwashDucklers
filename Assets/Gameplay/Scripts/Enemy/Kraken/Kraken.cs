using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Kraken : MonoBehaviour
{
    //This script is for controlling the kraken body and animations of the cosmetic tentacles
    //Wait til all intro anims are done before moving eye


    [SerializeField] GameObject eye;
    [SerializeField] GameObject eyeTarget;
    [SerializeField] PlayerManager pm;
    [SerializeField] Vector3 playerLoc;
    [SerializeField] bool activate;
    [SerializeField] private float targetAngle = 30f;

    void Start()
    {
        activate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.players.Count > 0 && activate==true)
        {
            eyeTarget = pm.players[0];
            StartCoroutine(EyeFollow());
            activate = false;
        }

    }

    private void OnEnable()
    {
        //KrakenWater();
    }

    private void OnDisable()
    {
        //NormalWater();
    }

    private IEnumerator EyeFollow()
    {
        //get the direction to the player
        var delta = eyeTarget.transform.position - eye.transform.position;
        //get the angle from -vector3.forward to the direction and if its less than target angle then rotate
        var angle = Vector3.SignedAngle(-Vector3.forward, delta, Vector3.up);
        angle = Mathf.Abs(angle);
        if (angle < targetAngle)
        {
            eye.transform.LookAt(eyeTarget.transform.position);
            eye.transform.rotation *= Quaternion.FromToRotation(Vector3.left, Vector3.forward);
        } 
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(EyeFollow());
    }
}
