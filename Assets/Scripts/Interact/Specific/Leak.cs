using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    [SerializeField] private float leakAmmount = 1f;
    [SerializeField] private ShipHealth health;
    private void Start()
    {
        health = FindObjectOfType<ShipHealth>(); 
        //effect the ship health
        health.dmgRate += leakAmmount;
    }

    private void OnDisable()
    {
        health.dmgRate -= leakAmmount; 
    }
}
