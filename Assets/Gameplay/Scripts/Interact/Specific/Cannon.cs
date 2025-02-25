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
    
    [SerializeField] private ParticleSystem cannonballParticles;
    [SerializeField] private float launchDuration = 1f;
    [SerializeField] private float strength = 10f;
    [SerializeField] private bool canFire = true;
    [SerializeField] private float coolDownTime = 5f;
    private Animation anim;


    void Start()
    {
        cannonballPool = new GameObjectPool(cannonballPrefab, 10,transform);
        if (OnInteract == null)
        {
            //    OnInteract = new UnityEvent<GameObject>();
        } 
        cannonballParticles = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        anim = this.transform.GetChild(1).gameObject.GetComponent<Animation>();
    }

    public void Fire(GameObject Source)
    {
        if (!canFire) return;
        canFire = false;
        
        //get rid of the cannonball
        if (!Source.TryGetComponent(out Inventory inv))
        {
            canFire = true;
            return;
        }
        
        //remove the cannonball from the inventory
        inv.RemoveItem();
        
        //fire the cannon
        //play some vfx
        //play some sfx
        //launch the cannonball
        var cannonball = cannonballPool.GetObject();
        cannonball.transform.position = cannonballSpawnPoint.position;
        cannonball.transform.rotation = cannonballSpawnPoint.rotation;
        cannonballParticles.Play();
        anim.Play();
        SoundManager.PlayAudioClip("CannnonFire", this.transform.position, 1f);

        if (cannonball.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(cannonballSpawnPoint.forward * strength,ForceMode.VelocityChange);
        }
        
        StartCoroutine(CoolDown());
        

    }
    
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canFire = true;
    }

    private void OnDrawGizmos()
    {
        //draw the trajectory of the cannonball
        LaunchManager.DrawTrajectory(cannonballSpawnPoint.position, cannonballSpawnPoint.forward * strength,launchDuration);
    }
}
