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
    public UnityEvent<GameObject> onTrigger;
    public BoxCollider box;
    public string ignoreTag;
    [SerializeField] private bool shouldDebug = false;

    private void Awake()
    {
        if (box != null) return;
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (ignoreTag != "" && other.CompareTag(ignoreTag))
        {
            return;
        }
        onTrigger?.Invoke(other.gameObject);
    }

#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if(!shouldDebug) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position,transform.lossyScale);
        Handles.Label(transform.position,gameObject.name);
    }
#endif
}
