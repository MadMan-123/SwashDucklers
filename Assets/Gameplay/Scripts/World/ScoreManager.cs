using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreManager : MonoBehaviour 
{
    
    
    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    public static ScoreManager Instance;

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
    
    private void Start()
    {
        UpdateText();
    }

    public void AddScore(int value)
    {
        score += value;
        score = ClampScore(score);
        UpdateText();
    }
    
    public void DecreaseScore(int value)
    {
        score -= value;
        score = ClampScore(score);
        UpdateText();
    }
    private int ClampScore(int value) => value < 0 ? 0 : value;
    private void UpdateText()
    {
        scoreText.text = $"Score:{score.ToString()}";
    }
}
