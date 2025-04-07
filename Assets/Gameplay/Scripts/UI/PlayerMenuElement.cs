using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMenuElement : MonoBehaviour
{
    [SerializeField] public int menuOwnerIndex;
    //private MenuManager menuManager;

    private void Awake()
    {
        //TryGetComponent(out menuManager);
        gameObject.SetActive(false);
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
        // Pause the game
        Time.timeScale = 0;

        //unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1;

        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
