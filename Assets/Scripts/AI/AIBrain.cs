using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private float fov = 90;
    [SerializeField] private float viewRadius = 10;
    public NavMeshAgent agent;
    public Rigidbody rb;
    private Health health;
    [SerializeField] private Transform target;
    [SerializeField] private State state;
    private bool shouldUpdate = true;
    Vector3 delta;
    [SerializeField] private float circleDistance = 5f;
    [SerializeField] private float randDifference = 90f;
    [SerializeField] private float circleRadius = 5f;
    [SerializeField] private Collider[] colliders = new Collider[10];
    [SerializeField] public float knockDownTime = 5f;
    [SerializeField] private bool shouldDebug = false;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackRadius = 0.75f;
    [SerializeField] private float damage = 5;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float cooldownTime = 3f;
    Inventory inv;
    private LayerMask boatLayer;

    public enum State
    {
        Idle,
        Chase,
        Attack,
        Flee,
        Wander
    }

    void Start()
    {
        startFlag = false;
        boatLayer = LayerMask.NameToLayer("Boat");
        state = State.Wander;
        if (TryGetComponent(out agent))
        {
            agent.enabled = true;
        }

        if (TryGetComponent(out rb))
        {
            rb.isKinematic = false;
        }

        if (TryGetComponent(out health))
        {
            health.SetHealth(health.GetMaxHealth());
        }

        if (TryGetComponent(out inv))
        {

        }

    }

    private void Update()
    {
        if (!agent.enabled && !startFlag)
        {
            startFlag = true;
            //if the agent is not enabled wait to be on the floor
            StartCoroutine(WaitUntilFloorHit());
        }

        if (agent.enabled)
        {
            var layerMask = LayerMask.GetMask("player");
            var count = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, colliders, ~layerMask);
            
            
            if(count > 0)
            {
                target = FindBestTarget(colliders);
            
                if (target)
                {
                    Debug.LogError(target.name);
                    var distance = (target.position - transform.position).magnitude;
                    //Judge what state to be in
            
                    //if the health is low, flee
                    if (health.GetHealth() >= health.GetMaxHealth() / 2)
                    {
                        ChangeState(State.Flee);
            
                    }
            
                    //if we are in attack range, attack
                    if (distance < attackDistance)
                    {
                        ChangeState(State.Attack);
            
                    }
                        
                    //if we are not chase
                    if (distance < viewRadius)
                    {
                        ChangeState(State.Chase);
            
                            
                    }    
                }
            }
        }

        Vector3 destination;
        if (target)
            delta = target.position -
                    transform.position; //How i didnt realise this was the wrong way round, we will never know


        if (target)
        {
            destination = state switch
            {
                State.Idle => Idle(),
                State.Chase => Chase(),
                State.Attack => Attack(),
                State.Flee => Flee(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
        }
        else if(state == State.Wander)
        {
            destination = Wander();
        }
        else
        {
            destination = Idle();
        }
            
        



        //flocking behaviour
        /*Vector3 flockOffset = Vector3.zero; //will be the offset from the destination as so AI can flock   

        //determine the AI near us
        var aiCount = Physics.OverlapSphereNonAlloc(transform.position, 5f, colliders);
        var aiList = new List<AIBrain>();

        float avgDistance = 0f;
        //loop through the colliders
        for (int i = 0; i < aiCount; i++)
        {
            //if the collider is an AI
            if (colliders[i].TryGetComponent(out AIBrain ai))
            {
                avgDistance += (transform.position - colliders[i].transform.position).magnitude;
                //add the AI to the list
                aiList.Add(ai);
            }
        }

        avgDistance /= aiList.Count;

        float sepWeight = 0;
        float cohWeight = 0;
        float sepThreshold = 4f;

        if (avgDistance < sepThreshold)
        {
            sepWeight = 1f;
            cohWeight = 0.5f;
        }
        else
        {
            sepWeight = 0.5f;
            cohWeight = 1f;
        }*/

        /*var sep = Seperation(aiList) ;
        var coh = Cohesion(aiList);

        var deltaSep = (sep - transform.position);
        var deltaCoh = (coh - transform.position);

        //var ali = Alignment();

        flockOffset += deltaSep * sepWeight;
        flockOffset += deltaCoh * cohWeight;*/
            
        //Debug.DrawRay(transform.position, deltaSep, Color.red, 0.1f);
        //Debug.DrawRay(transform.position, deltaCoh, Color.blue, 0.1f);
            
           
        //destination += flockOffset * flockStrength;
            

        var newDelta = (destination - transform.position);

        AvoidObstacles();
        //clamp the destination to the navmesh
        if (NavMesh.SamplePosition(destination, out var hit, 5f, NavMesh.AllAreas))
        {
            destination = hit.position;
                
            agent.SetDestination(destination);
        }
        else
        {
                
        }

            
        
        if(shouldDebug)
            Debug.DrawRay(transform.position, newDelta, Color.red, 0.1f);

    }
    
    
     private Transform FindBestTarget(Collider[] potentialTargets)
     {
         Transform bestTarget = null;
         var shortestDistance = float.MaxValue;
         for (int i = 0; i < potentialTargets.Length; i++)
         {
             if (CanSee(potentialTargets[i].transform) && 
                 potentialTargets[i].CompareTag("Player") && 
                 potentialTargets[i].TryGetComponent(out Inventory inv) && 
                 inv.item)
             {
                 float distance = Vector3.Distance(transform.position, potentialTargets[i].transform.position);
                 if (distance < shortestDistance)
                 {
                     shortestDistance = distance;
                     bestTarget = potentialTargets[i].transform;
                     
                 }
                 
             }
             
         }
         
     
         return bestTarget;
    }
     
    
    [SerializeField] private float cohesionRadius = 4f;
    private Vector3 Cohesion(List<AIBrain> aiList)
    {
        //get the average position of the AI
        var result = Vector3.zero;
        var count = 0;
        for(int i = 0; i < aiList.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, aiList[i].transform.position);
            
            if (distance < cohesionRadius)
            {
                Debug.DrawLine(transform.position,agent.transform.position,Color.blue,0.1f);
                result += aiList[i].transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            result /= count;
        }

        return result;
    }

    [SerializeField] float separationRadius = 4f;
    private Vector3 Seperation(List<AIBrain> brains)
    {
        var result = Vector3.zero;
        var count = 0;
        for (int i = 0; i < brains.Count; i++)
        {
            var newBrain = brains[i];

            var distance = Vector3.Distance(transform.position, newBrain.transform.position);
            if(distance < separationRadius)
            {
                
                Debug.DrawLine(transform.position,newBrain.transform.position,Color.red,0.1f);
                var delta = transform.position - agent.transform.position;
                delta *= 1 / distance;

                //delta set magnitude tto 1 / distance
                result += delta;
                count++;
            }
            
        }
        if (count > 0)
        {
            result *= separationRadius;
        }

        return result;
    }

        private IEnumerator WaitUntilFloorHit()
        {
            //wait untill there is a navmesh under the agent
            while (!NavMesh.SamplePosition(
                       transform.position, 
                       out var data,
                       1f,
                       NavMesh.AllAreas
                       ))
            {
                yield return null;
            }
            agent.enabled = true;
            rb.isKinematic = true;
        }

        private Vector3 AvoidObstacles()
        {
            //judge if there is something infront of the AI or they are looking off the edge of the navmesh
            if (Physics.Raycast(transform.position, transform.forward, 1f, boatLayer) ||
                !NavMesh.SamplePosition(transform.position + transform.forward, out var hit, 0.5f, NavMesh.AllAreas))
            {
                //turn all the way around by 180 degrees
                transform.Rotate(0, 180, 0);
                //draw the new direction and the ray to the obstacle or edge
                Debug.DrawRay(transform.position, transform.forward, Color.red, 0.1f);
            }
            return agent.destination;

        }

        private Vector3 Flee()
        {
             
            //find a point that is opposite to the target
            return transform.position - delta;
    
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private Vector3 Attack()
        {
            if (delta.magnitude > attackDistance)
            {
                if (!CanSee(target))
                {
                    ChangeState(State.Wander);
                    return Wander();
                }
                ChangeState(State.Chase);
                return target.position;
            };
            
            if (!canAttack) return transform.position;
            ResetFlag(); 
            
            var destination = delta * 0.85f;
            //do attack
            Collider[] attackCol = new Collider[10];
            
            var count = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * 0.5f,attackRadius, attackCol);
            for (int i = 0; i < count; i++)
            {
                if(attackCol[i] == GetComponent<Collider>()) continue; 
                //if the target has an inventory
                if (attackCol[i].TryGetComponent(out Inventory colInv) && colInv.item)
                {
                    colInv.TakeItem(inv);
                    ChangeState(State.Flee);
                }
                
                if(attackCol[i].TryGetComponent(out Health health))
                {
                    health.TakeDamage(gameObject,damage);
                    //apply knockback
                    health.ApplyKnockback(transform.forward);
                }
                
                
            } 
            //clamp the destination
            Invoke(nameof(ResetFlag),cooldownTime);
            
            return destination;
        }

        private Vector3 Chase()
        {
            var cache = target.transform.position - transform.position;
            if(cache.magnitude < attackDistance)
            {
                ChangeState(State.Attack);
                return transform.position;
            }

            return target.position;
        }

        private void ResetFlag()
        {
            canAttack = !canAttack;
        }
        
        
        private float wanderAngle = 0f;
        private bool startFlag = false;
        [SerializeField] private float flockStrength = 0.5f;


        private Vector3 Wander()
        {
            if (target)
            {
                ChangeState(State.Chase);
                return Chase();
            }
            wanderAngle += Random.Range(-randDifference, randDifference) * Mathf.Deg2Rad;
            var circlePos = transform.position + (transform.forward * circleDistance);
            
            var offsetX = Mathf.Cos(wanderAngle) * circleRadius;
            var offsetZ = Mathf.Sin(wanderAngle) * circleRadius;

            var targetPos = new Vector3(
                circlePos.x + offsetX,
                transform.position.y,
                circlePos.z + offsetZ);
            
            
            
            return targetPos;
        }
        private Vector3 Idle()
        {

            return transform.position;
        }

        public void ChangeState(State newState)
        {
            state = newState;
            
        }
        public  Vector3 ConstraintPointToCircle(Vector3 position, Vector3 circlePosition, float radius)
        {
            //sum initialisation 
            Vector3 sum = Vector3.zero;
		
            //first figure out the delta between each x and y axis
            Vector3 delta = circlePosition - position;

            //pass the delta then work out the angle (theta) to a constrained position on the surface of the circle 
            float theta = Mathf.Atan2(delta.z, delta.x);

            //make the sum return the circle pos + radius then multiply by cos(theta) to return the constrained position on the circle
            sum.x = circlePosition.x + radius * Mathf.Cos(theta);
            //do the same for the y axis but with sin(theta)
            sum.z = circlePosition.z + radius * Mathf.Sin(theta);

            return sum;

        } 
        public bool CanSee(Transform transformToSee)
        {
            Vector3 directionToTarget = transformToSee.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the target is within the field of view
            if (angle <= fov / 2f)
            {


                return true;
            }

            return false;
        }

        public void ToggleAttack(GameObject source)
        {
            if(source.CompareTag("Player"))
                target = source.transform;
            ChangeState(State.Attack);
        }
        public void DisableThenEnable(float sec)
        {
            StartCoroutine(ReenableAgent(sec));
        }
        public IEnumerator ReenableAgent(float sec)
        {
            //wait for sec
            agent.enabled = false;
            rb.isKinematic = false;
           
            //REMOVE THIS IF YOU DONT WANT THE AI TO GO LIMP - MW
            //unfreeze agent x and z rotation
            rb.constraints = RigidbodyConstraints.None; 
            shouldUpdate = false;
            //while the agent is not on the ground
            yield return new WaitForSeconds(sec);
            //reenable the agent
            shouldUpdate = true;
            //ALSO REMOVE THIS -MW
            //freeze agent x and z rotation
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            
            rb.isKinematic = true;
            agent.enabled = true;
        }

        private void OnDrawGizmos()
        {
            if(!shouldDebug) return;
                
            //draw the meta data for the AI
            //Sight
            Gizmos.DrawWireSphere(transform.position,viewRadius);
            Gizmos.color = Color.cyan;
            //draw the wander circle
            Gizmos.DrawWireSphere(transform.position + transform.forward * circleDistance, circleRadius);
            //draw the fov
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * viewRadius);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0,fov,0) * transform.forward * viewRadius);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0,-fov,0) * transform.forward * viewRadius);
        }
        
}
