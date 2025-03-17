using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private List<EnvironmentObjectType> environmentObjects = new();
    private static readonly Vector3 movment = new Vector3(-1, 0, 0);

    private List<GameObject> active = new();
   
    private void Start()
    {
        foreach (var environmentObject in environmentObjects)
        {
            environmentObject.pool = new GameObjectPool(environmentObject.prefab, 10, transform);
        }
    }

    private void Update()
    {
        for (var index = 0; index < active.Count; index++)
        {
            var o = active[index];
            
            if (o == null)
            {
                active.RemoveAt(index);
                continue;
            } 
            
            //move the object
            o.transform.position += movment * (environmentObjects[index].speed * Time.deltaTime);
            
            //check if the object is out of bounds
            if (o.transform.position.x < -10)
            {
                o.SetActive(false);
                active.RemoveAt(index);
            }
        }
    }

    private IEnumerator SpawnRandomObject(float interval = 10)
    {
        var index = Random.Range(0, environmentObjects.Count);
        var current = environmentObjects[index];
        var obj = current.pool.GetObject();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        active.Add(obj);
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnRandomObject());
        
        
    }
    
    
}


public class EnvironmentObjectType
{
    public GameObject prefab;
    public GameObjectPool pool;
    public float speed;
}