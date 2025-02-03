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

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position.x;
        EndPosition = Screen.width - 200;

        currentTime = 0;
        endTime = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < endTime)
        {
            currentTime = currentTime + 1 * Time.deltaTime;
        }
        else 
        {
            currentTime = endTime;
            //GAME WON
        }

        PercentageTimePassed = (currentTime / endTime);

        boatposition =  (EndPosition * PercentageTimePassed) + StartPosition;

        transform.position = new Vector3(boatposition, transform.position.y, transform.position.z);
    }
}
