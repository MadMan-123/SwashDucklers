using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannnonBall : MonoBehaviour
{
    public float damage = 1;
    [SerializeField]private GameObject impactAnim;
    private Transform cam;


    private int krakenLayer = -1;
    
    private void Start()
    {
        cam = Camera.main?.gameObject.transform;
        
        //get the kraken layer
        krakenLayer = LayerMask.NameToLayer("Kraken");
    }
    private void OnCollisionEnter(Collision other)
    {
        bool isKraken = (other.gameObject.layer == krakenLayer);
        //check if there is a valid health component and if the other is a kraken
        if (other.gameObject.TryGetComponent(out Health hp) && isKraken)
        {
            Vector3 lookDir = (cam.transform.position - gameObject.transform.position).normalized;
            Quaternion direction = Quaternion.LookRotation(lookDir);
            Instantiate(impactAnim,gameObject.transform.position,direction);
            hp.TakeDamage(gameObject, damage);
            //return to the pool
            gameObject.SetActive(false);

        }
    }
}
