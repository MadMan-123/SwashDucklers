using System;
using System.Collections;
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
        private Transform target;
        [SerializeField] private State state;
        private bool shouldUpdate = true;
        Vector3 delta;
        [SerializeField] private float circleDistance = 5f;
        [SerializeField] private float randDifference = 90f;
        [SerializeField] private float circleRadius = 5f;
        [SerializeField] private Collider[] colliders = new Collider[10];
        [SerializeField] public float knockDownTime = 5f;
        [SerializeField] private bool shouldDebug = false;
        [SerializeField] private float attackDistance = 1.5f;
        [SerializeField] private float attackRadius = 0.75f;
        [SerializeField] private float damage = 5;
        [SerializeField] private bool canAttack = true;
        [SerializeField] private float cooldownTime = 3f;
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
            boatLayer = LayerMask.NameToLayer("Boat");
            state = State.Idle;
            if (TryGetComponent(out agent))
            {
                agent.enabled = false;
            }
            
            if(TryGetComponent(out rb))
            {
                rb.isKinematic = false;
            }

            if (TryGetComponent(out health))
            {
                health.SetHealth(health.GetMaxHealth());
            }
        }
        
        private void Update()
        {
            if (!agent.enabled)
            {
                //if the agent is not enabled wait to be on the floor
                StartCoroutine(WaitUntilFloorHit());
            }
            
            if (shouldUpdate && agent.enabled)
            {
                //check for a target
                //sphere cast then filter with fov check
                var count = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, colliders,LayerMask.NameToLayer("player"));
                colliders = colliders.Where(collider => collider.CompareTag("Player")).ToArray();
 
                count = colliders.Length;
                if(count == 0)
                {
                    //if there are no colliders in the view radius, return to wander
                    ChangeState(State.Wander);
                    
                }
                
                for (int i = 0; i < count; i++)
                {
                    //check if the collider is in the fov
                    if (CanSee(colliders[i].transform))
                    {
                        //todo: some sort of dynamic priority system on what the target should be
                        target = colliders[i].transform;
                        
                        var distance = (target.position - transform.position).magnitude;
                        //Judge what state to be in

                        //if the health is low, flee
                        if(health.GetHealth() >= health.GetMaxHealth() / 2)
                            ChangeState(State.Flee);
                        
                        //if we are in attack range, attack
                        if(distance < attackDistance)
                            ChangeState(State.Attack);
                        
                        //if we are not chase
                        if (distance < viewRadius) 
                            ChangeState(State.Chase);
                            

                        //if we cant see the target any more, wander
                        

                    }
                }

                Vector3 destination;
                
                if (target)
                {
                    delta = transform.position - target.position;

                    switch (state)
                    {
                        case State.Idle:
                            destination = Idle();
                            break;
                        case State.Chase:
                            destination = Chase();
                            break;
                        case State.Attack:
                            destination = Attack();
                            break;
                        case State.Flee:
                            destination = Flee();
                            break;
                        case State.Wander:
                            destination = Wander();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    //clamp the destination to the navmesh
                    if (NavMesh.SamplePosition(destination, out var hit, 2f, NavMesh.AllAreas))
                    {
                        destination = hit.position;
                    }
                    else
                    {
                        destination = transform.position;
                    }
                    
                    agent.SetDestination(destination);
                }

               
            }
        }

        private IEnumerator WaitUntilFloorHit()
        {
            yield return new WaitForSeconds(5);
            agent.enabled = true;
            rb.isKinematic = true;
        }


        private Vector3 Flee()
        {
            //find a point that is opposite to the target
            return transform.position - delta;
    
        }

        private Vector3 Attack()
        {
            if (!canAttack) return transform.position;
            ResetFlag(); 
            
            var destination = delta * 0.85f;
            //do attack
            Collider[] colliders = new Collider[10];
            var count = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * 0.5f,attackRadius, colliders);
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject != gameObject && colliders[i].TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
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
            return target.position;
        }

        private void ResetFlag()
        {
            canAttack = !canAttack;
        }
        private Vector3 Wander()
        {
            //Circle pos
            var circlePos = transform.position + (agent.velocity * circleDistance);
	
            //Generate a small random angle
            float randomAngle = Random.Range(-randDifference, randDifference) * Mathf.Deg2Rad;

            //Calculate the random offset
            float offsetX = Mathf.Cos(randomAngle) * circleRadius;
            float offsetY = Mathf.Sin(randomAngle) * circleRadius;

            //Apply the random offset
            var velocityPos = (transform.position + agent.velocity);
            velocityPos.x += offsetX;
            velocityPos.y += offsetY;

            //Constrain the point to the circle
            var constrainedPos = ConstraintPointToCircle(velocityPos, circlePos, circleRadius);
            
            //calculate the desired velocity by taking the constrained position and current position, getting the unit vector then multiplying by max current speed
            return constrainedPos;
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
            sum.y = circlePosition.y + radius * Mathf.Sin(theta);

            return sum;

        } 
        bool CanSee(Transform target)
        {
            //check if target is within field of view
            var angle = Vector3.Angle(transform.forward, target.position - transform.position);
            if (angle < fov)
            {
                //check if target is within line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.position - transform.position, out hit))
                {
                    return (hit.transform == target);
                }
            }

            return false;
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
            
            //while the agent is not on the ground
            yield return new WaitForSeconds(sec);
            //reenable the agent
            
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
