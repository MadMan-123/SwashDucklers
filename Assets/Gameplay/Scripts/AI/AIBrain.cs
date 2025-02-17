using System;
using System.Collections;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AIBrain : MonoBehaviour
{
        public NavMeshAgent agent;
        public Rigidbody rb;
        public Inventory inventory;
        [SerializeField] private float viewRadius = 10;
        [SerializeField] private Target target;
        [SerializeField] private State state;
        [Header("Wander behaviour")]
        [SerializeField] private float circleDistance = 5f;
        [SerializeField] private float randDifference = 90f;
        [SerializeField] private float circleRadius = 5f;
        [Header("Attack behaviour")]
        [SerializeField] private float attackDistance = 1.5f;
        //[SerializeField] private float attackRadius = 0.75f;
        [SerializeField] private float damage = 5;
        [SerializeField] private bool canAttack = true;
        [SerializeField] private float cooldownTime = 3f;
        [SerializeField] public float knockDownTime = 5f;
        [SerializeField] private bool shouldDebug = false;
        
        [SerializeField] private float fleeDistance = 5f;
        private Health health;
        private LayerMask boatLayer;
        private Vector3 delta;
        private Collider[] colliders = new Collider[10];
        
        float[] distances = new float[10];
        private float wanderAngle = 0f; 
        [SerializeField] private bool onFloor;
        [SerializeField] private bool hasCargo = false;
        [SerializeField] private bool isFleeing;
        [SerializeField] private float fleeTime = 5f;

        
        public enum State
        {
            Idle,
            Chase,
            Attack,
            Flee,
            Wander,
            JumpOff,
            Steal
        }

        void Start()
        {
            boatLayer = LayerMask.NameToLayer("Boat");
            state = State.Wander;
            
            if (!TryGetComponent(out agent))
                agent = gameObject.AddComponent<NavMeshAgent>(); 
            
            agent.enabled = false;
            
            if(!TryGetComponent(out rb))
                rb = gameObject.AddComponent<Rigidbody>();
                
            rb.isKinematic = false;
            
            if (!TryGetComponent(out health))
                health = gameObject.AddComponent<Health>();
            
            health.SetHealth(health.GetMaxHealth());
            
            if(!TryGetComponent(out inventory))
                inventory = gameObject.AddComponent<Inventory>();
        }
       
        private void Update()
        {
            if (!agent.enabled)
            {
                //if the agent is not enabled wait to be on the floor
                StartCoroutine(WaitUntilFloorHit());
            }

            if (agent.enabled)
            {
                //check for a target
                CheckForPlayer(); 
                //decision tree
                //is player near 
                if (target != null && target.type == Target.Type.Player)
                {
                    //check if we have cargo
                    if (hasCargo)
                    {
                        //if yes then flee
                        ChangeState(State.Flee);
                    }
                    else if (!isFleeing)
                    {
                        //chase the player
                        ChangeState(State.Chase);
                        //if the player has an item and in range

                        if (delta.magnitude < attackDistance)
                        {
                            //check if the player has an item and steal only if near   
                            if (target.trackedTransform.TryGetComponent(out Inventory inv) && inv.item != null ) 
                            {
                                //check if the player has an item
                                //if yes steal
                                inv.TakeItem(inventory);
                                hasCargo = true;
                            } 
                            ChangeState(State.Attack);
                        }
                        
                    }
                    else
                    {
                        ChangeState(State.Flee);
                    }

                }
                
                //if no then check if we have cargo in hand
                else
                {
                    if (hasCargo)
                    {
                        //if yes then flee
                        ChangeState(State.Flee);
                    }
                    else if(target != null )
                    {
                        
                        //if no then chase
                        ChangeState(State.Chase);
                        
                        

                    }
                                                
                }

                HandleMovement();
            }
        }

        
        private Target GetClosestCargo()
        {
            Collider[] colliders = new Collider[5];
            float[] cache = new float[5];
            //sphere cast to get all the cargo
            Physics.OverlapSphereNonAlloc(transform.position, viewRadius, colliders);
            var cargo = colliders.Where(x => x && x.TryGetComponent(out Cargo cargo)).ToArray();
            var count = cargo.Length;
            if(count == 0) return null;
            //get the distances
            for (int i = 0; i < count; i++)
            {
                cache[i] = (cargo[i].transform.position - transform.position).magnitude;
            }
            
            //sort the cargo by distance
            Helper.QuickSortWithDistances(cargo, cache, 0, count - 1);
            //return the closest cargo
            return new Target(cargo[0].transform, Target.Type.Cargo);
        }

        private Target CheckForPlayer()
        {
            //get the player layer mask
            var playerLayer = LayerMask.NameToLayer("player");
            
            //sphere cast then filter with fov check
            Physics.OverlapSphereNonAlloc(transform.position, viewRadius, colliders, 1 << playerLayer);
            //get all the players by checking the tag and if the value is null
            var players = colliders.Where(x => x).ToArray();
            //get the count
            var count = players.Length;
            Array.Clear(distances, 0, distances.Length);
            //get the distances
            for (int i = 0; i < count; i++)
            {
                //get the distance
                distances[i] = (players[i].transform.position - transform.position).magnitude;
            }
            //sort the players by distance
            Helper.QuickSortWithDistances(players, distances, 0, count - 1);
            //set the target to the first player, else null
            target = players.Length > 0 ? new Target(players[0].transform, Target.Type.Player) : null;
             
            //get the delta for later calculations
            if (target != null)
                delta = transform.position - target.trackedTransform.position;
            
            
            return target;
        }

        private void HandleMovement()
        {
                var destination = state switch
                {
                    State.Idle => transform.position,
                    State.Wander => Wander(),
                    State.Flee => Flee(),
                    State.Attack => Attack(),
                    State.Chase => Chase(),
                    State.Steal => Steal(),
                    State.JumpOff => JumpOff(),
                    _ => transform.position
                };


                //clamp the destination to the navmesh, if the point is not on the navmesh then we should find the closest point
                if (!NavMesh.SamplePosition(destination, out var hit, 0.5f, NavMesh.AllAreas))
                {
                    NavMesh.FindClosestEdge(destination, out hit, NavMesh.AllAreas);
                    destination = hit.position;
                }

                //draw the destination as a big line
                //if the agent is enabled and is placed on a nav mesh area
                if (agent.enabled && NavMesh.SamplePosition(transform.position, out var hit2, 0.5f, NavMesh.AllAreas))
                    agent.SetDestination(destination);
                
                //obstacle avoidance
                //we need to turn around if there is something infront of us or we are looking off the edge of the navmesh
                /*if (Physics.Raycast(transform.position, transform.forward, out var hit3, 1f, NavMesh.AllAreas))
                {
                    //turn around
                    transform.Rotate(Vector3.up, 180);
                }*/
                
        }

        private Vector3 JumpOff()
        {
            throw new NotImplementedException();
        }

        private Vector3 Steal() => throw new NotImplementedException();

        private readonly RaycastHit[] hits = new RaycastHit[2];

        private void FixedUpdate()
        {
            //check if we are on the boat, if we are then we should flag the agent to be enabled
            //sphere cast to check if we are on the boat just below the agent
            if (!onFloor)
            {
                var count = (Physics.SphereCastNonAlloc(
                    transform.position,
                    0.5f,
                    Vector3.down,
                    hits,
                    0.1f,
                    1 << boatLayer
                ));
                
                onFloor = count > 0;
            }
        }

        private IEnumerator WaitUntilFloorHit()
        {
            //wait until the agent is on the floor
            while(onFloor) yield return null;
            
            //check underneath 
            agent.enabled = true;
            rb.isKinematic = true;
        }


        private Vector3 Flee()
        {
            if (target == null)
            {
                ChangeState(State.Wander);
                return transform.position;
            }
            //get a position far away 
            Vector3 directionToFlee = (transform.position - target.trackedTransform.position).normalized;
            Vector3 fleePosition = transform.position + directionToFlee * fleeDistance; 
            
            //clamp the destination to the navmesh, if the point is not on the navmesh then we should find the closest point
            if (!NavMesh.SamplePosition(fleePosition, out var hit, 2, NavMesh.AllAreas))
            {
                NavMesh.FindClosestEdge(fleePosition, out hit, NavMesh.AllAreas);
                fleePosition = hit.position;
            }
            
            return fleePosition;
        }

        private Vector3 Attack()
        {
            if (target == null)
            {
                ChangeState(State.Wander);
                return transform.position;
            }

            if (isFleeing)
            {
                ChangeState(State.Flee);
                return Flee();
            }
            if (!canAttack) return transform.position;
            canAttack = false;
            
            var destination = delta * 0.95f;
            //do attack
            if (target.trackedTransform.TryGetComponent(out Health health))
            {
                health.TakeDamage(gameObject,damage);
              
                //flee
                ChangeState(State.Flee);
                isFleeing = true;
                StartCoroutine(ResetFleeFlag(fleeTime));
            }
        
            
            StartCoroutine(ResetAttackFlag(cooldownTime));
            
            
            return destination;
        }

        private Vector3 Chase()
        {


            return target.trackedTransform.position;
        }


        IEnumerator ResetFleeFlag( float time)
        {
            yield return new WaitForSeconds(time);
            isFleeing = !isFleeing;
            
        }
        
        IEnumerator ResetAttackFlag( float time)
        {
            yield return new WaitForSeconds(time);
            canAttack = !canAttack;
        }
        
        
        private Vector3 Wander()
        {

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
            

        public void ChangeState(State newState)
        {
            state = newState;
            
        }
  


        public void ToggleAttack(GameObject source)
        {
            if(source.CompareTag("Player"))
                target = new Target(source.transform, Target.Type.Player);
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
            
            //while the agent is not on the ground
            yield return new WaitForSeconds(sec);
            //reenable the agent
            
            //ALSO REMOVE THIS -MW
            //freeze agent x and z rotation
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            
            
            //clamp the agent to the navmesh before enabling the agent
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            
            rb.isKinematic = true;
            agent.enabled = true;
            
            
        }

        private void OnDrawGizmos()
        {
            if(!shouldDebug) return;
                
            //draw the metadata for the AI
            //Sight
            Gizmos.DrawWireSphere(transform.position,viewRadius);
            Gizmos.color = Color.cyan;
            //draw the wander circle
            Gizmos.DrawWireSphere(transform.position + transform.forward * circleDistance, circleRadius);
            //draw the fov
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * viewRadius);
        }

        [Serializable]
        public class Target
        {
            public Transform trackedTransform;
            public Type type;
            public enum Type
            {
                NoTarget = -1,
                Cargo,
                Player
            }

            public Target(Transform transform, Type newType)
            {
                trackedTransform = transform;
                type = newType;
            }
        }

        
}
