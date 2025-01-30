

using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public float score;
    public float transitionSpeed = 1000;
    float displayScore;
    public float pointsPerSecond = 20;
    public TextMeshProUGUI scoreText;

    //add points over time
    private void Update()
    {
        score -= pointsPerSecond * Time.deltaTime;
        displayScore = Mathf.MoveTowards(displayScore, score, transitionSpeed * Time.deltaTime);
        UpdateScoreDisplay();
    }


    public void DecreaseScore(float amount)
    {
        score += amount;
    }
    public void UpdateScoreDisplay()
    {
        scoreText.text = string.Format("ship health: {0:0}%", displayScore);
    }

}