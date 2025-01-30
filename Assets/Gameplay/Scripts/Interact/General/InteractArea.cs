using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InteractArea : MonoBehaviour
{
    [SerializeField] public bool needTool;
    
    [SerializeField] bool fasterWithTool;
    [SerializeField] public bool isTool;
    [SerializeField] public bool isStation;


    [SerializeField] public ItemManager.Type expectedType;
    [SerializeField] UnityEvent<GameObject,float> OnInteract;
    [SerializeField] private InteractComponent currentInteractable;
    [SerializeField] private PlayerControler currentController;
    
    [SerializeField] public string TaskName;
    [SerializeField] private float toolTime = 2f;
    [SerializeField] private float regularTime = 4f;


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
            ResetAll();
        }
    }

    public void ResetAll()
    {

            currentInteractable.inArea = false;               
            currentInteractable.AreaImIn = null;
            currentInteractable = null;
            currentController = null;
        
    }
    public bool CheckTask()
    {
        TaskManager.instance.CompleteTask(TaskName);
        var task = TaskManager.TaskHashMap[TaskName];
        
            
        //if the task is completed, and we are a dynamic object then return the object to the pool
        if (task.isDynamic && task.isCompleted)
        {
            task.isCompleted = false;
            TaskManager.instance.ReturnTask(gameObject);
            ResetAll();
        }

        return true;
    }
    public void Interact(GameObject player)
    {
        if(TaskName !=  "")
            if(!CheckTask()) return; 
        OnInteract?.Invoke(player,regularTime);
    }

    public void InteractWithTool(ItemManager.Type tool, GameObject player)
    {
        if(TaskName != "")
            if(!CheckTask() || tool != expectedType ) return;
        var time = expectedType == tool && fasterWithTool ? regularTime : toolTime;
        if (player.TryGetComponent(out Inventory inv))
        {
            switch (tool)
            {
                case ItemManager.Type.CannonBall:

                    inv.Return();

                    break;
                case ItemManager.Type.Plank:

                    inv.RemoveItem();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tool), tool, null);
            }

        }
        
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(TaskName != "")
            Handles.Label(transform.position + transform.up * 1.5f,$"Task: {TaskName}"); 
    }
#endif
}
