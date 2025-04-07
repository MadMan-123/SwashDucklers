using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private float time;
    [SerializeField] private float smoothness;


    private void Start()
    {
        StartCoroutine(Raise());
    }

    IEnumerator Raise()
    {
        Transform startTransform = transform;
        float progress = 0;
        var increment = smoothness / time;
        while (progress < 1)
        {
            transform.position = Vector3.Lerp(startTransform.position, (startTransform.position + (Vector3.up * height)), progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
