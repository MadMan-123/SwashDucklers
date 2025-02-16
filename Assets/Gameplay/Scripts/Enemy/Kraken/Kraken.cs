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

    public IEnumerator EyeFollow()
    {
        eye.transform.LookAt(eyeTarget.transform.position);
        eye.transform.rotation *= Quaternion.FromToRotation(Vector3.left, Vector3.forward);
        if (eye.transform.rotation.x > 30)
        {
                        Debug.Log("clamp");

            //eye.transform.eulerAngles = new Vector3(30f,eye.transform.eulerAngles.y,eye.transform.eulerAngles.z);
            eye.transform.Rotate(30f, eye.transform.eulerAngles.y, eye.transform.eulerAngles.z);
        }
        else if (eye.transform.rotation.x < -30)
        {
            Debug.Log("clamp");
            eye.transform.Rotate(-30f, eye.transform.eulerAngles.y, eye.transform.eulerAngles.z);
        }
            yield return new WaitForSeconds(0.05f);
        StartCoroutine(EyeFollow());
    }
}
