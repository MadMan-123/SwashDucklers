using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : Spawner
{
    public int maxSpawnSize = 3;
    public int spawnedCount = 0;
   public override void Init()
   {
         pool = new GameObjectPool(prefab, poolSize, transform);
         
         //go through the pool and get the AI brains, then 
         //dependency inject the spawner as so we can later remove the count from how many spawned
         for (var index = 0; index < pool.Count; index++)
         {
             var obj = pool[index];
             if (obj.TryGetComponent(out AIBrain ai))
             {
                 ai.owner = this;
             }
         }


         routine = _spawn();
         StartCoroutine(routine);
         
         
   }
   
   private IEnumerator _spawn()
    {
        //check if we have reached the max spawn size
        if (spawnedCount >= maxSpawnSize)
        {
            yield return new WaitForSeconds(interval);
            routine = _spawn();
            StartCoroutine(routine);
            //exit the coroutine
            yield break;
        }
        area = AreaManager.GetArea(areaName);
        
 
        for (int i = 0; i < waveSize; i++)
        {
            var obj = pool.GetObject();
            
            
            
            
            //disable the agent and enable kinematic
            if (obj.TryGetComponent(out NavMeshAgent agent))
            {
                agent.enabled = false;
            }

            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }

            //reenable the agent and disable kinematic
            StartCoroutine(_reenable(obj,agent));
             
            
            //get the position
            var pos = area.GeneratePositionInArea(true,true,true);   
            
            //clamp the position to the navmesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
            {
                pos = hit.position;
            }
            
            //set the position
            obj.transform.position = pos;
            //draw the position
            Debug.DrawLine(obj.transform.position,obj.transform.position + Vector3.up,Color.red,5f);
            spawnedCount++;

        }
        yield return new WaitForSeconds(interval);
        routine = _spawn();
        StartCoroutine(routine);
    }


    public override void Return(GameObject gameObject)
    {
        base.Return(gameObject);
        spawnedCount--;
    }

    private IEnumerator _reenable(GameObject aiInstance, NavMeshAgent agent) 
    {
        while (!Physics.Raycast(aiInstance.transform.position, Vector3.down, 0.5f,LayerMask.GetMask("Boat")))
        {
            yield return null;
        }
       
        // clamp the agent to the navmesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(aiInstance.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            aiInstance.transform.position = hit.position;
        }
        //  enable NavMeshAgent
        if (agent) agent.enabled = true;
        
    }
}
