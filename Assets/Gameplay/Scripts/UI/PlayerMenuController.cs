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
    private bool menuOpen = false;
    
    private static Dictionary<int,bool> activeMenus = new Dictionary<int, bool>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        menuPanel.SetActive(false);
        TryGetComponent(out playerInput);

        activeMenus.TryAdd(playerIndex, false);
    }
    
    public void OnToggleMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleMenu();
        }
    }
    
    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
        menuPanel.SetActive(menuOpen);
        activeMenus[playerIndex] = menuOpen;
        
        if (menuOpen)
        {
            previousActionMap = playerInput.currentActionMap.name;
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            playerInput.SwitchCurrentActionMap(previousActionMap);
        }
    }
    
    public bool CanInteractWithMenu(int menuOwnerIndex)
    {
        return playerIndex == menuOwnerIndex;
    }
    
    public static bool IsPlayerAllowedToInteract(int playerIndex, int menuOwnerIndex)
    {
        return playerIndex == menuOwnerIndex && activeMenus.ContainsKey(menuOwnerIndex) && activeMenus[menuOwnerIndex];
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
