using System;
using UnityEngine;




/// <summary>
/// GM: this game manager will be enum based, this will help us read the states easy as well as manage them.
/// </summary>
public class GameStateManager : Singleton<GameStateManager>  //GM: this will inherit from the singleton class
{
    public static event Action<GameStateManager> OnBEFOREStateChanged;
    public static event Action<GameStateManager> OnAFTERStateChanged;

   // public GameStateManager { get; private set; } GM: WILL COME BACK TO THIS BIT LATER

}
