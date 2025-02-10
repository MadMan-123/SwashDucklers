using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannnonBall : MonoBehaviour
{
    public float damage = 1;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Health hp))
        {
            hp.TakeDamage(gameObject, damage);
            //return to the pool
            gameObject.SetActive(false);
            
        }
    }
}
