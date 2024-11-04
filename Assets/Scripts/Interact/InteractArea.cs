using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InteractArea : MonoBehaviour
{
    [SerializeField] public bool needTool;
    
    [SerializeField] bool fasterWithTool;
    [SerializeField] public bool isTool;
    [SerializeField] public bool isStation;


    [SerializeField] public string toolUsed;
    [SerializeField] UnityEvent<GameObject,float> OnInteract;
    [SerializeField] private InteractComponent currentInteractable;
    [SerializeField] private PlayerControler currentController;
    
    [SerializeField] public string TaskName;
    [SerializeField] private int toolTime = 2;
    [SerializeField] private int regularTime = 4;


    private void Start()
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && 
            col.TryGetComponent(out currentInteractable) && 
            col.TryGetComponent(out currentController))
        {
            currentInteractable.inArea = true;
            currentInteractable.AreaImIn = this;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            currentInteractable.inArea = false;
            currentInteractable.AreaImIn = null;
            currentInteractable = null;
            currentController = null;
        }
    }

    public void CheckTask()
    {
        if(TaskName != "")
        {
			
			if (task.dynamicCounter <= task.target)
			{
                TaskManager.instance.CompleteTask(TaskName);
			}

            //if the task is completed, and we are a dynamic object then return the object to the pool
            if (!TaskManager.TaskHashMap[TaskName].isStatic)
            {
                TaskManager.TaskHashMap[TaskName].isCompleted = false;
            }
            
            TaskManager.instance.ReturnTask(gameObject);
        }
    }
    public void Interact(GameObject player)
    {
        CheckTask(); 
        OnInteract?.Invoke(player,regularTime);
    }

    public void InteractWithTool(string tool, GameObject player)
    {
        CheckTask();
        var time = toolUsed == tool && fasterWithTool ? regularTime : toolTime;
        //invoke the event and pass the source object as the player
        OnInteract?.Invoke(player, time);
    }

    public void DisableAndReEnable(GameObject source, float time)
    {
        var routine = _DisableAndReEnable(source, time);
        StartCoroutine(routine);
    }
    
    private IEnumerator _DisableAndReEnable(GameObject player, float sec)
    {
        if(!currentController) yield break;
        currentController.DisableMovement();
        currentController.rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(sec);
        currentController.EnableMovement();
    }

    public void InteractCancel()
    {
        StopAllCoroutines();
    }
}
