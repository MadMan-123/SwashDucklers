using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{

    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {

        playerInput = GetComponent<PlayerInput>();

        //Set device for this player
        switch (playerInput.playerIndex)
        {
            case 0:
                PlayerStats.player1input = playerInput.devices[0];
                break;
            case 1:
                PlayerStats.player2input = playerInput.devices[0];
                break;
            case 2:
                PlayerStats.player3input = playerInput.devices[0];
                break;
            case 3:
                PlayerStats.player4input = playerInput.devices[0];
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Start Game
    public void StartGame(InputAction.CallbackContext value)
    {

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TestScene");

    }
}
