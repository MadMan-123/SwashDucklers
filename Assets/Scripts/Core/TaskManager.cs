using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Core;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
        //some buffer of tasks we can perform
        [SerializeField] private List<TaskDescriptor> taskList = new();
        
        //counter of tasks completed
        public int tasksCompleted = 0;
        
        //how many seconds between tasks spawning
        [SerializeField] private float taskSecondInterval = 5f; //5 seconds
        //hash map to all the tasks
        public static Dictionary<string,Task> TaskHashMap = new();
       
        //dynamic pools
        List<GameObjectPool> dynamicPools = new();
        
        //track all ids
        private readonly List<int> dynamicIds = new List<int>();
        //Singleton
        public static TaskManager instance;
        
        //used to spawn dynamic tasks
        private IEnumerator SpawnDynamicTasks;
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
                        
        }

        private void Start()
        {
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
                }
                
                //get all the dynamic tasks
                for (int i = 0; i < dynamicIds.Count; i++)
                {
                        //get the task descriptor
                        var taskDesc = taskList[dynamicIds[i]];
                        if (taskDesc.task.isDynamic && taskDesc.task.isStatic)
                        {
                                dynamicPools.Add(new GameObjectPool(taskDesc.dynamicStaticIAreas.Count,transform));
                                
                                //turn the dynamicStaticIAreas into a list of game objects
                                List<GameObject> gameObjects = taskDesc.dynamicStaticIAreas.Select(item => item.gameObject).ToList();
                                //set buffer
                                dynamicPools[^1].SetBuffer(gameObjects);
                                //set the object pools to the dynamic static areas 
                                for(int j = 0; j < taskDesc.dynamicStaticIAreas.Count; j++)
                                {
                                        var poolObj = dynamicPools[^1][j];

                                        if(poolObj.TryGetComponent(out InteractArea iArea))
                                                iArea.gameObject.SetActive(false);
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
                                var index = UnityEngine.Random.Range(0, pool.Count);
                                //enable the interact area
                                obj = pool[index];
                                //enable the object
                                if (obj.TryGetComponent(out InteractArea iArea))
                                {
                                        iArea.gameObject.SetActive(true);
                                }
                        }
                        //if its just dynamic then set it to a new position in its area
                        else if (task.isDynamic)
                        {
                                obj = pool.GetObject();
                                //set the position
                                obj.transform.position = AreaManager.GetArea(task.areaName).GeneratePositionInArea();
                        }
                }
                //wait for X seconds
                yield return new WaitForSeconds(taskSecondInterval - timeToTake);
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
        
        
        //Used to define a task in the editor
        [Serializable]
        public class TaskDescriptor
        {
                public string taskName;
                public Task task;
                public GameObject dynamicObjectPrefab;
                public List<InteractArea> dynamicStaticIAreas = new();
        }
        
        //the actual meat and bones (well kinda more the bones) of the Task
        [Serializable]
        public class Task 
        {
                private string taskName;
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
