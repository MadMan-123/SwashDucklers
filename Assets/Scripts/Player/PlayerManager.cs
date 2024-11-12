using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
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

    [SerializeField] List<GameObject> players = new();
    [SerializeField] private string areaName;
    public int PlayerNo; //i swear to god if i see another counting variable set as a float and not an int 

    [SerializeField] private float debugRadius = 0.45f;

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
        PlayerNo++;
        players.Add(player.gameObject);
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

#if UNITY_Editor
    private void OnDrawGizmos()
    {
        //draw each position
        Gizmos.color = Color.cyan;
        Handles.Label(player1Spawn + Vector3.up,"Player 1 Spawn");
        Gizmos.DrawSphere(player1Spawn, debugRadius);
        Handles.Label(player2Spawn + Vector3.up,"Player 2 Spawn");
        Gizmos.DrawSphere(player2Spawn, debugRadius);
        Handles.Label(player3Spawn + Vector3.up,"Player 3 Spawn");
        Gizmos.DrawSphere(player3Spawn, debugRadius);
        Handles.Label(player4Spawn + Vector3.up,"Player 4 Spawn");
        Gizmos.DrawSphere(player4Spawn, debugRadius);

    }
#endif
}
