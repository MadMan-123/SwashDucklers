    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class TempGameData
    {
        public static int KrakenHealthIncrease;
        public static float CrabSpawnIncrease;
        public static float LeakSpawnIncrease;
        public static int CrabSpawnSize;

    }
    
    public class TempGameDataCarrier : MonoBehaviour
    {
        public EnemySpawner enemySpawner;
        public KrakenManager krakenManager;
        public static bool IsStart = true;
        public static int LevelCount = 0;
        
        private void Start()
        {
            
            LevelCount++;
            if (!IsStart)
            {
                //if the level count is divisible by 2, then can increase the kraken health
                if (LevelCount % 2 == 0)
                {
                    krakenManager.health.SetMaxHealth(krakenManager.health.GetMaxHealth() + TempGameData.KrakenHealthIncrease);
                    
                    //increase leak spawn rate
                    //get the task from the task manager
                    var task = TaskManager.TaskHashMap["Pump"];
                    //increase the spawn rate
                    task.taskSecondInterval -= TempGameData.LeakSpawnIncrease;
                    
                }
                enemySpawner.interval -= TempGameData.CrabSpawnIncrease;
                enemySpawner.maxSpawnSize += TempGameData.CrabSpawnSize;
                
                
            }
            
            
        }
    }