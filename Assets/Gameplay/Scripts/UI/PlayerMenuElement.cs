using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMenuElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int menuOwnerIndex;
    private MenuManager menuManager;

    private void Awake()
    {
        TryGetComponent(out menuManager);
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput playerInput = eventData.pressEventCamera.GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            int playerIndex = playerInput.playerIndex;

            if (PlayerMenuController.IsPlayerAllowedToInteract(playerIndex, menuOwnerIndex))
            {
                HandleUIAction();
            }
        }
    }

    private void HandleUIAction()
    {
        Debug.Log("UI action performed by authorized player");
    }

    private void OnEnable()
    {
        menuManager.PauseGame();
    }

    private void OnDisable()
    {
        menuManager.ResumeGame();
    }
}
