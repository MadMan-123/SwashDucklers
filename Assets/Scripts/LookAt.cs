using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform lookAtTransform;
    void Start()
    {
        //testing
        if (Camera.main != null) lookAtTransform = Camera.main.transform;
    }

    // this is so ugly i know its just going to look at the camera we will have a better one if we have this, just debug so far
    void Update()
    {
        //look at the transform
        transform.LookAt(lookAtTransform,Vector3.up);
        transform.Rotate(transform.up,180);
        transform.Rotate(transform.right,90);
    }
}
