using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLauncher : Launcher 
{
    public string[] areaNames;
    public void LaunchEnemy(GameObject obj)
    {
        var areaName = areaNames[Random.Range(0,areaNames.Length)];
        var pos = Vector3.zero;
        var area = AreaManager.GetArea(areaName);
        pos = area.GeneratePositionInArea();
        //clamp the position to the navmesh
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        
        var cachedVel = LaunchManager.CalculateVelocity(pos,obj.transform.position,launchDuration,(pos - obj.transform.position).y); 
        ValidateLaunchPosition(cachedVel,obj);
        var vel = LaunchManager.instance.LaunchObjectWithVar(obj,pos,launchDuration);
        //add the player to the list of players
        //remove the player from the list after a certain amount of time
        if(shouldDebug)
            LaunchManager.DrawTrajectory(obj.transform.position,vel,launchDuration);
        
    }
}
