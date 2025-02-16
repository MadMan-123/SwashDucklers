using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Cannon : Interactable
{
    
    public GameObject cannonballPrefab;
    GameObjectPool cannonballPool;
    public Transform cannonballSpawnPoint;
    
    [SerializeField] private float launchDuration = 1f;
    [SerializeField] private float strength = 10f;


    
    void Start()
    {
        cannonballPool = new GameObjectPool(cannonballPrefab, 10,transform);
        if (OnInteract == null)
        {
            //    OnInteract = new UnityEvent<GameObject>();
        }
    }

    public void Fire(GameObject Source)
    {
        //get rid of the cannonball
        if(!Source.TryGetComponent(out Inventory inv)) return;
        
        //remove the cannonball from the inventory
        inv.RemoveItem();
        
        //fire the cannon
        //play some vfx
        //play some sfx
        //launch the cannonball
        var cannonball = cannonballPool.GetObject();
        cannonball.transform.position = cannonballSpawnPoint.position;
        cannonball.transform.rotation = cannonballSpawnPoint.rotation;
        SoundManager.PlayAudioClip("CannnonFire", this.transform.position, 1f);

        if (cannonball.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(cannonballSpawnPoint.forward * strength,ForceMode.VelocityChange);
        }

    }

    private void OnDrawGizmos()
    {
        //draw the trajectory of the cannonball
        LaunchManager.DrawTrajectory(cannonballSpawnPoint.position, cannonballSpawnPoint.forward * strength,launchDuration);
    }
}
