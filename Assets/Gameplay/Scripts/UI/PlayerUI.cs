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

    [SerializeField] GameObject ColorSliders;
    [SerializeField] GameObject ColorButtons;

    [SerializeField] Slider R;
    [SerializeField] Slider G;
    [SerializeField] Slider B;

    [SerializeField] Button lastHat;
    [SerializeField] Button forwardHat;

    [SerializeField] TextMeshProUGUI costumeText;
    //[SerializeField] TextMeshProUGUI PlayerText;
    [SerializeField] private Image playerSprite;
    [SerializeField] private Sprite[] playerTextSprites;
    [SerializeField] private Sprite[] readySprites;
    [SerializeField] private Image readyButton;
    [SerializeField] GameObject ReadyIcon;

    float red;
    float green;
    float blue;
    int HatID;

    public bool isReady = false;


    private void PlayerUIHandle()
    {
        //get the correct player input
        var input = this.playerInput.playerIndex switch
        {
            0 => PlayerStats.player1input,
            1 => PlayerStats.player2input,
            2 => PlayerStats.player3input,
            3 => PlayerStats.player4input,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        //get the colours
        var colour = playerInput.playerIndex switch
        {
            0 => PlayerStats.player1Color,
            1 => PlayerStats.player2Color,
            2 => PlayerStats.player3Color,
            3 => PlayerStats.player4Color,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        input = playerInput.devices[0]; //Set device
        red = colour.r; //Default Colors
        green = colour.g; ;
        blue = colour.b; ;
        playerSprite.sprite = playerTextSprites[0];
        //PlayerText.text = "Player 1";
                
    }
    
    // Start is called before the first frame update
    void Start()
    {

        playerInput = GetComponent<PlayerInput>();

        PlayerUIHandle();
              
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
                PlayerStats.player1Hat = HatID;
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player1Hat].name;
                break;
            case 1:
                PlayerStats.player2Color = new Color(red, green, blue);
                PlayerStats.player2Hat = HatID;
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player2Hat].name;
                break;
            case 2:
                PlayerStats.player3Color = new Color(red, green, blue);
                PlayerStats.player3Hat = HatID;
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player3Hat].name;
                break;
            case 3:
                PlayerStats.player4Color = new Color(red, green, blue);
                PlayerStats.player4Hat = HatID;
                costumeText.text = PlayerStats.Hatlist[PlayerStats.player4Hat].name;
                break;
        }

    }

    //Start Game
    public void StartGame(InputAction.CallbackContext value)
    {
    }

    public void Back(InputAction.CallbackContext value)
    {

    }

    public void readyUp()
    {
      
        if (isReady == false)
        {
            isReady = true;
            ReadyIcon.SetActive(true);
            readyButton.sprite = readySprites[1];
            PlayerStats.readyPlayers++;
        }
        else
        {
            isReady = false;
            ReadyIcon.SetActive(false);
            readyButton.sprite = readySprites[0];
            PlayerStats.readyPlayers--;
        }
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

    public void setHat(int id)
    {
        HatID = id;
    }

    public void setRed(float r) 
    {
        red = r;
    }

    public void setGreen(float g)
    {
        green = g;
    }

    public void setBlue(float b)
    {
        blue = b;
    }

    public void customColor()
    {
        if (ColorSliders.activeInHierarchy == false)
        {
            ColorSliders.SetActive(true);
            ColorButtons.SetActive(false);
        }
        else
        {
            ColorSliders.SetActive(false);
            ColorButtons.SetActive(true);
        }
    }
}
