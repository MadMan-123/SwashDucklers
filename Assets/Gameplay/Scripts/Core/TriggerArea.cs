using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = System.Numerics.Vector3;

[RequireComponent(typeof(BoxCollider))]
public class TriggerArea : MonoBehaviour
{
    //event to be called when the trigger is entered
    public UnityEvent<GameObject> onTrigger;
    
    //box collider to be used as the trigger
    public BoxCollider box;
    
    //tag to ignore
    public string ignoreTag;
    
    //debugging
    [SerializeField] private bool shouldDebug = false;

    //list of objects that have entered the trigger
    public List<GameObject> tracked = new();
    
    
    private void Awake()
    {
        //get the box collider and set it to be a trigger
        if (box != null) return;
        //get the box collider and set it to be a trigger
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        //if the tag is ignored or the object is already tracked return
        if (((ignoreTag != "" && other.CompareTag(ignoreTag) ) || tracked.Contains(other.gameObject)))
        {
            return;
        }
        //add the object to the list of tracked objects
        tracked.Add(other.gameObject); 
        
        //invoke the event
        onTrigger?.Invoke(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other)
    {
        //if the tag is ignored or the object is already tracked return
        if ((ignoreTag != "" && other.CompareTag(ignoreTag)) || tracked.Contains(other.gameObject))
        {
            //remove the object from the list of tracked objects
            tracked.Remove(other.gameObject);
        }
    }

#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        //draw information about the trigger area
        if(!shouldDebug) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position,transform.lossyScale);
        Handles.Label(transform.position,gameObject.name);
    }
#endif
}
