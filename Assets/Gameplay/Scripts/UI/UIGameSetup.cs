using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIGameSetup : MonoBehaviour
{

    [SerializeField] Toggle krakenToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Start Game
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            startGame();
        }

        //Return to previous menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
        }

    }

    public void startGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Boat Scene");
    }

    public void kraken()
    {
        if (krakenToggle.isOn == true)
        {
            StageParameters.krakenEnabled = true;
            Debug.Log("Kraken On");
        }
        else
        {
            StageParameters.krakenEnabled = false;
            Debug.Log("Kraken Off");
        }
    }

}
