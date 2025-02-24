using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = ((int)StageParameters.currentShipHealth).ToString() + "/" + StageParameters.maxShipHealth.ToString();
    }
}
