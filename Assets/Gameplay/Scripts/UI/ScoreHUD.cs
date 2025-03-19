using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI cargoText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    //No texts to be updated per frame
    /*// Update is called once per frame
    void Update()
    {
        cargoText.text = StageParameters.currentCargo.ToString() + "/" + StageParameters.startingCargo.ToString();
        /*
        scoreText.text = StageParameters.Score.ToString();
        #1#
    }*/
}
