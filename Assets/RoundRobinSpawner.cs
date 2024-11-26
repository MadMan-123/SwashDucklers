using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundRobinSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects;
    [SerializeField] private GameObject current;
    [SerializeField] private float intervalOn = 20, intervalOff = 30;

    IEnumerator routine;

    private void Start()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
            routine = _spawnObject();
            StartCoroutine(routine);
        }
    }
    IEnumerator _spawnObject()
    {
        var index = Random.Range(0, gameObjects.Count);
        current = gameObjects[index];
        current.SetActive(true);
        yield return new WaitForSeconds(intervalOn);
        current.SetActive(false);
        yield return new WaitForSeconds(intervalOff);

        routine = _spawnObject();
        StartCoroutine(routine);

    }

    

}
