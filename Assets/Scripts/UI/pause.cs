using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausemenu : MonoBehaviour
{
    public GameObject pause;
    public GameObject Gameplay;
    public GameObject settings;
    public GameObject controls;
    public bool paused;
    public bool PauseDaGame;
    // Start is called before the first frame update
    void Start()
    {
      ResumeButton();
        Time.timeScale = 1f;
        paused = false;
    }

    public void Update()
    {
        PauseDaGame = Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Joystick1Button9);

        if (PauseDaGame && !paused)
        {
            pauseButton();
            paused = true;
        }

 
    }

    public void MainMenuButton()
    {
        // go back to main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu test");
       
    }

    public void pauseButton()
    {
        //set to pause screen
        Gameplay.SetActive(false);
        pause.SetActive(true);
        Time.timeScale = 0f;

    }

    public void settingsButton()
    {
        //set to settings screen
        pause.SetActive(false);
        settings.SetActive(true);
      
    }

    public void backButton()
    {
        //set back to pause screen
        pause.SetActive(true);
        controls.SetActive(false);
        settings.SetActive(false);

    }
    public void controlsButton()
    {
        //set to controls screen
        controls.SetActive(true);
        settings.SetActive(false);

    }

    public void ResumeButton()
    {
        //set to gameplay screen
        Gameplay.SetActive(true);
        pause.SetActive(false);
        settings.SetActive(false);
        controls.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
}