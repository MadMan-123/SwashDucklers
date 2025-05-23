using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    // Start is called before the first frame update
    void Start()
    {

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
        
        if(StageParameters.levelLength == Length.Endless)
                Hud.SetActive(false);
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
            //todo: remove magic string
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }

        PercentageTimePassed = (currentTime / endTime);

        boatposition =  (EndPosition * PercentageTimePassed) + StartPosition;

        transform.position = new Vector3(boatposition, transform.position.y, transform.position.z);
    }
}
