using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KrakenManager : MonoBehaviour
{
    //This script will control the krakens behaviours and when it should do whatever

    [SerializeField] GameObject kraken;
    [SerializeField] GameObject tentacles;
    [SerializeField] GameObject krakenHealth;
    [SerializeField] KrakenHud krakenHud;
    [SerializeField] GameObject gameTimer;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;
    [SerializeField] float waterChangeDuration;

    [SerializeField] Weather weather;

    float timeBeforeNext;
    bool isActive = false;

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
        yield return new WaitForSeconds(2.5f);
        //SoundManager.PlayAudioClip("Lightning OR Something", this.transform.position, 1f);
        yield return new WaitForSeconds(1.25f);
        tentacles.SetActive(true); 
        yield return new WaitForSeconds(1.25f);
        kraken.SetActive(true);
        krakenHealth.SetActive(true);
        gameTimer.SetActive(false);
        CameraShake.Instance.ShakeCamera(1.5f, waterChangeDuration + 0.5f);
        SoundManager.PlayAudioClip("KrakenSpawn", this.transform.position, 1f);
    }

    //Function called by cannons when fired -SD
    public void krakenHit()
    {
        SoundManager.PlayAudioClip("KrakenHurt", this.transform.position, 1f);
        krakenHud.KrakenHit();

        if (krakenHud.currentHealth == 0)
        {
            //Kraken is dead
            kraken.SetActive(false);
            tentacles.SetActive(false);
            krakenHealth.SetActive(false);
            if (StageParameters.levelLength != Length.Endless)
            {
                gameTimer.SetActive(true);
            }
            weather.KrakenDeSpawn();
            StartCoroutine(KrakenSpawnTimer());
            
        }
    }

}
