using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIGameSetup : MonoBehaviour
{

    //References to input, uses general input so all players can control this menu -SD
    private Input Input;

    //References to the Menu Objects-SD
    [SerializeField] Button lastHat;
    [SerializeField] Button forwardHat;

    //Reference to character select screen
    [SerializeField] GameObject characterScreen;

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
            UnityEngine.SceneManagement.SceneManager.LoadScene("Boat Scene");
        }

        //Return to previous menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
        }
    }
}
