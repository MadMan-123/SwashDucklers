using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuButton : MonoBehaviour, IPointerClickHandler
{
    private MenuManager menuManager;
    private PlayerMenuElement menuElement;

    [SerializeField] private ButtonType bT;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GetComponentInParent<MenuManager>();
        menuElement = GetComponentInParent<PlayerMenuElement>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput playerInput = eventData.pressEventCamera.GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            int playerIndex = playerInput.playerIndex;
            if (playerIndex == menuElement.menuOwnerIndex)
            {
                switch(bT)
                {
                    case ButtonType.Resume:
                        Resume();    
                    break;
                    case ButtonType.Options:
                        Options();
                        break;
                    case ButtonType.DropOut:
                        DropOut();
                        break;
                }
            }
        }
    }

    private void Resume()
    {
        print("Resi,");
    }

    private void Options()
    {
        print("opt");
    }

    private void DropOut()
    {
        print("DropOut");
    }
    enum ButtonType
    {
        Resume,
        Options,
        DropOut,
        ReturnToMenu,
    }
}
