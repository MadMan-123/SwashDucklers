using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float PlayerNo;

    // Start is called before the first frame update
    void Start()
    {
        PlayerNo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerJoined()
    {

        PlayerNo = PlayerNo + 1;
        Debug.Log("Player joined");
        Debug.Log($"Players:{PlayerNo}");

    }

    private void OnPlayerLeft()
    {

        PlayerNo = PlayerNo - 1;
        Debug.Log("Player left");
        Debug.Log($"Players:{PlayerNo}");

    }
}
