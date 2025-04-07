using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[Serializable]
public struct Hat
{
    public string name;
    public GameObject model;
    public Vector3 position;
    public Vector3 previewPosition;
}

[Serializable]
public enum DuckColors
{
    Yellow,
    Orange,
    Red,
    Green,
    Pink,
    White,
    Blue,
    Cyan,
    Purple
}

public static class PlayerStats
{

    //Number of players
    public static int playerNo { get; set; }

    //Number of ready players
    public static int readyPlayers { get; set; }

    //Player Colors
    public static Color player1Color { get; set; } = Color.yellow;
    public static Color player2Color { get; set; } = Color.blue;
    public static Color player3Color { get; set; } = Color.green;
    public static Color player4Color { get; set; } = Color.white;

    //Player input devices
    public static InputDevice player1input { get; set; }
    public static InputDevice player2input { get; set; }
    public static InputDevice player3input { get; set; }
    public static InputDevice player4input { get; set; }

    //Player Hats
    public static int player1Hat { get; set; } = 0;
    public static int player2Hat { get; set; } = 0;
    public static int player3Hat { get; set; } = 0;
    public static int player4Hat { get; set; } = 0;

    //List of Hats
    public static List<Hat> Hatlist { get; set; }


    //Fixed players
    //This is sort of thrown together but im using it to ensure players cant overlap with each other
    public static bool yellowTaken = false;
    public static bool orangeTaken = false;
    public static bool redTaken = false;
    public static bool greenTaken = false;
    public static bool pinkTaken = false;
    public static bool whiteTaken = false;
    public static bool blueTaken = false;
    public static bool cyanTaken = false;
    public static bool purpleTaken = false;

    public static DuckColors player1ColorName { get; set; } = DuckColors.Yellow;
    public static DuckColors player2ColorName { get; set; } = DuckColors.Orange;
    public static DuckColors player3ColorName { get; set; } = DuckColors.Cyan;
    public static DuckColors player4ColorName { get; set; } = DuckColors.Blue;
}
