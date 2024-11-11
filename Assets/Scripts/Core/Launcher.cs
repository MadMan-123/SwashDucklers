using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private float launchDuration = 1f;
    [SerializeField] private string areaName;
    Vector3 pos;


    public void LaunchObject(GameObject obj)
    {
        var area = AreaManager.GetArea(areaName);
        pos = area.GeneratePositionInArea();
        var vel = LaunchManager.instance.LaunchObjectWithVar(obj,pos,launchDuration);
        LaunchManager.DrawTrajectory(obj.transform.position,vel,launchDuration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.5f); 
    }
}
