using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using Cinemachine;
using Core;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
        //some buffer of tasks we can perform
        [SerializeField] private List<TaskDescriptor> taskList = new();
        
        //counter of tasks completed
        public int tasksCompleted = 0;
        

        //hash map to all the tasks
        public static Dictionary<string,Task> TaskHashMap = new();
       
        //dynamic pools
        readonly List<GameObjectPool> dynamicPools = new();
        
        //track all ids
        private readonly List<int> dynamicIds = new List<int>();
        //Singleton
        public static TaskManager instance;
        
        //used to spawn dynamic tasks
        private IEnumerator SpawnDynamicTasks;
        [SerializeField] private bool shouldDebug = false;
        [SerializeField] private float playerCheckRadius = 4.5f;
        
        LayerMask playerMask;

        [SerializeField] private CinemachineTargetGroup cinemachineTarget;


        private bool ranOnce = false;
        private void Awake()
        {
                //singleton boilerplate
                if (!instance)
                {
                        instance = this;
                }
                else
                {
                        Destroy(gameObject);
                }
                
                playerMask = LayerMask.GetMask("player");
        }

        private void Start()
        {
                if(ranOnce) return;
            TaskHashMap = new();

                //for each task in the task list
                for (var i = 0; i < taskList.Count; i++)
                {
                        //reference to the task
                        ref var task = ref taskList[i].task;    
                        //add to the hash map
                        TaskHashMap.Add(taskList[i].taskName, task);
                        //log out progress
                        Debug.Log($"{taskList[i].taskName} : is added to the task hash map, Capacity: {TaskHashMap.Count}");
                        //set up object pools
                        if((taskList[i].task.isDynamic || !taskList[i].task.isStatic) || taskList[i].dynamicObjectPrefab)
                                dynamicIds.Add(i);
                        foreach (var interactable in taskList[i].Interactables) { if (interactable is Leak leak) { leak.target = cinemachineTarget; } }
                }
                
                //get all the dynamic tasks
                for (int i = 0; i < dynamicIds.Count; i++)
                {
                        //get the task descriptor
                        var taskDesc = taskList[dynamicIds[i]];
                        if (taskDesc.task.isDynamic && taskDesc.task.isStatic)
                        {
                                dynamicPools.Add(new GameObjectPool(taskDesc.Interactables.Count,transform));
                                
                                //turn the dynamicStaticIAreas into a list of game objects
                                List<GameObject> gameObjects = taskDesc.Interactables.Select(item => item.gameObject).ToList();
                                //set buffer
                                dynamicPools[^1].SetBuffer(gameObjects);
                                //set the object pools to the dynamic static areas 
                                for(int j = 0; j < taskDesc.Interactables.Count; j++)
                                {
                                        var poolObj = dynamicPools[^1][j];

                                        /*if(poolObj.TryGetComponent(out Interactable iArea))
                                                iArea.gameObject.SetActive(false);*/
                                }

                        }
                        else if (taskDesc.task.isDynamic)
                        {
                                //make object pool
                                dynamicPools.Add(new GameObjectPool(taskDesc.dynamicObjectPrefab, 5, transform));
                        }
                }
                //setup the coroutine
                SpawnDynamicTasks = _SpawnDynamicTasks();
                ranOnce = true;
                StartCoroutine(SpawnDynamicTasks);
                
        }

        private IEnumerator _SpawnDynamicTasks()
        {
                float timeToTake = 0f;
                
                //todo: this might change, right now its just randomly spawning all dynamic tasks
                for (int i = 0; i < dynamicPools.Count; i++)
                {
                        //ref to pool
                        var pool = dynamicPools[i];
                        
                        //current object
                        GameObject obj = null;
                        var task = TaskHashMap[taskList[dynamicIds[i]].taskName];
                        
                        
                        
                        //if the task is static and dynamic choose a random one to be active
                        if (task.isStatic && task.isDynamic)
                        {
                                //get a random index 
                                //Lol forgot count returns the amount of elements from 1 not 0, that error has popped up for a while sos i didnt see this earlier :/ - MW

                                var players = GameData.Players;
                                
                                //get avg position of players
                                Vector3 avgPos = Vector3.zero;
                                for (int j = 0; j < players.Count; j++)
                                {
                                        avgPos += players[j].transform.position;
                                }
                                avgPos /= players.Count;
                                
                                //draw a line to the avg position
                                
                                //calculate the distances between the task pool and avg position, then sort them based on distance
                                float[] distances = new float[pool.Count];
                                for (int j = 0; j < pool.Count; j++)
                                {
                                        distances[j] = (avgPos - pool[j].transform.position).magnitude;
                                      
                                }

                                var tasks = new GameObject[pool.Count];
                                tasks = pool.GetAllObjects().ToArray();
                                
                                //print before then after

                                
                                //sort the distances
                                Helper.QuickSortWithDistances(tasks, distances,0, distances.Length - 1);
        
                                //get the task that is the furthest away from the average position by
                                //checking all elements and finding the first one that is not active
                                int last = 0;
                                for (int j = tasks.Length - 1; j >= 0; j--)
                                {
                                        if (tasks[j].activeSelf) continue;
                                        last = j;
                                        break;
                                }
                                
                                //enable the interact area
                                obj = tasks[last];
                                if(obj.activeSelf) continue;
                                //enable the object
                                if (obj.TryGetComponent(out Interactable iArea))
                                {

                                        iArea.gameObject.SetActive(true);
                                }
                                timeToTake = task.taskSecondInterval;
                                break;
                        }
                        //if its just dynamic then set it to a new position in its area
                        else if (task.isDynamic)
                        {
                                obj = pool.GetObject();
                                //set the position
                                obj.transform.position = AreaManager.GetArea(task.areaName).GeneratePositionInArea();
                                
                                timeToTake = task.taskSecondInterval;
                        }
                }
                //wait for X seconds
                yield return new WaitForSeconds(timeToTake);
                //recursively call this coroutine
                SpawnDynamicTasks = _SpawnDynamicTasks();
                StartCoroutine(SpawnDynamicTasks);
        }
        public void CompleteTask(string taskName)
        {
                
                //Get the current task
                var task = TaskHashMap[taskName];
                //null check
                if (task == null)
                {
                        //log warning if null
                        Debug.LogWarning($"{taskName} is not found");
                        return;
                }
                
                //if the task is dynamic then dont set isComplete to true, just add to counter


                        task.isCompleted = true;

                //add to the counter
                tasksCompleted++;
                //log that a task has been completed
                Debug.Log($"{taskName} completed");
        }
        //this is used for dynamic tasks
        public bool ReturnTask(GameObject objectToReturn)
        {
                //for each pool
                for (int i = 0; i < dynamicPools.Count; i++)
                {
                        //get the pool
                        var pool = dynamicPools[i];
                        //does the pool contain the object?
                        if (pool.Contains(objectToReturn))
                        {
                                //if so return the object 
                                pool.ReturnObject(objectToReturn);
                                //Return success 
                                return true;
                        }
                }
                //if not return faliure
                return false;
        }


        private void OnDrawGizmos()
        {
                if(!shouldDebug) return;
                //draw each dynamic static tasks
                for (var i = 0; i < taskList.Count; i++)
                {
                        var task = taskList[i];
                        for (var k = 0; k < task.Interactables.Count; k++)
                        {
                                var area = task.Interactables[k];
                                //draw
                                Gizmos.color = Color.red;
                                //add slight variation 
                                Gizmos.DrawWireSphere(area.transform.position, 0.5f);
                        }
                }
        }

        private void OnDestroy()
        {
                StopCoroutine(SpawnDynamicTasks);
                //remove the instance
                instance = null;
        }
        

        //Used to define a task in the editor
        [Serializable]
        public class TaskDescriptor
        {
                public string taskName;
                public Task task;
                public GameObject dynamicObjectPrefab;
                public List<Interactable> Interactables = new();
        }
        
        //the actual meat and bones (well kinda more the bones) of the Task
        [Serializable]
        public class Task 
        {
                private string taskName;
                public float taskSecondInterval = 5f; //5 seconds
                public bool isStatic;
                public bool isCompleted;
                public bool isDynamic;
                //public int currentTaskId = -1;
                public string areaName;
                public int target = 1;
                public void SetName(string name) => taskName = name;       
                public string GetName() => taskName;
        }
        


        
}
