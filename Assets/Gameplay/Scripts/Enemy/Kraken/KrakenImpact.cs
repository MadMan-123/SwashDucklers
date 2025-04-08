using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenImpact : MonoBehaviour
{
  [SerializeField]private GameObject impactAnim;
  private Transform cam;

  private void Start()
  {
    cam = Camera.main?.gameObject.transform;
  }

  private void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.TryGetComponent<CannnonBall>(out var ball))
    {
      Vector3 lookDir = cam.transform.position - col.gameObject.transform.position;
      Quaternion direction = Quaternion.LookRotation(lookDir);
      Instantiate(impactAnim,col.transform.position,direction);
    }
    Debug.Log("Impacted");
    
  }
}

