using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paused;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        paused=false;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
