using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KrakenManager : MonoBehaviour
{
    [SerializeField] GameObject Kraken;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;
    float timeBeforeNext;
    bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        Kraken.SetActive(false);
        StartCoroutine(KrakenSpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeNext = Time.time + SpawnTime;
        if (Time.time > timeBeforeNext)
        {
           // Kraken.SetActive(true);
        }
        //Kraken.SetActive(false);

    }

    IEnumerator KrakenSpawnTimer()
    {
        yield return new WaitForSecondsRealtime(SpawnTime);
        Kraken.SetActive(true);
        yield return new WaitForSecondsRealtime(UpTime);
        Kraken.SetActive(false);
        StartCoroutine(KrakenSpawnTimer());
    }
}
