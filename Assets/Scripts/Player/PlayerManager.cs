using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public float PlayerNo;


    [SerializeField] private Vector3 player1Spawn;
    [SerializeField] private Vector3 player2Spawn;
    [SerializeField] private Vector3 player3Spawn;
    [SerializeField] private Vector3 player4Spawn;

    [SerializeField] private Vector3 player1SpawnRotation;
    [SerializeField] private Vector3 player2SpawnRotation;
    [SerializeField] private Vector3 player3SpawnRotation;
    [SerializeField] private Vector3 player4SpawnRotation;

    [SerializeField] private Color player1LitColor;
    [SerializeField] private Color player1DarkColor;
    [SerializeField] private Color player2LitColor;
    [SerializeField] private Color player2DarkColor;
    [SerializeField] private Color player3LitColor;
    [SerializeField] private Color player3DarkColor;
    [SerializeField] private Color player4LitColor;
    [SerializeField] private Color player4DarkColor;

    private Input Input;

    // Start is called before the first frame update
    void Start()
    {
        PlayerNo = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerJoined(PlayerInput player)
    {

        PlayerNo = PlayerNo + 1;
        Debug.Log("Player joined");
        Debug.Log($"Players:{PlayerNo}");

        switch (player.playerIndex)
        {
            case 0:
               player.GetComponent<PlayerControler>().spawnpoint = player1Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player1SpawnRotation;
                player.GetComponent<PlayerControler>().litColor = player1LitColor;
                player.GetComponent<PlayerControler>().darkColor = player1DarkColor;
                break;
            case 1:
                player.GetComponent<PlayerControler>().spawnpoint = player2Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player2SpawnRotation;
                player.GetComponent<PlayerControler>().litColor = player2LitColor;
                player.GetComponent<PlayerControler>().darkColor = player2DarkColor;
                break;
            case 2:
                player.GetComponent<PlayerControler>().spawnpoint = player3Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player2SpawnRotation;
                player.GetComponent<PlayerControler>().litColor = player3LitColor;
                player.GetComponent<PlayerControler>().darkColor = player3DarkColor;
                break;
            case 3:
                player.GetComponent<PlayerControler>().spawnpoint = player4Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player4SpawnRotation;
                player.GetComponent<PlayerControler>().litColor = player4LitColor;
                player.GetComponent<PlayerControler>().darkColor = player4DarkColor;
                break;
        }

    }

    public void OnPlayerLeft(PlayerInput player)
    {

        PlayerNo = PlayerNo - 1;
        Debug.Log("Player left");
        Debug.Log($"Players:{PlayerNo}");

    }
}
