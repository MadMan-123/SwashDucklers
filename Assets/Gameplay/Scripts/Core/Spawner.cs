using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    public float interval = 1f;
    public int waveSize = 1;
    public string areaName;
    protected AreaManager.Area area;
    protected GameObjectPool pool;


    protected IEnumerator routine;
    private void Start()
    {
        Init();
    }
    
    public virtual void Init()
    {
        pool = new GameObjectPool(prefab, poolSize, transform);
        routine = _spawn();
        StartCoroutine(routine);
    }


    private IEnumerator _spawn()
    {
        
        area = AreaManager.GetArea(areaName);
        
 
        for (int i = 0; i < waveSize; i++)
        {
            var obj = pool.GetObject();
            
            //set the position
            obj.transform.position = area.GeneratePositionInArea(true,true,true);   
            //draw the position
            Debug.DrawLine(obj.transform.position,obj.transform.position + Vector3.up,Color.red,5f);
        }
        yield return new WaitForSeconds(interval);
        routine = _spawn();
        StartCoroutine(routine);
    }


    public virtual void Return(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }
        
        pool.ReturnObject(gameObject);
    }
    

}
