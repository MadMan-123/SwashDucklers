    using System.Collections.Generic;
    using UnityEngine;

    public static class GameData
    {
        public static List<GameObject> Players = new();
        public static List<GameObject> Enemies = new();
        public static int CargoCount = 5;
        public static int CargoMax = 5;
    }