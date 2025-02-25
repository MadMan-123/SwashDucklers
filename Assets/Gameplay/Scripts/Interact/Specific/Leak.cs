using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Leak : Interactable 
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject repairAnim;
    [SerializeField] private int damage = 1;
    [SerializeField] private int toRepair = 1;
    [SerializeField] private int count = 0;
    [SerializeField] private ShipHealth health;
    [SerializeField] private PlankVisualiser plankVisualiser;
    Transform vfxHolder;
    private void Start()
    {
        cam = Camera.main?.gameObject; 
        vfxHolder = GameObject.FindWithTag("VFXHolder").transform;

        
    }

    private void OnEnable()
    {

        //reset the count
        count = 0;
        
        health = FindObjectOfType<ShipHealth>();
        //effect the ship health


        health.DamageShip(damage);
        //health.dmgRate += leakAmmount;

    }

    public void Repaired(GameObject source)
    {
        //check if the source has the required item
        if (!source.TryGetComponent(out Inventory inv) ) return;
        //if the item is the required type
        if ( inv.item.type != Item.Type.NoItem && inv.item.type != itemRequired) return;
        //increment the count
        count++;
        
        plankVisualiser.RepairPlank();
        //take the players item
        inv.RemoveItem();
        //if the count is equal to the required amount
        if (count != toRepair) return;
        //disable the object
        gameObject.SetActive(false);

    }


    void DisableLogic()
    {
         if(cam == null) return;
         //health.dmgRate -= leakAmmount;
         health.RepairShip(damage);
         var pos = transform.position +  new Vector3(0, 0.5f, 0);
 
          
         Vector3 lookDir = cam.transform.position - pos;
         Quaternion direction = Quaternion.LookRotation(lookDir);
         Instantiate(repairAnim,pos,direction);
    }

    private void OnDisable()
    {
        DisableLogic();
    }
}
