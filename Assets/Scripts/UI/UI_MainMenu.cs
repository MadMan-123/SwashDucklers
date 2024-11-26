using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_MainMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject CreditsMenu;

    // set which screen is shown
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
    }

    public void RETURNTOMENU()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Character Select");
    }

    public void CreditsButton()
    {
        // Show Credits Menu(unused)
        MainMenu.SetActive(false);
  
   
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
       
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}