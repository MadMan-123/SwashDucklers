using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannnonBall : MonoBehaviour
{
    public float damage = 1;
    [SerializeField]private GameObject impactAnim;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main?.gameObject.transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Health hp))
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
