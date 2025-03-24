using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    
    [SerializeField] private List<EnvironmentObjectType> environmentObjects = new();
    [SerializeField] private float outOfBoundsDistance = -25;
    [SerializeField] private bool debug;
    
    private static readonly Vector3 movment = new Vector3(-1, 0, 0);
    private static readonly Vector3 upMovment = new Vector3(0, 1, 0);
    private const int MaxObjects = 5;
    private Waves waves;
    private EnvironmentObject[] active = new EnvironmentObject[MaxObjects * 2];

    
    
    //edit this to stop moving stuff and cancel the recursive coroutine, 
    public bool shouldMove = true;
    private bool wait = false;
    private void Start()
    {
        for (var index = 0; index < environmentObjects.Count; index++)
        {
            var environmentObject = environmentObjects[index];
            environmentObject.pools = new GameObjectPool[environmentObject.prefabs.Length];
            for (int i = 0; i < environmentObject.prefabs.Length; i++)
            {
                environmentObject.pools[i] = new GameObjectPool(environmentObject.prefabs[i], MaxObjects, transform);
            }
            
            waves = FindObjectOfType<Waves>(); // Find the wave generator in the scene
        }
        

        //for each environment object prefab 
        for (var i = 0; i < environmentObjects.Count; i++)
        {
            //start the coroutine to spawn the objects
            StartCoroutine(SpawnRandomObject());
        }
    }

    private void Update()
    {
        for (var index = 0; index < active.Length; index++)
    {
        var o = active[index];
        if (o == null) continue;

        //We no longer use Waves.cs -MW
        
        /*// Smooth Y transition
        if (waves != null)
        {
            float waveY = waves.GetWaveHeight(o.transform.position.x, o.transform.position.z);
            Vector3 pos = o.transform.position;
            
            pos.y = Mathf.Lerp(pos.y, waveY, Time.deltaTime * 5.0f);
            o.transform.position = pos;
        }*/

        // Only move the object if shouldMove is true
        if (shouldMove)
        {
            // Move the object along the X-axis (or the direction you choose)
            o.transform.position += movment * (o.speed * Time.deltaTime) + upMovment * (Mathf.Sin(Time.time * o.bobSpeed ) *  o.bobHeight * Time.deltaTime);
        }

        // Check if the object is out of bounds
        if (o.transform.position.x < outOfBoundsDistance)
        {
            o.transform.gameObject.SetActive(false);
            active[index] = null;
        }
    }
}

    public IEnumerator SpawnRandomObject(float interval = 15)
    {
        if(!shouldMove) yield break;
        //get a random enviroment object
        var index = Random.Range(0, environmentObjects.Count );
        var current = environmentObjects[index];
        
        //get a random prefab from the pools
        var pool = current.pools[Random.Range(0, current.pools.Length)];
        //check if its valid
        if (pool == null) yield break;
        
        var obj = pool.GetObject();
        
        //set the position of the object
        obj.transform.position = current.position;
        obj.SetActive(true);
        
        int validIndex = active.ToList().FindIndex(x => x == null);
        
        if (validIndex == -1)
        {
            yield return new WaitForSeconds(interval);
            StartCoroutine(SpawnRandomObject());
            yield break;
        }
       
        active[validIndex] = new EnvironmentObject
        {
            transform = obj.transform,
            speed = current.speed,
            bobSpeed = current.bobSpeed,
            bobHeight = current.bobHeight
            
        };
        
        
        yield return new WaitForSeconds(interval);
        if(shouldMove) 
            StartCoroutine(SpawnRandomObject()); 
    }


    
    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        if (!debug) return;
        //if there is a valid environment object
        if (environmentObjects.Count > 0)
        {
            for (var i = 0; i < environmentObjects.Count; i++)
            {
                var environmentObject = environmentObjects[i];
                if(environmentObject == null || environmentObject.prefabs.Length == 0) continue;
                
                //make a colour that is based of the speed, 0 is red and 10 is green
                var color = Color.Lerp(Color.red, Color.green, environmentObject.speed / 10);
                
                
               
                //set the gizmo colour
                Gizmos.color = color;
                
                
                
                //get a formated string of each prefab name line for line
                var prefabNames = environmentObject.prefabs.Aggregate("", (current, prefab) => current + prefab.name + "\n");
                
                #if UNITY_EDITOR
                    //draw the name of the prefab above this and display the speed of the object
                    Handles.Label(environmentObject.position + Vector3.up * 1.25f + Vector3.right / 2 + Vector3.forward /2,  prefabNames +" \nSpeed: " + environmentObject.speed);
                #endif
                Gizmos.DrawWireCube(environmentObject.position, Vector3.one);
            }
            
        }
        
        //draw the out of bounds line
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(outOfBoundsDistance, 0, 0), new Vector3(outOfBoundsDistance, 0, 100));
        
    }
    
}

public class EnvironmentObject
{
    public Transform transform;
    public float speed;
    public float bobSpeed;
    public float bobHeight;
    
}

[Serializable]
public class EnvironmentObjectType
{
    public GameObject[] prefabs;
    public Vector3 position;
    public GameObjectPool[] pools;
    public float speed;
    public float bobSpeed;
    public float bobHeight;
}