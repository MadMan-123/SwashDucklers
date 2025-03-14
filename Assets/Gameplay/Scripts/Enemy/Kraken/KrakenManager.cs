using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Cinemachine;

public class KrakenManager : MonoBehaviour
{
    //This script will control the krakens behaviours and when it should do whatever

    [SerializeField] GameObject kraken;
    [SerializeField] GameObject tentacles;
    [SerializeField] GameObject krakenHealth;
    [SerializeField] private Health health;
    [SerializeField] KrakenHud krakenHud;
    [SerializeField] GameObject gameTimer;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;
    [SerializeField] float waterChangeDuration;

    [SerializeField] Weather weather;

    [SerializeField] private CinemachineTargetGroup cameraTarget;

    float timeBeforeNext;
    //bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (StageParameters.krakenEnabled == true)
        {
            kraken.SetActive(false);
            tentacles.SetActive(false);
            krakenHealth.SetActive(false);

            if (StageParameters.levelLength != Length.Endless)
            {
                gameTimer.SetActive(true);
            }
            StartCoroutine(KrakenSpawnTimer());

        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator KrakenSpawnTimer()
    {
        yield return new WaitForSeconds(5f);
        weather.KrakenSpawn();
        yield return new WaitForSeconds(5f);
        kraken.SetActive(true);
        tentacles.SetActive(true);
        krakenHealth.SetActive(true);
        gameTimer.SetActive(false);
        CameraShake.Instance.ShakeCamera(1.5f, waterChangeDuration + 0.5f);
        SoundManager.PlayAudioClip("KrakenSpawn", this.transform.position, 1f);
        cameraTarget.AddMember(kraken.transform, 1, 2.5f);
    }

    //Function called by cannons when fired -SD
    public void krakenHit()
    {
        SoundManager.PlayAudioClip("KrakenHurt", this.transform.position, 2f);
        krakenHud.KrakenHit();

        if (!health.IsDead) return;
        //Kraken is dead
        kraken.SetActive(false);
        tentacles.SetActive(false);
        krakenHealth.SetActive(false);
        
        health.SetHealth(0);
        if (StageParameters.levelLength != Length.Endless)
        {
            gameTimer.SetActive(true);
        }
        weather.KrakenDeSpawn();
        cameraTarget.RemoveMember(kraken.transform);
        StartCoroutine(KrakenSpawnTimer());
    }

}
