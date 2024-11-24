using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private PlayerInput playerInput;

    [SerializeField] Slider R;
    [SerializeField] Slider G;
    [SerializeField] Slider B;

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
                break;
            case 1:
                PlayerStats.player2input = playerInput.devices[0];
                red = PlayerStats.player2Color.r; //Default Colors
                green = PlayerStats.player2Color.g; ;
                blue = PlayerStats.player2Color.b; ;
                break;
            case 2:
                PlayerStats.player3input = playerInput.devices[0];
                red = PlayerStats.player3Color.r; //Default Colors
                green = PlayerStats.player3Color.g; ;
                blue = PlayerStats.player3Color.b; ;
                break;
            case 3:
                PlayerStats.player4input = playerInput.devices[0];
                red = PlayerStats.player4Color.r; //Default Colors
                green = PlayerStats.player4Color.g; ;
                blue = PlayerStats.player4Color.b; ;
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

        //Set visuals
        switch (playerInput.playerIndex)
        {  
            case 0:
                PlayerStats.player1Color = new Color(red, green, blue);
                break;
            case 1:
                PlayerStats.player2Color = new Color(red, green, blue);
                break;
            case 2:
                PlayerStats.player3Color = new Color(red, green, blue);
                break;
            case 3:
                PlayerStats.player4Color = new Color(red, green, blue);
                break;
        }
    }

    //Start Game
    public void StartGame(InputAction.CallbackContext value)
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");

    }
}
