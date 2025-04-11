using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetScoreText : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        
        if (scoreText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
            return;
        }
 
        // Set the initial score text
        scoreText.text = "Score: " + ScoreManager.score;
    }
}
