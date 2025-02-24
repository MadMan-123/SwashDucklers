//using System;
//using UnityEngine;

//<summary>
///// GM: Manages the game states using an enum for readability and maintainability.
///// GM: Implements event-driven notifications for state changes and contains logic for handling individual states.
///// </summary>
//public class GameStateManager : Singleton<GameStateManager>
//{
//    public static GameStateManager Instance;

//    public GameState State;


//    public static event Action<GameState> GameStateChanged;
    

//    void Start()

//    {
//        UpdateGameState(GameState.Kraken);
//    }

//    // GM: Define possible game states as an enum for clear representation.
//        public enum GameState
//    {
//        none,
//        Kraken,
//        ShipAt50PercentHealth,
//        ShipAt25PercentHealth
//    }

//    // GM: Events to notify subscribers before and after state changes.
//    public static event Action<GameState> OnBEFOREStateChanged;
//    public static event Action<GameState> OnAFTERStateChanged;

//    // GM: Holds the current state of the game.
//    private GameState currentState = GameState.None;

//   // GM: Property to access the current state, with a private setter to control changes.
//    public GameState CurrentState
//    {
//        get => currentState;
//        private set
//        {
//            // GM: If the new state is the same as the current state, do nothing.
//            if (currentState == value) return;

//            // GM: Notify subscribers that the state is about to change.
//           OnBEFOREStateChanged?.Invoke(value);

//            // GM: Update the current state to the new value.
//            currentState = value;

//            // GM: Notify subscribers that the state has changed.
//            OnAFTERStateChanged?.Invoke(currentState);

//            // GM: Handle specific logic for the new state.
//            HandleStateChange(currentState);
//        }
//    }

//    // GM: Public method to change the game state.
//    public void ChangeState(GameState newState)
//    {
//        // GM: If the new state is the same as the current state, do nothing.
//        if (newState == CurrentState) return;

//        // GM: Update the state using the property, triggering associated logic.
//        CurrentState = newState;
//    }

//    // GM: Handles logic specific to the current game state.
//    private void HandleStateChange(GameState state)
//    {
//        switch (state)
//        {

//            case GameState.Kraken:
//                HandleKraken();
//                break;
//            case GameState.ShipAt50PercentHealth:
//                HandleShipAt50PercentHealth();
//                break;
//            case GameState.ShipAt25PercentHealth:
//                HandleShipAt25PercentHealth();
//                break;
//            default:
//                // GM: Log a warning if the state is unhandled.
//                Debug.LogWarning("Unhandled game state: " + state);
//                break;
//        }

//        OnGameStateChanged?.(newState); //GM: this chest to see if anyone has subscribed this function, in which case if not it won't be triggered.

//    }
//    /// <summary>
//    /// GM: Feel free to edit this at any time btw, this should be treated more as a template rather than a finished system :D
//    /// gm: Also I may have went in a bit heavy with the debug logs
//    /// </summary>

//    // GM: Example method for handling the "KrakenAttack" state. Replace with actual logic.
//    private void HandleKraken()
//    {
//        Debug.Log("Kraken is attacking!");
//        // GM: Add logic for Kraken attack here.
//    }

//    // GM: Example method for handling the "ShipAt50PercentHealth" state. Replace with actual logic.
//    private void HandleShipAt50PercentHealth()
//    {
//        Debug.Log("Ship health is at 50%.");
//        // GM: Add logic for the ship at 50% health scenario here.
//    }

//    // GM: Example method for handling the "ShipAt25PercentHealth" state. Replace with actual logic.
//    private void HandleShipAt25PercentHealth()
//    {
//        Debug.Log("Ship health is at 25%.");
//        // GM: Add logic for the ship at 25% health scenario here.
//    }
    
//    // GM: Called when this component is enabled.
//    private void OnEnable()
//    {
//        // GM: Log changes before and after state transitions for debugging purposes.
//        OnBEFOREStateChanged += state => Debug.Log($"BEFORE: Changing state to {state}");
//        OnAFTERStateChanged += state => Debug.Log($"AFTER: Changed state to {state}");
//    }

//    // GM: Called when this component is disabled.
//    private void OnDisable()
//    {
//        // GM: Unsubscribe from events to prevent memory leaks or errors.
//        OnBEFOREStateChanged -= state => Debug.Log($"BEFORE: Changing state to {state}");
//        OnAFTERStateChanged -= state => Debug.Log($"AFTER: Changed state to {state}");
//    }
//}
