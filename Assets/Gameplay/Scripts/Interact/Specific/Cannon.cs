using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Cannon : Interactable
{
    private static readonly int IsShooting = Animator.StringToHash("IsShooting");

    public GameObject cannonballPrefab;
    GameObjectPool cannonballPool;
    public Transform cannonballSpawnPoint;
    
    [SerializeField] private ParticleSystem cannonballParticles;
    [SerializeField] private float launchDuration = 1f;
    [SerializeField] private float strength = 10f;
    [SerializeField] private bool canFire = true;
    [SerializeField] private float coolDownTime = 5f;
    [SerializeField] private Animator anim;
    
    [SerializeField]private int cannonballCount = 0, jamAmmount = 3, timeToUnJam = 12;

    void Start()
    {
        cannonballPool = new GameObjectPool(cannonballPrefab, 10,transform);
        if (OnInteract == null)
        {
            //    OnInteract = new UnityEvent<GameObject>();
        } 
        cannonballParticles = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        anim = this.transform.gameObject.GetComponent<Animator>();
    }

    public void Fire(GameObject Source)
    {

        if (!canFire) return;
        canFire = false;
        
        //get rid of the cannonball
        if (!Source.TryGetComponent(out Inventory inv) && cannonballCount < jamAmmount)
        {
            canFire = true;
            return;
        }
        
        //remove the cannonball from the inventory
        inv.RemoveItem();
        
        StartCoroutine(FireCannon());
        if(cannonballCount + 1 < jamAmmount)
        {
            StartCoroutine(CoolDown());
        }
        cannonballCount++;
    
        if(cannonballCount >= jamAmmount)
        {
            
            StartCoroutine(UnJam());
            cannonballCount = 0;
            
        }
    }

    IEnumerator UnJam()
    {
        canFire = false;
        anim.SetBool(IsShooting, false);
        yield return new WaitForSeconds(timeToUnJam);
        canFire = true;
    }
     
    IEnumerator FireCannon()
    {
       if(cannonballCount >= jamAmmount) yield break; 
        anim.SetBool(IsShooting, true);
        yield return new WaitForSeconds(0.75f);

        //fire the cannon
        //play some vfx
        //play some sfx
        //launch the cannonball
        var cannonball = cannonballPool.GetObject();
        cannonball.transform.position = cannonballSpawnPoint.position;
        cannonball.transform.rotation = cannonballSpawnPoint.rotation;
        cannonballParticles.Play();

        SoundManager.PlayAudioClip("CannnonFire", this.transform.position, 1f);

        if (cannonball.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(cannonballSpawnPoint.forward * strength, ForceMode.VelocityChange);
        }

        

    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        if(cannonballCount < jamAmmount)
            canFire = true;
        else
            yield break;
        

        anim.SetBool("IsShooting", false);
    }

    private void OnDrawGizmos()
    {
        //draw the trajectory of the cannonball
        LaunchManager.DrawTrajectory(cannonballSpawnPoint.position, cannonballSpawnPoint.forward * strength,launchDuration);
    }
}
