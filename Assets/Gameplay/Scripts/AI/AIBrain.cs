using System;
using System.Collections;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIBrain : MonoBehaviour
{
        public NavMeshAgent agent;
        public Rigidbody rb;
        public Inventory inventory;
        public EnemySpawner owner; 
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
        [Header("Visual behaviour")] 
        private int walkingID;
        [SerializeField] private Animator anim;
        [SerializeField] private float fleeDistance = 10f;
        private Health health;
        private LayerMask boatLayer;
        private Vector3 delta;
        
        
        float[] distances = new float[10];
        private float wanderAngle = 0f; 
        [SerializeField] private bool onFloor;
        [SerializeField] private bool hasCargo = false;
        [SerializeField] private bool isFleeing;
        [SerializeField] private float fleeTime = 5f;

        private readonly RaycastHit[] hits = new RaycastHit[2];
        [SerializeField] private bool reenableFlag;
        
        [SerializeField] private Collider[] colliders = new Collider[10];
        public enum State
        {
            Idle,
            Chase,
            Attack,
            Flee,
            Wander,
            JumpOff
        }

        void Start()
        {
            shouldDebug = true;
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

            if (!TryGetComponent(out inventory))
                inventory = gameObject.AddComponent<Inventory>();
            walkingID = Animator.StringToHash("IsWalking");
        }
       
        private void Update()
        {
            if (!agent.enabled || !enabled)
            {
                //if the agent is not enabled wait to be on the floor
                StartCoroutine(WaitUntilFloorHit());
            }

            
            //for the reccord, i hate this, i absolutely hate this,
            //there is nothing more i hate than thousands of if statements but we cant return in update because that breaks the AI - MW
            if (agent.enabled)
            {
                //check for a target
                CheckForTargets();
                //decision tree
                //is player near

                
                    if (target != null)
                    {

                        if (target.type == Target.Type.Player)
                        {
                            //check if we have cargo
                            if (hasCargo)
                            {
                                //if yes then flee
                                ChangeState(State.JumpOff);
                            }
                            else if (!isFleeing)
                            {
                                //chase the player
                                ChangeState(State.Chase);
                                //if the player has an item and in range

                                if (delta.magnitude < attackDistance)
                                {
                                    //check if the player has an item and steal only if near   
                                    if (target.trackedTransform.TryGetComponent(out Inventory inv) && inv.item != null)
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
                                ChangeState(State.JumpOff);
                            }
                            else if (target != null && target.trackedTransform)
                            {
                                //if no then chase
                                ChangeState(State.Chase);
                                //if less than attack distance then steal
                                if (delta.magnitude < attackDistance)
                                {
                                    //check if the player has an item
                                    if (target.trackedTransform.TryGetComponent(out CargoStack stack))
                                    {
                                        stack.TryPickUp(gameObject);
                                        hasCargo = true;
                                        //we should run off now
                                        ChangeState(State.JumpOff);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        //if no target then wander
                        ChangeState(State.Wander);
                    }

                    HandleMovement();
            }
            

        }
            
        //return the closest cargo

        private Target CheckForTargets()
        {
            //check if we have a target, if so can we see it
            if(target != null && target.trackedTransform) 
            {
                //check if the target is in the view radius
                if ((target.trackedTransform.position - transform.position).magnitude > viewRadius)
                {
                    //if not then set the target to null
                    target = null;
                    ChangeState(State.Wander);
                }
            }
            //get the player layer mask
            var playerLayer = LayerMask.NameToLayer("player");
            var cargoLayer = LayerMask.NameToLayer("Cargo");
            
            var layerMask = 1 << playerLayer | 1 << cargoLayer;
           
            //clear the colliders
            Array.Clear(colliders, 0, colliders.Length);
            //sphere cast then filter with fov check
            Physics.OverlapSphereNonAlloc(transform.position, viewRadius, colliders, layerMask);
            //get all the players by checking the tag and if the value is null
            var targets = colliders.Where(x => x).ToArray();
            //get the count
            var count = targets.Length;
            if(count == 0) return null;
            Array.Clear(distances, 0, distances.Length);
            //get the distances
            for (int i = 0; i < count; i++)
            {
                //get the distance
                distances[i] = (targets[i].transform.position - transform.position).magnitude;
            }
            //sort the players by distance
            Helper.QuickSortWithDistances(targets, distances, 0, count - 1);
            //set the target to the first player, else null
            int currentLayer;
            if (count > 0)
            {
                currentLayer = targets[0].gameObject.layer;
                target = new Target(targets[0].transform, 
                    currentLayer == playerLayer ? Target.Type.Player : currentLayer == cargoLayer ? Target.Type.Cargo : Target.Type.NoTarget);
            }
            else
            {
                
                target = null;
                return null;
            }

            currentLayer = target.trackedTransform.gameObject.layer;
            
            var targetType =  currentLayer == playerLayer ? Target.Type.Player : currentLayer == cargoLayer? Target.Type.Cargo : Target.Type.NoTarget;
            target.type = targetType;
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
                    State.JumpOff => JumpOff(),  
                    _ => transform.position
                };

                //TODO: implement jump off and steal

                    
                    //clamp the destination to the navmesh, if the point is not on the navmesh then we should find the closest point
                if (!NavMesh.SamplePosition(destination, out var hit, 0.5f, NavMesh.AllAreas))
                {
                    NavMesh.FindClosestEdge(destination, out hit, NavMesh.AllAreas);
                    destination = hit.position;
                }
  
               
                
                
                //draw the destination as a big line
                //if the agent is enabled and is placed on a nav mesh area
                if (agent.enabled && NavMesh.SamplePosition(transform.position, out var hit2, 0.5f, NavMesh.AllAreas))
                {
                    //obstacle avoidance
                    //check in front of the agent for physical objects
                    var count = Physics.RaycastNonAlloc(transform.position, transform.forward, hits, 1f);
                    //get the valid hits
                    var valid = hits.Where(h => h.collider).ToList();
                    
                    //if there is a hit then we should move the agent to the side
                    if (count > 0 && valid.Count > 0)
                    {
                        //get the hit
                        var hitInfo = valid[0];
                        //get the normal
                        var normal = hitInfo.normal;
                        //get the direction
                        var direction = Vector3.Cross(normal, Vector3.up);
                        //get the destination
                        destination = transform.position + direction;
                    }
                    agent.SetDestination(destination);
                }
              
                

        }

        private Vector3 JumpOff()
        {
            //get the edge of the navmesh on the z axis 
            NavMeshHit upHit = new();
            //jump into the enemy death area
            var pos1 = transform.position + new Vector3(0, 0, -10);
            
            //try sample the position
            NavMesh.SamplePosition(pos1, out upHit, 10, NavMesh.AllAreas);
            
            //draw the two points
            if (shouldDebug)
            {
                Debug.DrawLine(pos1, upHit.position, Color.green);
            }
            
            //get the distance to the two points
            var upDistance = (upHit.position - transform.position).magnitude;
            
            //get the closest point
            var fleePosition = upHit.position;
           
            //check if the distance is less than 1, if so we will make the launcher lanch the AI to the enemy death area
            if ((fleePosition != upHit.position || !(upDistance < 0.5f))) return fleePosition;
            // enable rigidbody and disable agent
            rb.isKinematic = false;
            agent.enabled = false;
           
            var camTransform = Camera.main.transform;
            //ensure the crab's y axis is facing the camera 
            //smoothly rotate the crab to face the camera
            transform.rotation = Quaternion.LookRotation(-Vector3.forward);
 
            
            var direction = transform.forward;
            direction.y = 2;
            //launch the AI
            rb.AddForce(direction * 4.25f , ForceMode.VelocityChange);
            
            //reset the ai state
            ChangeState(State.Wander);
            //drop the cargo
            hasCargo = false;
            inventory.DropItem(direction, true);
            
            
            //disable the AI
            enabled = false;

            return fleePosition;
        }



        private void FixedUpdate()
        {
            //check if we are on the boat, if we are then we should flag the agent to be enabled
            //sphere cast to check if we are on the boat just below the agent
            if (!onFloor )
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
            if(state == State.JumpOff) yield break;
             Physics.SphereCastNonAlloc(
                                transform.position,
                                0.2f,
                                Vector3.down,
                                hits,
                                0.1f,
                                1 << boatLayer
            );
                
            var objects = hits.Where(x => x.collider).ToArray();
            
            
            //wait until the agent is on the floor
            while(objects.Length == 0 || reenableFlag) yield return null;
            
            //check underneath 
            agent.enabled = true;
            enabled = true;
            rb.isKinematic = true;
        }


        private Vector3 Flee()
        {
            if (target == null)
            {
                ChangeState(State.Wander);
                return transform.position;
            }
            //get an edge of the navmesh
            var fleePosition = transform.position + delta.normalized * fleeDistance;
           
            //get the two furthest points from the target (as we are on a boat there is the back and front we can go to)
            //the axis is the x axis
            
            var targetPos = target.trackedTransform.position;
            var targetX = targetPos.x;
            
            const float difference = 15;
            //get the two points

            var pos = transform.position;
            
            var back = new Vector3(targetX - difference, pos.y, pos.z);
            var front = new Vector3(targetX + difference, pos.y ,pos.z);
            
            //try and sample the position of the two points
            NavMeshHit backHit;
            NavMeshHit frontHit;
            
            const float checkDistance = 10f;
            NavMesh.SamplePosition(back, out backHit, checkDistance, NavMesh.AllAreas);
            NavMesh.SamplePosition(front, out frontHit, checkDistance, NavMesh.AllAreas);
           
            //draw the two points
            if (shouldDebug)
            {
                Debug.DrawLine(back, backHit.position, Color.green);
                Debug.DrawLine(front, frontHit.position, Color.green);
            }
                
            
            //get the distance to the two points
            var backDistance = (backHit.position - target.trackedTransform.position).magnitude;
            var frontDistance = (frontHit.position - target.trackedTransform.position).magnitude;
            
            //get the furthest point
            fleePosition = backDistance > frontDistance ? backHit.position : frontHit.position;
            
            
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

                //Rumble logic -SD
                if (target.type == Target.Type.Player)
                {
                    //not sure if this is the best place to put this but eh
                    PlayerControler playerControler = target.trackedTransform.gameObject.GetComponent<PlayerControler>(); 
                    playerControler.Rumble(playerControler.itemStolenRumble);
                }

                //if the player has a rigidbody then we should apply force based on health
                if (target.trackedTransform.TryGetComponent(out Rigidbody rb))
                {
                    //calculate the velocity needed
                    var healthPercent = health.GetHealth() / health.GetMaxHealth();
                    
                    //get the direction
                    var direction = (transform.position - target.trackedTransform.position).normalized;
                    //get the force
                    var force = direction * (damage + healthPercent * damage);
                    //apply the force
                    
                    rb.AddForce(force,ForceMode.VelocityChange);
                    anim.SetBool(walkingID, true);
                }
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
            if(target == null || target.trackedTransform == null)
            {
                ChangeState(State.Wander);
                return transform.position;
            }
        
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

            reenableFlag = true;
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
            reenableFlag = false;
            
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

        /*public void Death()
        {
            //disable the object
            owner.spawnedCount--; 
            gameObject.SetActive(false);
        }*/
        
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
