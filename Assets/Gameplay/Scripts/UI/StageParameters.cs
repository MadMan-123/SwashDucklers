using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Struct for level data-SD
[Serializable]
public struct Level
{
    public string name;
    //TODO:Store Where the stage is kept in Memory so it can be easily accessed by name
}

//Enum for possible level lengths -SD
public enum Length
{
    Short,
    Medium,
    Long,
    Endless
}

//Contains all modifiable parameters for each stage -SD
public static class StageParameters
{
    //List containing all levels in the game -SD
    //TODO:This should be stored somewhere else
    public static List<Level> levelList { get; set; }

    //Current Level -SD
    public static Level currentLevel;

    //Length of level -SD
    public static Length levelLength = Length.Medium;

    //Kraken enabled -SD
    public static bool krakenEnabled = true;

    //Kraken Health
    public static int krakenHealth = 5;

    //Starting cargo
    public static int startingCargo = 5;

    //Current cargo
    public static int currentCargo = 5;

    //Score
    public static int Score = 0;

    //Max Ship health
    public static float maxShipHealth = 100;

    //current Ship health
    public static float currentShipHealth = 0;
}
