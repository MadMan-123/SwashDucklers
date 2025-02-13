using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KrakenManager : MonoBehaviour
{
    //This script will control the krakens behaviours and when it should do whatever

    [SerializeField] GameObject kraken;
    [SerializeField] GameObject tentacles;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;
    [SerializeField] float waterChangeDuration;

    [SerializeField] Weather weather;

    float timeBeforeNext;

    // Start is called before the first frame update
    void Start()
    {
        kraken.SetActive(false);
        tentacles.SetActive(false);
        StartCoroutine(KrakenSpawnTimer());
    }

    IEnumerator KrakenSpawnTimer()
    {
        yield return new WaitForSeconds(5f);
        weather.KrakenSpawn();
        yield return new WaitForSeconds(5f);
        kraken.SetActive(true);
        tentacles.SetActive(true);
        CameraShake.Instance.ShakeCamera(1.5f, waterChangeDuration + 0.5f);
    }
}
