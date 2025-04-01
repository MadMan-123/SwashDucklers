using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreManager : MonoBehaviour 
{   
    //current score
    public int score = 0;
    //score text reference
    [SerializeField] private TextMeshProUGUI scoreText;

    //singleton
    public static ScoreManager Instance;

    //singleton boilerplate
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
    
    //initialize the score
    private void Start()
    {
        UpdateText();
    }

    //add score
    public void AddScore(int value)
    {
        //add the value
        score += value;
        //clamp the score
        score = ClampScore(score);
        //update the ui
        UpdateText();
    }
    
    public void DecreaseScore(int value)
    {
        //decrease the score
        score -= value;
        //clamp the score
        score = ClampScore(score);
        //update the ui
        UpdateText();
    }
    //ensure the score is not negative 
    private static int ClampScore(int value) => value < 0 ? 0 : value;
    
    //update the score text
    private void UpdateText()
    {
        //update the text
        scoreText.text = $"Score:{score.ToString()}";
    }
}
