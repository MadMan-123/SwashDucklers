using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UiPlayerControler : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] Texture player1Image;
    [SerializeField] Texture player2Image;
    [SerializeField] Texture player3Image;
    [SerializeField] Texture player4Image;

    [SerializeField] private Color defaultPlayer1LitColor;
    [SerializeField] private Color defaultPlayer2LitColor;
    [SerializeField] private Color defaultPlayer3LitColor;
    [SerializeField] private Color defaultPlayer4LitColor;

    [SerializeField] List<Hat> hatList;

    private GameObject joinText1;
    private GameObject joinText2;
    private GameObject joinText3;
    private GameObject joinText4;
    private GameObject startText;

    private PlayerInputManager playerManager;

    // Start is called before the first frame update
    void Start()
    {

        //References to join text
        joinText1 = this.transform.GetChild(0).gameObject;
        joinText2 = this.transform.GetChild(1).gameObject;
        joinText3 = this.transform.GetChild(2).gameObject;
        joinText4 = this.transform.GetChild(3).gameObject;
        startText = this.transform.GetChild(4).gameObject;

        //TODO:all this needs to be added to the game manager
        PlayerStats.playerNo = 0;
        PlayerStats.readyPlayers = 0;
        //Load Hats into memory;
        PlayerStats.Hatlist = hatList;

        startText.SetActive(false);

        //playerManager.EnableJoining();

        Debug.Log(PlayerStats.playerNo);

        //Spawn joined players
        for (int i = 0; i < PlayerStats.playerNo; i++)
        {
            switch (i)
            {
                case 0:
                    PlayerInput.Instantiate(playerPrefab, i, null, -1, PlayerStats.player1input);
                    break;
                case 1:
                    PlayerInput.Instantiate(playerPrefab, i, null, -1, PlayerStats.player2input);
                    break;
                case 2:
                    PlayerInput.Instantiate(playerPrefab, i, null, -1, PlayerStats.player3input);
                    break;
                case 3:
                    PlayerInput.Instantiate(playerPrefab, i, null, -1, PlayerStats.player4input);
                    break;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.playerNo > 0)
        {
           
            if (PlayerStats.readyPlayers == PlayerStats.playerNo)
            {
                startText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton9))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(2);
                }

            }
            else 
            {
                startText.SetActive(false);
            }
        }
        else
        {
            startText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            //Disbled for demo day -SD
            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

    }

    public void OnPlayerJoined(PlayerInput player)
    {

        PlayerStats.playerNo = PlayerStats.playerNo + 1;

        Debug.Log("ADD PLAYER");
        Debug.Log(PlayerStats.playerNo);

        //Set parent to the panel so it displays correctly
        player.transform.SetParent(this.transform);

        //player.setupScreen = setupScreen;

        //Set information based on player ID
        switch (player.playerIndex)
        {
            case 0:
                //player.transform.position = new Vector3(500,750,0); //Position

                player.GetComponent<RectTransform>().localPosition = new Vector3(-500, 250, 0);
                player.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f , 1);
                //player.GetComponent<RectTransform>().left = 4;
                //player.GetComponent<RectTransform>().left = 4;
                //player.GetComponent<RectTransform>().left = 4;


                joinText1.SetActive(false); //Disable join text
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player1Image;
                PlayerStats.player1Color = defaultPlayer1LitColor;
                break;
            case 1:
                player.GetComponent<RectTransform>().localPosition = new Vector3(400, 250, 0);
                player.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1);
                joinText2.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player2Image;
                PlayerStats.player2Color = defaultPlayer2LitColor;
                break;
            case 2:
                player.GetComponent<RectTransform>().localPosition = new Vector3(-500, -250, 0);
                player.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1);
                joinText3.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player3Image;
                PlayerStats.player3Color = defaultPlayer3LitColor;
                break;
            case 3:
                player.GetComponent<RectTransform>().localPosition = new Vector3(400, -250, 0);
                player.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1);
                joinText4.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player4Image;
                PlayerStats.player4Color = defaultPlayer4LitColor;
                break;
        }
    }

    public void OnPlayerLeft(PlayerInput player)
    {


        //Readd join text
        switch (player.playerIndex)
        {
            case 0:
                player.transform.position = new Vector3(500, 750, 0);
                joinText1.SetActive(true);
                break;
            case 1:
                player.transform.position = new Vector3(1400, 750, 0);
                joinText2.SetActive(true);
                break;
            case 2:
                player.transform.position = new Vector3(500, 300, 0);
                joinText3.SetActive(true);
                break;
            case 3:
                player.transform.position = new Vector3(1400, 300, 0);
                joinText4.SetActive(true);
                break;
        }

        //PlayerStats.playerNo = PlayerStats.playerNo - 1;

    }

}
