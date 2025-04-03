using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform lookAtTransform;
    private Transform cam;
    void Start()
    {
        //testing
        cam = Camera.main?.transform;
    }

    // this is so ugly i know its just going to look at the camera we will have a better one if we have this, just debug so far
    void Update()
    {
        //var lookDir = (cam.position - transform.position).normalized;
        //look at the transform
        transform.LookAt(cam.position);
        transform.Rotate(transform.up,180);
        transform.Rotate(transform.right,50);
    }
}
