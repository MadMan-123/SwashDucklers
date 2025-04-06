using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuController : MonoBehaviour
{
    public GameObject menuPanel;
    private int playerIndex;
    
    private PlayerInput playerInput;
    private string previousActionMap;
    public static bool menuOpen = false;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        menuPanel.SetActive(false);
        TryGetComponent(out playerInput);

    }
    
    public void OnToggleMenu(InputAction.CallbackContext context)
    {
        if (context.performed && !menuOpen)
        {
            menuOpen = true;
            //check if the 
            ToggleMenu();
        }
    }
    
    private void ToggleMenu()
    {
        //menuOpen = !menuOpen;
        menuPanel.SetActive(menuOpen);
        
        if (menuOpen)
        {
            previousActionMap = playerInput.currentActionMap.name;
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            menuOpen = false;
            playerInput.SwitchCurrentActionMap(previousActionMap);
        }
    }
    
    public bool CanInteractWithMenu(int menuOwnerIndex)
    {
        return playerIndex == menuOwnerIndex;
    }
    
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
