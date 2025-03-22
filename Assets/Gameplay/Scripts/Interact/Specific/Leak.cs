using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Leak : Interactable 
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject repairAnim;
    [SerializeField] private int damage = 1;
    [SerializeField] private int toRepair = 1;
    [SerializeField] private int count = 0;
    [SerializeField] private ShipHealth health;
    [SerializeField] private PlankVisualiser plankVisualiser;
    [SerializeField] private GameObject[] leakEffect;
    Transform vfxHolder;
    [SerializeField]private int active;
    [SerializeField] private int doubleRarity = 5;
    public CinemachineTargetGroup target;
    [SerializeField] private float cameraTargetWeight=1;
    [SerializeField] private float cameraTargetRadius = 3.5f;
    private void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        cam = Camera.main?.gameObject; 
        vfxHolder = GameObject.FindWithTag("VFXHolder").transform;
    }

    private void OnEnable()
    {
        count = 0;
        foreach (GameObject effect in leakEffect) { effect.SetActive(false); }
        plankVisualiser.RemovePlank();
        var num = Random.Range(0, doubleRarity);
        active = num < 4 ? 0 : 1;
        leakEffect[active].SetActive(true);
        toRepair = active+1;
        //reset the count
        
        health = FindObjectOfType<ShipHealth>();
        //effect the ship health


        health.DamageShip(damage);
        //health.dmgRate += leakAmmount;
        target.AddMember(transform, cameraTargetWeight,cameraTargetRadius);
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
        leakEffect[active].SetActive(false);
       
        DisableLogic();
        //wait to disable the object for x amount of time
        StartCoroutine(DisableLeak());
    }

    IEnumerator DisableLeak(float wait = 5)
    {
        yield return new WaitForSeconds(wait);
        gameObject.SetActive(false);
    }

    void DisableLogic()
    {
         if(cam == null) return;
         //health.dmgRate -= leakAmmount;
         health.RepairShip(damage);
         var pos = transform.position +  new Vector3(0, 0.5f, 0);
 
         ScoreManager.Instance.AddScore(10);
          
         Vector3 lookDir = cam.transform.position - pos;
         Quaternion direction = Quaternion.LookRotation(lookDir);
         target.RemoveMember(transform);
         Instantiate(repairAnim,pos,direction);
    }

    private void OnDisable()
    {
    }
}
