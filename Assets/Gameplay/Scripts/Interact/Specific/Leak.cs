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
    [SerializeField] GameObject bigRepairAnim;
    [SerializeField] private GameObject smallRepairAnim;
    [SerializeField] private int damage = 1;
    [SerializeField] private int toRepair = 1;
    [SerializeField] private int count = 0;
    [SerializeField] private ShipHealth health;
    [SerializeField] private PlankVisualiser plankVisualiser;
    [SerializeField] private GameObject[] leakEffect;
    public Transform vfxHolder;
    [SerializeField]private int active;
    [SerializeField] private int doubleRarity = 5;
    public CinemachineTargetGroup target;
    [SerializeField] private float cameraTargetWeight=1;
    [SerializeField] private float cameraTargetRadius = 3.5f;
    private bool start = false;
    public bool tutLeak = false;
    private void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        cam = Camera.main?.gameObject; 
    }

    private void OnEnable()
    {
        count = 0;
            
            foreach (GameObject effect in leakEffect)
            {
                effect.SetActive(false);
            }
            if (tutLeak)
            {
                leakEffect[0].SetActive(true);
                toRepair = 1;
                plankVisualiser.LeakSpawn(active);
            }
            else if (!tutLeak)
            {

            int num = Random.Range(0, doubleRarity);
            active = num < doubleRarity - 1 ? 0 : 1;

            leakEffect[active].SetActive(true);
            toRepair = active + 1;
            //reset the count
            if (start)
            {
                plankVisualiser.LeakSpawn(active);
            }

            health = FindObjectOfType<ShipHealth>();
            //effect the ship health


            health.DamageShip(damage);
            //health.dmgRate += leakAmmount;
            target.AddMember(transform, cameraTargetWeight, cameraTargetRadius);
            start = true;
        }
    }

    public void Repaired(GameObject source)
    {
        //check if the source has the required item
        if (!source.TryGetComponent(out Inventory inv) ) return;
        //if the item is the required type
        if ( inv.item.type != Item.Type.NoItem && inv.item.type != itemRequired) return;
        //increment the count
        count++;
        
        plankVisualiser.RepairPlank(count-1);
        //take the players item
        inv.RemoveItem();
        //if the count is equal to the required amount
        RepairAnim(smallRepairAnim,3);
        
        if (count != toRepair) return;
        //disable the object
        leakEffect[active].SetActive(false);
       
        DisableLogic();
        //wait to disable the object for x amount of time
        StartCoroutine(DisableLeak());
    }

    IEnumerator DisableLeak(float wait = 0.5f)
    {
        yield return new WaitForSeconds(wait);
        gameObject.SetActive(false);
    }

    void DisableLogic()
    {
        RepairAnim(bigRepairAnim);
        if (tutLeak) return;
         if(cam == null) return;
         health.RepairShip(damage);
         
         
         ScoreManager.Instance.AddScore(10);
         target.RemoveMember(transform);
    }

    void RepairAnim(GameObject repairAnim,float distance = 0)
    {
        var pos = transform.position +  new Vector3(0, 0.5f, 0);
        Vector3 lookDir = (cam.transform.position - pos).normalized;
        var vfxSpawnLoc = transform.position +(lookDir*distance);
        Quaternion direction = Quaternion.LookRotation(lookDir);
        Instantiate(repairAnim,vfxSpawnLoc,direction,vfxHolder);
    }


    private void OnDisable()
    {
    }
}
