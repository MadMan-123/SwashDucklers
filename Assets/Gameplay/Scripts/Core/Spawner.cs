using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    public float interval = 1f;
    public int waveSize = 1;
    public string areaName;
    private AreaManager.Area area;
    private GameObjectPool pool;
    
    
    private IEnumerator routine;
    private void Start()
    {
        pool = new GameObjectPool(prefab, poolSize,transform);
        pool.Dynamic = true;
        routine = _spawn();
        StartCoroutine(routine);
    }


    private IEnumerator _spawn()
    {
        
        area = AreaManager.GetArea(areaName);
        
 
        for (int i = 0; i < waveSize; i++)
        {
            var obj = pool.GetObject();
            
            //disable the agent and enable kinematic
            NavMeshAgent agent;
            if (obj.TryGetComponent(out agent))
            {
                agent.enabled = false;
                
            }

            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }

            //reenable the agent and disable kinematic
            StartCoroutine(_reenable(obj,agent));
            
            //set the position
            obj.transform.position = area.GeneratePositionInArea(true,true,true);   
            //draw the position
            Debug.DrawLine(obj.transform.position,obj.transform.position + Vector3.up,Color.red,5f);
            
            
        }
        yield return new WaitForSeconds(interval);
        routine = _spawn();
        StartCoroutine(routine);
    }

    private IEnumerator _reenable(GameObject aiInstance, NavMeshAgent agent) 
    {
        while (!Physics.Raycast(aiInstance.transform.position, Vector3.down, 0.5f,LayerMask.GetMask("Boat")))
        {
            yield return null;
        }
        
        //  enable NavMeshAgent
        if (agent) agent.enabled = true;
        
    }

    public void Return(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out NavMeshAgent agent))
        {
        }

        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }
        
        pool.ReturnObject(gameObject);
    }
    

}
