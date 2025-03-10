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

        //clamp the position to the navmesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            pos = hit.position;
            SoundManager.PlayAudioClip("Splash", this.transform.position, 2f);
        }
        

        //validate the launch position
        
        var cachedVel = LaunchManager.CalculateVelocity(pos,obj.transform.position,launchDuration,(pos - obj.transform.position).y); 
        
        ValidateLaunchPosition(cachedVel,obj);
        var vel = LaunchManager.instance.LaunchObjectWithVar(obj,pos,launchDuration);
        if(shouldDebug)
            LaunchManager.DrawTrajectory(obj.transform.position,vel,launchDuration);
    }


    private void ValidateLaunchPosition(Vector3 vel,GameObject obj)
    {
        //take the position of the player and the position of the boat
        //get a difference in the z axis and based if positive or negative adjust the player position along the z axis in the respective direction
        //only if the path is not valid
        
        var playerPos = obj.transform.position;
        var diff = playerPos.z - 5;
        
        //figure out if positive or negative
        var sign = diff > 0 ? 1 : -1;
        
        
        //try to adjust the player position until a valid path is found
        while (!IsValidPath(obj,playerPos - obj.transform.position))
        {
            playerPos.z += sign > 0 ? -1 : 1;
            playerPos.z *= 2.5f;
        }
        
        
        
        
    }
        
        public bool IsValidPath(GameObject obj,Vector3 velocity)
        {
            //raycast to see if there is a clear path
            RaycastHit hit;
            return !Physics.Raycast(obj.transform.position, velocity, out hit, velocity.magnitude);
        }
    
    private void OnDrawGizmos()
    {
        if(!shouldDebug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.5f); 
    }
    
    
    
}
