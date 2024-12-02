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
}

public static class PlayerStats
{

    //Number of players
    public static int playerNo { get; set; }

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

}
