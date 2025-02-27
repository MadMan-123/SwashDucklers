using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enviroment : MonoBehaviour
{
    #region objects

    public GameObject[] islands; //used for objects that stop during kraken fight
    public GameObject[] boats; //used for normal speend moving objects
    public GameObject[] small; //used for slow non-stopping objects
    public GameObject[] skyunused; //used for  objects that go over the ship example birds

    #endregion



    #region variables

    public int SpeedIsland; //speed of objects
    public int SpeedBoat; //speed of objects    
    public int SpeedSmall;//speed of objects
    private float TimeTillIsland = 0; //delay between object spawns 
    private float TimeTillBoat = 0; //delay between object spawns 
    private float TimeTillSmall = 0; //delay between object spawns 
    
    public float initTimeTillIsland = 0; //delay between object spawns 
    public float initTimeTillBoat = 0; //delay between object spawns 
    public float initTimeTillSmall = 0; //delay between object spawns 
    
    public bool IsSpawner;
    public bool krakenactive;

    #endregion

 
    #region spawn locations
    public Transform islandsSpawn; //used for objects that stop during kraken fight
    public Transform boatsSpawn; //used for normal speend moving objects
    public Transform smallSpawn; //used for slow non-stopping objects

    #endregion



    // Start is called before the first frame update
    private void Start()
    {
       
    }
    void Update()
    {  
        Islands();
        Small();
        Boats();
      
        
        TimeTillSmall -= Time.deltaTime;
        TimeTillBoat -= Time.deltaTime;
        TimeTillIsland -= Time.deltaTime;
    }

    
    void Islands()
    {
        if (IsSpawner)
        {
            if (TimeTillIsland <= 0)
            {
                int randomIndex = Random.Range(0, islands.Length);
                GameObject instantiatedislands = Instantiate(islands[randomIndex], islandsSpawn.position, Quaternion.identity) as GameObject;

                TimeTillIsland = initTimeTillIsland;
            }
        }
    }



    #region boats object script
    void Boats()
    {
        if (IsSpawner)
        {
            if (!krakenactive)
            if (TimeTillBoat <= 0)
            {
                int randomIndex = Random.Range(0, boats.Length);
                GameObject instantiatedsmall = Instantiate(boats[randomIndex], boatsSpawn.position, Quaternion.identity) as GameObject;

                TimeTillBoat = initTimeTillBoat;
            }
        }
    }
    #endregion


    #region small objects script
    void Small()
    {
        if (IsSpawner)
        {
            if (TimeTillSmall <= 0)
            {
                int randomIndex = Random.Range(0, small.Length);
                GameObject instantiatedsmall = Instantiate(small[randomIndex], smallSpawn.position, Quaternion.identity) as GameObject;
                
                TimeTillSmall = initTimeTillSmall;
            }
        }
    }
    #endregion

    #region destroy objects script

    void OnTriggerenter(Collider target)
    {
        if (!IsSpawner)
        {
            if (target.CompareTag("environment"))
            {
                Destroy(target.gameObject);
            }
        }
    }

    #endregion
}


