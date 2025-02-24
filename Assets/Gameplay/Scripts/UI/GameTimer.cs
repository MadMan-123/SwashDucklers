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

    // Start is called before the first frame update
    void Start()
    {

        //StartPosition = (0 + 500) * (Screen.width / 9020f);
        //EndPosition = (HudRect.rect.width - 500) * (Screen.width/9020f);//Width of this object;

        currentTime = 0;

        //Caculate end time -SD
        //Values can be changed later -SD
        switch (StageParameters.levelLength)
        {
            case Length.Short:
                endTime = 30;
                break;
            case Length.Medium:
                endTime = 60;
                break;
            case Length.Long:
                endTime = 90;
                break;
            case Length.Endless:
                Hud.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Looks complicated but effectively  (edge of screen +/- offset * modifier for current resolution) -SD
        StartPosition = (570 * (Screen.width / 1920f));
        EndPosition = (HudRect.rect.width - 1150) * (Screen.width / 1920f);

        if (currentTime < endTime)
        {
            currentTime = currentTime + 1 * Time.deltaTime;
        }
        else 
        {
            currentTime = endTime;
            //GAME WON
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
        }

        PercentageTimePassed = (currentTime / endTime);

        boatposition =  (EndPosition * PercentageTimePassed) + StartPosition;

        transform.position = new Vector3(boatposition, transform.position.y, transform.position.z);
    }
}
