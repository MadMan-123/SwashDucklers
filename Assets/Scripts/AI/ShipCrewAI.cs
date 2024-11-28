using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class ShipCrewAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public bool walking;
    //move around ship
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;



    private void Awake()
    {
        walking = true;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (walking) RunAround();

    }

    private void RunAround()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

 }
