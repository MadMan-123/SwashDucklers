using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private PlayerInput playerInput;

    [SerializeField] Slider R;
    [SerializeField] Slider G;
    [SerializeField] Slider B;

    [SerializeField] Button lastHat;
    [SerializeField] Button forwardHat;

    [SerializeField] TextMeshProUGUI costumeText;
    [SerializeField] TextMeshProUGUI PlayerText;

    float red;
    float green;
    float blue;

    // Start is called before the first frame update
    void Start()
    {

        playerInput = GetComponent<PlayerInput>();

        //Set device for this player
        switch (playerInput.playerIndex)
        {
            case 0:
                PlayerStats.player1input = playerInput.devices[0]; //Set device
                red = PlayerStats.player1Color.r; //Default Colors
                green = PlayerStats.player1Color.g; ;
                blue = PlayerStats.player1Color.b; ;
                PlayerText.text = "Player 1";
                break;
            case 1:
                PlayerStats.player2input = playerInput.devices[0];
                red = PlayerStats.player2Color.r; //Default Colors
                green = PlayerStats.player2Color.g; ;
                blue = PlayerStats.player2Color.b; ;
                PlayerText.text = "Player 2";
                break;
            case 2:
                PlayerStats.player3input = playerInput.devices[0];
                red = PlayerStats.player3Color.r; //Default Colors
                green = PlayerStats.player3Color.g; ;
                blue = PlayerStats.player3Color.b; ;
                PlayerText.text = "Player 3";
                break;
            case 3:
                PlayerStats.player4input = playerInput.devices[0];
                red = PlayerStats.player4Color.r; //Default Colors
                green = PlayerStats.player4Color.g; ;
                blue = PlayerStats.player4Color.b; ;
                PlayerText.text = "Player 4";
                break;
        }

        R.value = red;
        G.value = green;
        B.value = blue;

    }

    // Update is called once per frame
    void Update()
    {
        R.onValueChanged.AddListener((r) => 
        {

            red = r;

        });

        G.onValueChanged.AddListener((g) =>
        {

            green = g;

        });

        B.onValueChanged.AddListener((b) =>
        {

            blue = b;

        });

        //Change Hat
        //lastHat.interactable = true;
        //forwardHat.interactable = true;
        //lastHat.onClick.AddListener(previousHat);
        //forwardHat.onClick.AddListener(nextHat);

        //Set visuals
        switch (playerInput.playerIndex)
        {  
            case 0:
                PlayerStats.player1Color = new Color(red, green, blue);
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player1Hat].name;
                break;
            case 1:
                PlayerStats.player2Color = new Color(red, green, blue);
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player2Hat].name;
                break;
            case 2:
                PlayerStats.player3Color = new Color(red, green, blue);
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player3Hat].name;
                break;
            case 3:
                PlayerStats.player4Color = new Color(red, green, blue);
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player4Hat].name;
                break;
        }
    }

    //Start Game
    public void StartGame(InputAction.CallbackContext value)
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");

    }

    public void Back(InputAction.CallbackContext value)
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("menu test");

    }

    //Next Hat
    public void nextHat()
    {

       // forwardHat.interactable = false;

        switch (playerInput.playerIndex)
        {
            case 0:
                if (PlayerStats.player1Hat < PlayerStats.Hatlist.Count - 1)
                {
                    PlayerStats.player1Hat = PlayerStats.player1Hat + 1;
                }
                break;
            case 1:
                if (PlayerStats.player2Hat < PlayerStats.Hatlist.Count - 1)
                {
                    PlayerStats.player2Hat = PlayerStats.player2Hat + 1;
                }
                break;
            case 2:
                if (PlayerStats.player3Hat < PlayerStats.Hatlist.Count - 1)
                {
                    PlayerStats.player3Hat = PlayerStats.player3Hat + 1;
                }
                break;
            case 3:
                if (PlayerStats.player4Hat < PlayerStats.Hatlist.Count - 1)
                {
                    PlayerStats.player4Hat = PlayerStats.player4Hat + 1;
                }
                break;
        }

    }

    public void previousHat()
    {

        //lastHat.interactable = false;

        switch (playerInput.playerIndex)
        {
            case 0:
                if (PlayerStats.player1Hat > 0)
                {
                    PlayerStats.player1Hat = PlayerStats.player1Hat - 1;
                }
                break;
            case 1:
                if (PlayerStats.player2Hat > 0)
                {
                    PlayerStats.player2Hat = PlayerStats.player2Hat - 1;
                }
                break;
            case 2:
                if (PlayerStats.player3Hat > 0)
                {
                    PlayerStats.player3Hat = PlayerStats.player3Hat - 1;
                }
                break;
            case 3:
                if (PlayerStats.player4Hat > 0)
                {
                    PlayerStats.player4Hat = PlayerStats.player4Hat - 1;
                }
                break;
        }

    }

}
