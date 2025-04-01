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

    void OnEnable()
    {
        for (var i = 0; i < tentacleObjects.Length; i++)
        {
            var obj = tentacleObjects[i];
            obj.SetActive(false);
        }

        tentIndex = Random.Range(0, tentacleObjects.Length);

        var routine = EnableTentacle();

        StartCoroutine(routine);
        //lastCalled = tentAttack;
    }


    /*
    private IEnumerator GetNewTentacle()
    {
        //wait some time before picking the new tentacle
    }
    */
    
    private IEnumerator EnableTentacle( float animationTime = 0.5f)
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
        for (var i = 0; i < tentacleObjects.Length; i++)
        { 
            tentacleObjects[i].SetActive(false);
        }
    }
}