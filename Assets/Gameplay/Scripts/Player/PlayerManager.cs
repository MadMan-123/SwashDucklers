using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Vector3 player1Spawn;
    [SerializeField] private Vector3 player2Spawn;
    [SerializeField] private Vector3 player3Spawn;
    [SerializeField] private Vector3 player4Spawn;

    [SerializeField] private Vector3 player1SpawnRotation;
    [SerializeField] private Vector3 player2SpawnRotation;
    [SerializeField] private Vector3 player3SpawnRotation;
    [SerializeField] private Vector3 player4SpawnRotation;

    private Input Input;

    [SerializeField] public List<GameObject> players = new();
    [SerializeField] private string areaName;

    [SerializeField] private float debugRadius = 0.45f;
    [SerializeField] private bool shouldDebug = false;

    [SerializeField] private GameObject target; 

    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void Awake()
    {

        Debug.Log($"Players on spawn: {PlayerStats.playerNo}");

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

    }

    public void OnPlayerJoined(PlayerInput player)
    {
        //PlayerStats.playerNo++;
        players.Add(player.gameObject);
        Debug.Log("Player joined");
        Debug.Log($"Players:{PlayerStats.playerNo}");

        //target.GetComponent<CinemachineTargetGroup>();

        switch (player.playerIndex)
        {
            case 0:
               player.GetComponent<PlayerControler>().spawnpoint = player1Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player1SpawnRotation;
                break;
            case 1:
                player.GetComponent<PlayerControler>().spawnpoint = player2Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player2SpawnRotation;
                break;
            case 2:
                player.GetComponent<PlayerControler>().spawnpoint = player3Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player2SpawnRotation;
                break;
            case 3:
                player.GetComponent<PlayerControler>().spawnpoint = player4Spawn;
                player.GetComponent<PlayerControler>().spawnRotation = player4SpawnRotation;
                break;
        }

    }

    public void OnPlayerLeft(PlayerInput player)
    {
        //PlayerStats.playerNo = PlayerStats.playerNo - 1;
        Debug.Log("Player left");
        Debug.Log($"Players:{PlayerStats.playerNo}");

    }

#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if(!shouldDebug) return;
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
