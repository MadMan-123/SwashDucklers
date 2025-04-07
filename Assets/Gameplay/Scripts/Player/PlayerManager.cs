using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.InputSystem.LowLevel;

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

    [SerializeField] private CinemachineTargetGroup cameraTarget;
    [SerializeField] private float playerCameraWeight;
    [SerializeField] private float playerCameraRadius;

    //[SerializeField] private GameObject pauseMenuHolder;
    //[SerializeField] private GameObject menuPrefab;

    //private GameObject[] playersPauseMenus = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void Awake()
    {
        //clear the game data's player list
        GameData.Players.Clear();
        
        Debug.Log($"Players on spawn: {PlayerStats.playerNo}");

        //Spawn joined players
        for (int i = 0; i < PlayerStats.playerNo; i++)
        {

            var index = i - 1;
            if (index < 0)
            {
                index = 0;
            }
            var currentInputState = index switch
            {
                0 => PlayerStats.player1input,
                1 => PlayerStats.player2input,
                2 => PlayerStats.player3input,
                3 => PlayerStats.player4input,
                _ => throw new ArgumentOutOfRangeException()
            };

            
            PlayerInput.Instantiate(playerPrefab, i, null, -1, currentInputState);
      
        }
    }


    public void OnPlayerJoined(PlayerInput player)
    {
        //PlayerStats.playerNo++;
        players.Add(player.gameObject);
        GameData.Players.Add(player.gameObject);
        Debug.Log("Player joined");
        Debug.Log($"Players:{PlayerStats.playerNo}");
        
        print(player.name + "JOINED");
        
        if(player.TryGetComponent(out PlayerControler pc))
        {
            
            
            pc.playerCameraWeight = playerCameraWeight;
            pc.playerCameraRadius = playerCameraRadius;
        }
        else
        {
            print("PlayerControler not found on " + player.name);
        }
        //cameraTarget.AddMember(player.transform, 3, 2.5f);
        var spawn = player.playerIndex switch
        {
            0 => player1Spawn,
            1 => player2Spawn,
            2 => player3Spawn,
            3 => player4Spawn,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var rotation = player.playerIndex switch
        {
            0 => player1SpawnRotation,
            1 => player2SpawnRotation,
            2 => player3SpawnRotation,
            3 => player4SpawnRotation,
            _ => throw new ArgumentOutOfRangeException()
        };
        //playersPauseMenus[player.playerIndex] = Instantiate(menuPrefab);
        //playersPauseMenus[player.playerIndex].SetActive(false);
        //playersPauseMenus[player.playerIndex].GetComponent<PlayerMenuElement>().menuOwnerIndex = player.playerIndex;
        //var pmc = player.GetComponent<PlayerMenuController>();
        //pmc.menuPanel = playersPauseMenus[player.playerIndex];
        pc.spawnpoint = spawn;
        pc.spawnRotation = rotation;
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        //PlayerStats.playerNo = PlayerStats.playerNo - 1;
        Debug.Log("Player left");
        Debug.Log($"Players:{PlayerStats.playerNo}");

        cameraTarget.RemoveMember(player.transform);

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
