using System;
using System.Collections;
using System.Collections.Generic;
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
        List<ObjectPool> dynamicPools = new();
        
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
                //track all ids
                var taskIds = new List<int>();
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
                        if(taskList[i].dynamicObjectPrefab)
                                taskIds.Add(i);
                }
                
                //get all the dynamic tasks
                for (int i = 0; i < taskIds.Count; i++)
                {
                        //get the task descriptor
                        var taskDesc = taskList[taskIds[i]];
                        //make object pool
                        dynamicPools.Add(new ObjectPool(taskDesc.dynamicObjectPrefab,5,transform));
                        
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
                        var obj = pool.GetObject();
                        //set the position
                        //todo: i can forsee an issue in the way im getting the task
                        var task = TaskHashMap[taskList[i].taskName]; 
                        obj.transform.position = AreaManager.GetArea(task.areaName).GeneratePositionInArea();

                        if (!task.isStatic)
                        {
                                //todo: change this later but for now every 25 tasks its getting faster
                                timeToTake= task.dynamicCounter / 25f;
                        }
                }
                //wait for X
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
                if (!task.isStatic)
                {
                        task.dynamicCounter++;
						
						if (!(task.dynamicCounter < task.target))
						{
							 task.isCompleted = true;
						}
                }
                else
                {
                        //complete the task
                        task.isCompleted = true;
                }
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
        }
        
        //the actual meat and bones (well kinda more the bones) of the Task
        [Serializable]
        public class Task 
        {
                private string taskName;
                public bool isStatic;
                public bool isCompleted;
                //public int currentTaskId = -1;
                public string areaName;
                public int dynamicCounter = 0;
                public int target = 10;
                public void SetName(string name) => taskName = name;       
                public string GetName() => taskName;
        }

        
}