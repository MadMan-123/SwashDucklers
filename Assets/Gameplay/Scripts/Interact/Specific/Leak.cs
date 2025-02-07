using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject repairAnim;
    [SerializeField] private int Damage = 1;
    [SerializeField] private int Repair = 1;
    [SerializeField] private ShipHealth health;
    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }

    private void OnEnable()
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
        Vector3 iPosition = new Vector3(this.transform.position.x,this.transform.position.y+0.5f,this.transform.position.z);
        Vector3 lookDir = cam.transform.position - iPosition;
        Quaternion direction = Quaternion.LookRotation(lookDir);
        Instantiate(repairAnim,iPosition,direction);
    }
}
