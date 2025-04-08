using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Launcher : MonoBehaviour
{
    [SerializeField] public float launchDuration = 1f;
    [SerializeField] private string areaName;
    Vector3 pos;
    [SerializeField] public bool shouldDebug = false;

    public List<GameObject> players = new ();

    public void LaunchObject(GameObject obj)
    {
        if(players.Contains(obj)) return;
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
        //add the player to the list of players
        players.Add(obj);
        
        //remove the player from the list after a certain amount of time
        StartCoroutine(RemovePlayer(obj,launchDuration));
       
        if(shouldDebug)
            LaunchManager.DrawTrajectory(obj.transform.position,vel,launchDuration);
    }

    private IEnumerator RemovePlayer(GameObject o, float f)
    {
        yield return new WaitForSeconds(f);
        players.Remove(o);
    }


    protected void ValidateLaunchPosition(Vector3 vel,GameObject obj)
    {
        //move the player to underneath the camera
        
        var camera = Camera.main;
        if (camera == null) return;
        
        var camPos = camera.transform.position;
        
        //get the camera position and make the y axis the same as the player
        var position = new Vector3(obj.transform.position.x, obj.transform.position.y, camPos.z);
        
        //set the object to the new position
        obj.transform.position = position;
        
        //get the new velocity
        vel = LaunchManager.CalculateVelocity(pos,position,launchDuration,(pos - position).y);
        
        //while the velocity is not valid, move the player to a new position
        while (!IsValidPath(obj,vel))
        {
            //keep pushing the player behind the camera
            position = new Vector3(position.x, position.y, position.z - 5);
       
            //set the object to the new position
            obj.transform.position = position;
            
            //get the new velocity
            vel = LaunchManager.CalculateVelocity(pos,position,launchDuration,(pos - position).y);
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
