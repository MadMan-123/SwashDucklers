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
    [SerializeField] bool activate;

    [SerializeField]private float range = 30;
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
        //get the angle from the eye to the target
        //the direction we need is -transform.forward, the models weird
        var targetPosition = eyeTarget.transform.position;
        var eyePosition = eye.transform.position;
        var direction = targetPosition - eyePosition;
    
        var targetRotation = Quaternion.Euler(0f, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f, 0f);
        var angle = Vector3.Angle(-transform.forward, direction);
        if (Mathf.Abs(angle) <= range)
        {
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(EyeFollow());
    }
}
