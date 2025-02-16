using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDeletion : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
