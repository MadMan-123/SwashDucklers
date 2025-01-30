using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Launcher : MonoBehaviour
{
    [SerializeField] private float launchDuration = 1f;
    [SerializeField] private string areaName;
    Vector3 pos;
    [SerializeField] private bool shouldDebug = false;


    public void LaunchObject(GameObject obj)
    {
        var area = AreaManager.GetArea(areaName);
        pos = area.GeneratePositionInArea();

        //clamp the position
        NavMesh.SamplePosition(pos, out var hit, 2f, NavMesh.AllAreas);

        var vel = LaunchManager.instance.LaunchObjectWithVar(obj,pos,launchDuration);
        if(shouldDebug)
            LaunchManager.DrawTrajectory(obj.transform.position,vel,launchDuration);
    }

    private void OnDrawGizmos()
    {
        if(!shouldDebug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.5f); 
    }
}
