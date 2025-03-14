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
        
        if(shouldDebug)
            Debug.Log("TentacleAI OnEnable");
        
        int tentIndex = Random.Range(0, tentacleObjects.Length);
        var routine = EnableTentacle(tentIndex);

        StartCoroutine(routine);
        //lastCalled = tentAttack;
        
        if(shouldDebug)
            Debug.Log("Activated tentacle " + tentIndex);
    }

    private IEnumerator EnableTentacle(int tentIndex, float animationTime = 0.5f)
    {
        var tentacle = tentacleObjects[tentIndex];
        
        if(shouldDebug)
            Debug.Log("Enabling tentacle " + tentIndex);
       
        //animation shit
        yield return new WaitForSeconds(animationTime);
        
        tentacle.SetActive(true);
        
        //other tentacle shit here 
        
    }

    private void OnDisable()
    {
        foreach (GameObject obj in tentacleObjects)
            obj.SetActive(false);
        //throw new NotImplementedException();
    }
}