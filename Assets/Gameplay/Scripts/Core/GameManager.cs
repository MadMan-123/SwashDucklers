using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paused;
    public bool gameOver;
    public GameObject g_over;
    // Start is called before the first frame update
    void Start()
    {
        paused=false;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");

            //g_over.SetActive(true);
            //Time.timeScale = 0f;
        }
    }
}
