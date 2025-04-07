using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedPrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject thingToSpawn;
    [SerializeField] private float timeToWait;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private bool lookAtCamera;
    public Transform vfxHolder;
    private GameObject cam;

    private IEnumerator Start()
    {
        cam = Camera.main?.gameObject;
        if (thingToSpawn == null) yield break;
        var pos = transform.position +  new Vector3(0, 0.5f, 0);
        if (spawnLocation != null) { pos = spawnLocation.position; }
        Vector3 lookDir = (cam.transform.position - pos).normalized;
        Quaternion direction = Quaternion.LookRotation(lookDir);
        
        yield return new WaitForSeconds(timeToWait);
        Instantiate(thingToSpawn, pos, direction,vfxHolder);
    }
}
