using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMenuElement : MonoBehaviour
{
    [SerializeField] public int menuOwnerIndex;
    private MenuManager menuManager;

    private void Awake()
    {
        TryGetComponent(out menuManager);
        gameObject.SetActive(false);
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
