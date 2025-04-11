using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class GameTimer : MonoBehaviour
{
    [SerializeField] float StartPosition;
    [SerializeField] float EndPosition;

    [SerializeField] float currentTime;
    [SerializeField] float endTime;
    [SerializeField] float PercentageTimePassed;
    [SerializeField] float boatposition;

    [SerializeField] GameObject Hud;
    [SerializeField] RectTransform Water;
    [SerializeField] RectTransform HudRect;

    
    [SerializeField] float shortTime = 30;
    [SerializeField] float mediumTime = 45;
    [SerializeField] float longTime = 90;
    [SerializeField] float secondsToAdd = 10;
    private int levelIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        
        
        //make sure this dosent delete when we load a new scene
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        
        //StartPosition = (0 + 500) * (Screen.width / 9020f);
        //EndPosition = (HudRect.rect.width - 500) * (Screen.width/9020f);//Width of this object;

        currentTime = 0;

        //Caculate end time -SD
        //Values can be changed later -SD

        endTime = StageParameters.levelLength switch
        {
            Length.Short => shortTime,
            Length.Medium => mediumTime,
            Length.Long => longTime,

        };
        
        //check if we should add time to the end time
        if(!TempGameDataCarrier.IsStart)
            endTime += secondsToAdd;
        
        
        if(StageParameters.levelLength == Length.Endless)
                Hud.SetActive(false);
        
        if(TempGameDataCarrier.IsStart)
            TempGameDataCarrier.IsStart = false;
        
        
        
    }

    void ResetState()
    {
        //Reset the timer
        currentTime = 0;
        endTime = StageParameters.levelLength switch
        {
            Length.Short => shortTime,
            Length.Medium => mediumTime,
            Length.Long => longTime,

        };
        
        //Reset the boat position
        boatposition = StartPosition;
        
        
    }
    
    // Update is called once per frame
    void Update()
    {

        //Looks complicated but effectively  (edge of screen +/- offset * modifier for current resolution) -SD
        StartPosition = (570 * (Screen.width / 1920f));
        EndPosition = (HudRect.rect.width - 1150) * (Screen.width / 1920f);

        if (currentTime < endTime)
        {
            currentTime += 1 * Time.deltaTime;
        }
        else 
        {
            currentTime = endTime;
            //GAME WON
            //play transition animation and then load this scene again 
            
            //play transition animation
            
            ResetState();

            //add score based on cargo 
            //from 0 - 100 add score from 0 cargo to 5 cargo
            var cargo = StageParameters.currentCargo;
            var score = (cargo / 5) * 100;
           
            //add score to manager
            ScoreManager.Instance.AddScore(score);
            
            //save data to send to the next scene
            TempGameData.LeakSpawnIncrease += 0.2f;
            TempGameData.CrabSpawnIncrease += 0.2f;

            TempGameData.CrabSpawnSize = 1;
            TempGameData.KrakenHealthIncrease = 1;
            
            
            
            
            if(levelIndex != -1)
                MenuManager.instance.LoadScene(levelIndex,0.5f);
            
            
        }

        PercentageTimePassed = (currentTime / endTime);

        boatposition =  (EndPosition * PercentageTimePassed) + StartPosition;

        transform.position = new Vector3(boatposition, transform.position.y, transform.position.z);
    }
}
