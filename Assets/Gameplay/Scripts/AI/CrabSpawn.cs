using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabSpawn : MonoBehaviour
{
    public GameObject prefab;
    public int SpawnTimeMax = 10;
    public int SpawnTimeMin = 1;
    public float interval = 10;
    public int waveSize = 1;
    public GameObject[] crab_sp;



    private IEnumerator routine;
    private void Start()
    {

        routine = _spawn();
        StartCoroutine(routine);
    }


    private IEnumerator _spawn()
    {




        for (int i = 0; i < waveSize; i++)
        {
          
            int randomIndex = Random.Range(0, crab_sp.Length);
            Vector3 spawnPos = crab_sp[randomIndex].transform.position;
            

            //disable the agent and enable kinematic
            NavMeshAgent agent;
            if (prefab.TryGetComponent(out agent))
            {
                agent.enabled = false;

            }

            if (prefab.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }

            //reenable the agent and disable kinematic
            StartCoroutine(_reenable(prefab, agent));

            //set the position
            Instantiate(prefab, spawnPos, transform.rotation);

        }
        yield return new WaitForSeconds(interval + Random.Range(SpawnTimeMin, SpawnTimeMax));
        routine = _spawn();
        StartCoroutine(routine);
    }

    private IEnumerator _reenable(GameObject aiInstance, NavMeshAgent agent)
    {
        while (!Physics.Raycast(aiInstance.transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Boat")))
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

    }

}
