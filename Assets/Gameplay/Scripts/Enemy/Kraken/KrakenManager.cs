using System;
using System.Collections;
using System.ComponentModel;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

//using UnityEditor.Experimental.GraphView;

public class KrakenManager : MonoBehaviour
{
    private static readonly int TextureSpeed = Shader.PropertyToID("_TextureSpeed");

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
    [SerializeField] Kraken krakenBodyScript;
    private Animator krakenBodyAnimator;
    [SerializeField] GameObject tentacles;
    private TentacleAI tentacleAI;
    [SerializeField] GameObject krakenHealth;
    [SerializeField] private GameObject cosmeticTentacles;
        
    
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
    
    [SerializeField] private float textureSpeed = -1, waterKrakenModifer = 2;
    [SerializeField] private Material waterMaterial;

    float timeBeforeNext;

    [SerializeField] private float delayAfterDeath = 30f;
    //bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
        //assert we have a valid water material
        Assert.IsNotNull(waterMaterial,"waterMaterial is null");
       
        //get the water speed
        textureSpeed = waterMaterial.GetFloat(TextureSpeed);
        
        if (!krakenBodyAsset.TryGetComponent(out krakenBodyAnimator))
        {
            Debug.LogError("Kraken Body Animator not found");
        }

        if (!tentacles.TryGetComponent(out tentacleAI))
        {
            Debug.LogError("Tentacle AI not found");
        }
            
        if (StageParameters.krakenEnabled == true)
        {
            krakenBody.SetActive(false);
            tentacles.SetActive(false);
            krakenHealth.SetActive(false);

            if (StageParameters.levelLength != Length.Endless)
            {
                gameTimer.SetActive(true);
            }
            StartCoroutine(StartRoutines());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator StartRoutines(float waitTime = 0)
    {
        
        yield return new WaitForSeconds(waitTime);
        //efficiency, whats that? - TS
        StartCoroutine(KrakenBodySpawn());
        StartCoroutine(TentacleSpawn());
        StartCoroutine(WeatherSpawn());

    }
    IEnumerator KrakenBodySpawn()
    {
     
        yield return new WaitForSeconds(bodySpawns -2);
        TurnUpKrakenAudio();
        //Initiates body related functions - TS
        yield return new WaitForSeconds(2);
        krakenBody.SetActive(true);
        krakenHealth.SetActive(true);
        //change the speed of the waves 
        
        
        waterMaterial.SetFloat(TextureSpeed, textureSpeed / waterKrakenModifer);


        
        SoundManager.PlayAudioClip("KrakenSpawn", transform.position, 1f);
        cameraTarget.AddMember(krakenBody.transform, cameraPullWeight, cameraPullRadius);
        
        //tell the environment manager to stop moving objects
        environmentManager.shouldMove = false; 
        
    }

    void TurnUpKrakenAudio()
    {
        if (MusicManager.instance != null)
        {
            //lerp the kraken audio up
            float target = 0.5f;

            //lerp the kraken audio up
            MusicManager.instance.LerpChannelVolume(MusicManager.SpookyLeadChannel, target, 1.5f);
            MusicManager.instance.LerpChannelVolume(MusicManager.SpookyBassChannel, target, 1.5f);
            //lerp the jolly channel down
            MusicManager.instance.LerpChannelVolume(MusicManager.JollyChannel, 0, 1.5f);
        }
    }

    void TurnDownKrakenAudio()
    {
        //lerp the kraken audio up
        float target = 0.5f;
        
        //lerp the kraken audio up
        MusicManager.instance.LerpChannelVolume(MusicManager.SpookyLeadChannel,0,2f);
        MusicManager.instance.LerpChannelVolume(MusicManager.SpookyBassChannel,0,2f);
        
        //lerp the jolly channel down
        MusicManager.instance.LerpChannelVolume(MusicManager.JollyChannel,target,2);
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
        cosmeticTentacles.SetActive(true);
    }

    //Function called by cannons when fired -SD
    public void krakenHit()
    {
        SoundManager.PlayAudioClip("KrakenHit", transform.position, 2f);
        
        if (!health.IsDead)
        {
            krakenBodyAnimator.SetTrigger("KrakenHit");
        }
        else
        {
            TurnDownKrakenAudio();
            //Kraken is dead
            krakenBodyAnimator.SetTrigger("KrakenDies");
            //krakenBody.SetActive(false);
            StartCoroutine(tentacleAI.KrakenDeath());
            StartCoroutine(krakenBodyScript.KrakenDead());

            //reengage the environment manager
            environmentManager.shouldMove = true;
            environmentManager.StartEachLaneUp();
            

            //make the water move regularly again
            
            waterMaterial.SetFloat(TextureSpeed, textureSpeed);
            health.SetHealth(0);
            if (StageParameters.levelLength != Length.Endless)
            {
                gameTimer.SetActive(true);
            }

            weather.KrakenDeSpawn();
            StartCoroutine(StartRoutines(delayAfterDeath));
        }
    }

    public void DisableBody()
    {
        //Can also just remove in its own script, will see - TS
        krakenBody.SetActive(false);
        cameraTarget.RemoveMember(krakenBody.transform); // Could also have this in the main Kraken Dying - TS
        krakenHealth.SetActive(false);
    }

    public void DisableTentacles()
    {
        //Can also just remove in its own script, will see - TS
        tentacles.SetActive(false);
    }


    private void OnDestroy()
    {
        //set the water speed back to normal
        waterMaterial.SetFloat(TextureSpeed, textureSpeed);
    }

    public void DisableCosmeticTentacles()
    {
        cosmeticTentacles.SetActive(false);
    }
}
