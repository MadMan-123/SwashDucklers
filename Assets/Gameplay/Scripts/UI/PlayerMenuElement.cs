using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMenuElement : MonoBehaviour
{
    private GameObject playerObject;
    [SerializeField] public int menuOwnerIndex;
    private PlayerControler playerControl;
    private PlayerManager playerManager;
    //private MenuManager menuManager;

    private void Start()
    {
        //TryGetComponent(out menuManager);
        gameObject.SetActive(false);
        playerObject = transform.parent.gameObject;
        playerControl = playerObject.GetComponent<PlayerControler>();
        playerManager = playerControl.playerManager;
    }

    private void OnEnable()
    {
        PauseGame();
    }

    private void OnDisable()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        //if (PlayerStats.paused == false)
        //{
            // Pause the game
            Time.timeScale = 0;
            PlayerStats.paused = true;
            //unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        //}
    }

    public void ResumeGame()
    {
        //if (PlayerStats.paused)
        //{
            // Resume the game
            Time.timeScale = 1;
            PlayerStats.paused = false;
            //lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        //}
    }

    public void resumeButton()
    {
        gameObject.SetActive(false);
    }

    public void optionsButton()
    {

    }

    public void DropOutButton()
    {
        Destroy(playerObject);
    }

    public void ReturnToMenuButton()
    {
        MenuManager.instance.LoadScene(0);
    }

    private void OnDestroy()
    {
        Debug.Log("I was destroyed");
    }
}
