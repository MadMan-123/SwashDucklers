using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TentacleAI : MonoBehaviour
{
    //This should control when the tentacles are spawned, disabling previous tentacle smoothly and other

    public float tentacleDeployedTime;
    public float waitBetweenTents;
    public GameObject[] tentacleObjects;
    private int lastCalled;
    private int tentIndex = 0;
    public bool shouldDebug = false;
   
    
    
    //COMMIT 500 baby
    private void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
    }

    void OnEnable()
    {
        foreach (GameObject obj in tentacleObjects)
            obj.SetActive(false);
        tentIndex = Random.Range(0, tentacleObjects.Length);
        var routine = EnableTentacle(tentIndex);

        StartCoroutine(routine);
        //lastCalled = tentAttack;
    }

    private IEnumerator EnableTentacle(int tentIndex, float animationTime = 0.5f)
    {
        var tentacle = tentacleObjects[tentIndex];
        yield return new WaitForSeconds(animationTime);
        tentacle.SetActive(true);
    }

    public IEnumerator KrakenDeath()
    {
        yield return new WaitForSeconds(1f);
        if (tentacleObjects[tentIndex].TryGetComponent(out Tentacle tentacle))
        {
            tentacle.KrakenDead();
        }
    }
    private void OnDisable()
    {
        foreach (GameObject obj in tentacleObjects)
            obj.SetActive(false);
    }

    public void ActivateHitboxes()
    {
        
    }
}