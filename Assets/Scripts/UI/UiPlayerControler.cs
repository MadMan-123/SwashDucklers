using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UiPlayerControler : MonoBehaviour
{

    [SerializeField] PlayerInput playerUI;

    [SerializeField] Texture player1Image;
    [SerializeField] Texture player2Image;
    [SerializeField] Texture player3Image;
    [SerializeField] Texture player4Image;

    private GameObject joinText1;
    private GameObject joinText2;
    private GameObject joinText3;
    private GameObject joinText4;
    private GameObject startText;

    private int playerNo;

    // Start is called before the first frame update
    void Start()
    {

        //References to join text
        joinText1 = this.transform.GetChild(1).gameObject;
        joinText2 = this.transform.GetChild(2).gameObject;
        joinText3 = this.transform.GetChild(3).gameObject;
        joinText4 = this.transform.GetChild(4).gameObject;
        startText = this.transform.GetChild(5).gameObject;

        playerNo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPlayerJoined(PlayerInput player)
    {
        playerNo = playerNo + 1;
        startText.SetActive(true);

        //Set parent to the panel so it displays correctly
        player.transform.SetParent(this.transform);

        //Set information based on player ID
        switch (player.playerIndex)
        {
            case 0:
                player.transform.position = new Vector3(500,750,0); //Position
                joinText1.SetActive(false); //Disable join text
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player1Image;
                break;
            case 1:
                player.transform.position = new Vector3(1400, 750, 0);
                joinText2.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player2Image;
                break;
            case 2:
                player.transform.position = new Vector3(500, 300, 0);
                joinText3.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player3Image;
                break;
            case 3:
                player.transform.position = new Vector3(1400, 300, 0);
                joinText4.SetActive(false);
                player.transform.GetChild(2).GetComponent<RawImage>().texture = player4Image;
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

        playerNo = playerNo - 1;

        if (playerNo == 0)
        {
            startText.SetActive(false);
        }

    }

}
