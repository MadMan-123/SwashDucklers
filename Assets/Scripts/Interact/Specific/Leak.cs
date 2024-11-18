using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    [SerializeField] private int Damage = 1;
    [SerializeField] private int Repair = 1;
    [SerializeField] private ShipHealth health;
    private void Start()
    {
        health = FindObjectOfType<ShipHealth>();
        //effect the ship health


        health.DamageShip(Damage);
        //health.dmgRate += leakAmmount;

    }

    private void OnDisable()
    {
        //health.dmgRate -= leakAmmount;
        health.RepairShip(Repair);
    }
}
