using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class Crabcontroller : MonoBehaviour
{
    public bool hasplank;
    public NavMeshAgent agent;
    public GameObject[] planks;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;



    //States
    public float sightRange;
    public bool InSightRange;
    public int target;

    public bool planktake;
    // Start is called before the first frame update
    void Awake()
    {

        target = Random.Range(0, planks.Length  +1);
     
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    { 
        planks = GameObject.FindGameObjectsWithTag("HoleLocate");
           player = GameObject.FindWithTag("Player").transform;
       // planks = GameObject.FindWithTag("HoleLocate").transform;

        InSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!InSightRange && !hasplank) GoPlank();
        if (InSightRange && !hasplank ) RunAway();
        if (hasplank) OverBoard();


       // var dist = Vector3.Distance(planks.gameObject.transform.position, agent.transform.position);

       // if (dist  < 0.5)
        {
       //     planktake = true;
        }

        }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HoleLocate" &&  planktake )
        {
            hasplank = true;
        }
    }


    private void GoPlank()
    {
       
        
        agent.SetDestination(planks[target].gameObject.transform.position);
    }

    private void RunAway()
    {
        Vector3 runTo = transform.position + ((transform.position - player.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))));
        float distance = Vector3.Distance(transform.position, player.position);
        agent.speed = Random.Range(7.5f, 11f);
        agent.SetDestination(runTo);
    }

    private void OverBoard()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
