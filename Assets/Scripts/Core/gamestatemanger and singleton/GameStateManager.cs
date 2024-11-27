using System;
using UnityEngine;

/// <summary>
/// GameStateManager: Manages the game states using enums for readability and maintainability.
/// Inherits from a Singleton class to ensure a single instance.
/// </summary>
/*public class GameStateManager : Singleton<GameStateManager>
{
    // Define the possible game states as an enum for clarity.
    public enum GameState
    {
        None,
        Starting,
        SpawningPlayers,
        SpawningEnemies,
        KrakenAttack,
        ShipAt50PercentHealth,
        ShipAt25PercentHealth
    }

    // Events for notifying before and after state changes.
    public static event Action<GameState> OnBEFOREStateChanged;
    public static event Action<GameState> OnAFTERStateChanged;

    // The current state of the game.
    private GameState currentState = GameState.None;

    public GameState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState == value) return;

            // Notify listeners before the state changes.
            OnBEFOREStateChanged?.Invoke(value);

            // Update the state.
            currentState = value;

            // Notify listeners after the state changes.
            OnAFTERStateChanged?.Invoke(currentState);

            // Handle state-specific logic.
            HandleStateChange(currentState);
        }
    }

    // Public method to change the game state.
    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState) return;
        CurrentState = newState;
    }

    // Handles logic specific to each state.
    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningPlayers:
                HandleSpawningPlayers();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.KrakenAttack:
                HandleKrakenAttack();
                break;
            case GameState.ShipAt50PercentHealth:
                HandleShipAt50PercentHealth();
                break;
            case GameState.ShipAt25PercentHealth:
                HandleShipAt25PercentHealth();
                break;
            default:
                Debug.LogWarning("Unhandled game state: " + state);
                break;
        }
    }

    // Example methods for each game state. Replace with your actual logic.
    private void HandleStarting()
    {
        Debug.Log("Game is starting...");
        // Add starting logic here.
    }

    private void HandleSpawningPlayers()
    {
        Debug.Log("Spawning players...");
        // Add player spawning logic here.
    }

    private void HandleSpawningEnemies()
    {
        Debug.Log("Spawning enemies...");
        // Add enemy spawning logic here.
    }

    private void HandleKrakenAttack()
    {
        Debug.Log("Kraken is attacking!");
        // Add Kraken attack logic here.
    }

    private void HandleShipAt50PercentHealth()
    {
        Debug.Log("Ship health is at 50%.");
        // Add logic for 50% health scenario.
    }

    private void HandleShipAt25PercentHealth()
    {
        Debug.Log("Ship health is at 25%.");
        // Add logic for 25% health scenario.
    }

    // Optional: For debugging purposes.
    private void OnEnable()
    {
        OnBEFOREStateChanged += state => Debug.Log($"BEFORE: Changing state to {state}");
        OnAFTERStateChanged += state => Debug.Log($"AFTER: Changed state to {state}");
    }

    private void OnDisable()
    {
        OnBEFOREStateChanged -= state => Debug.Log($"BEFORE: Changing state to {state}");
        OnAFTERStateChanged -= state => Debug.Log($"AFTER: Changed state to {state}");
    }
}
*/