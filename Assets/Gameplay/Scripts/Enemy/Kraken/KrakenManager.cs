using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Cinemachine;

public class KrakenManager : MonoBehaviour
{
    //This script will control the krakens behaviours and when it should do whatever
    
    [SerializeField] private Health health;
    [SerializeField] KrakenHud krakenHud;
    [SerializeField] GameObject gameTimer;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;
    [SerializeField] float waterChangeDuration;

    [Header("Key Objects")]
    [SerializeField] GameObject krakenBody;
    [SerializeField] GameObject krakenBodyAsset;
    private Animator krakenBodyAnimator;
    [SerializeField] GameObject tentacles;
    private TentacleAI tentacleAI;
    [SerializeField] GameObject krakenHealth;
    
    [Header("Camera")]
    [SerializeField] private CinemachineTargetGroup cameraTarget;
    [SerializeField] private float cameraPullRadius=2.5f;
    [SerializeField] private float cameraPullWeight=1f;
    
    [Header("Timings")]
    [SerializeField] private float bodySpawns=12.5f;
    [SerializeField] private float tentaclesSpawns=7.5f;
    [SerializeField] private float weatherChanges=5f;
    
    [Header("Other World Stuff")]
    [SerializeField] Weather weather;
    [SerializeField] private EnvironmentManager environmentManager;

    float timeBeforeNext;
    //bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if(krakenBody.TryGetComponent(out Animator krakenAnimator)){krakenBodyAnimator = krakenAnimator;}
        if(tentacles.TryGetComponent(out TentacleAI tentAI)){tentacleAI = tentAI;}
            
        if (StageParameters.krakenEnabled == true)
        {
            krakenBody.SetActive(false);
            tentacles.SetActive(false);
            krakenHealth.SetActive(false);

            if (StageParameters.levelLength != Length.Endless)
            {
                gameTimer.SetActive(true);
            }
            StartRoutines();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void StartRoutines()
    {
        //efficiency, whats that? - TS
        StartCoroutine(KrakenBodySpawn());
        StartCoroutine(TentacleSpawn());
        StartCoroutine(WeatherSpawn());

    }
    IEnumerator KrakenBodySpawn()
    {
        //Initiates body related functions - TS
        yield return new WaitForSeconds(bodySpawns);
        krakenBody.SetActive(true);
        krakenHealth.SetActive(true);
        SoundManager.PlayAudioClip("KrakenSpawn", this.transform.position, 1f);
        cameraTarget.AddMember(krakenBody.transform, cameraPullWeight, cameraPullRadius);
        
        //tell the environment manager to stop moving objects
        environmentManager.shouldMove = false; 
        
    }

    IEnumerator WeatherSpawn()
    {
        //Initiates Weather (should be activated first) - TS
        yield return new WaitForSeconds(weatherChanges);
        weather.KrakenSpawn();
    }

    IEnumerator TentacleSpawn()
    {
        //Spawns everything related to the tentacles - TS
        yield return new WaitForSeconds(tentaclesSpawns);
        tentacles.SetActive(true);
        gameTimer.SetActive(false);
    }

    //Function called by cannons when fired -SD
    public void krakenHit()
    {
        SoundManager.PlayAudioClip("KrakenHit", this.transform.position, 2f);
        krakenHud.KrakenHit();

        if (!health.IsDead) return;
        //Kraken is dead
        //krakenBodyAnimator.SetTrigger("KrakenDies");
        krakenBody.SetActive(false);
        StartCoroutine(tentacleAI.KrakenDeath());
        krakenHealth.SetActive(false);
       
        //reengage the environment manager
        environmentManager.shouldMove = true;
        StartCoroutine(environmentManager.SpawnRandomObject());
        
        
        health.SetHealth(0);
        if (StageParameters.levelLength != Length.Endless)
        {
            gameTimer.SetActive(true);
        }
        weather.KrakenDeSpawn();
        cameraTarget.RemoveMember(krakenBody.transform);
        //StartRoutines();
    }

    public void DisableBody()
    {
        //Can also just remove in its own script, will see - TS
        krakenBody.SetActive(false);
    }

    public void DisableTentacles()
    {
        //Can also just remove in its own script, will see - TS
        tentacles.SetActive(false);
    }

}
