using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDeletion : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private bool destroyOnStart = true;
    private void Start()
    {
        if(destroyOnStart)
            StartCoroutine(Timer());
    }
    
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
    
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
