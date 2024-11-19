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

    //GM: this will kick the first state...TIME TO BOOGALOO
    //WILL COMMENTED OUT CODE TO I CAN EDIT IT SO IT DOESN'T MESS WITH PUSHES N PULLS
    /* private void Start() -> changeState(GameStateManager newState)
     {
         State = newState;
         switch (newState)
         {
                case GameStateManager.STARTING;
                 HandleSTARTING(); 
                 break;
                case GameStateManager.SPAWNINGPlayers;
                 HandleSPAWNINGPlayers();
                 break;
                 case GameStateManager.SPAWNINGEnemies;
                 HandleSPAWNINGEnemies();
                 break;
                 case GameStateManager.KrakenATTACK;
                 HandleKrakenATTACK;
                 break;
                 case GameStateManager.shipAT50PERCENTHEALTH;
                 HandleshipAT50PERCENTHEALTH;
                 break;
                 case GameStateManager.shipAT25PERCENTHEALTH;
                 HandleshipAT25PERCENTHEALTH;
                 break;
                 case GameStateManager.shipAT50PERCENTHEALTH;
                 HandleshipAT50PERCENTHEALTH;
                 break;


         }

     }
     */



}
