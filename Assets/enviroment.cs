using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enviroment : MonoBehaviour
{
    #region objects

    public GameObject[] large; //used for objects that stop during kraken fight
    public GameObject[] medium; //used for normal speend moving objects
    public GameObject[] small; //used for slow non-stopping objects
    public GameObject[] skyunused; //used for  objects that go over the ship example birds

    #endregion



    #region variables

    public int SpeedLarge; //speed of objects
    public int SpeedMedium; //speed of objects    
    public int SpeedSmall;//speed of objects
    private float TimeTillLarge = 0; //delay between object spawns 
    private float TimeTillMedium = 0; //delay between object spawns 
    private float TimeTillSmall = 0; //delay between object spawns 
    
    public float initTimeTillLarge = 0; //delay between object spawns 
    public float initTimeTillMedium = 0; //delay between object spawns 
    public float initTimeTillSmall = 0; //delay between object spawns 
    
    public bool IsSpawner;
    public bool krakenactive;

    #endregion

 
    #region spawn locations
    public Transform LargeSpawn; //used for objects that stop during kraken fight
    public Transform MediumSpawn; //used for normal speend moving objects
    public Transform SmallSpawn; //used for slow non-stopping objects

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
        TimeTillMedium -= Time.deltaTime;
        TimeTillLarge -= Time.deltaTime;
    }

    
    void Islands()
    {
        if (IsSpawner)
        {
            if (TimeTillLarge <= 0)
            {
                int randomIndex = Random.Range(0, large.Length);
                GameObject instantiatedislands = Instantiate(large[randomIndex], LargeSpawn.position, Quaternion.identity) as GameObject;

                TimeTillLarge = initTimeTillLarge;
            }
        }
    }



    #region boats object script
    void Boats()
    {
        if (IsSpawner)
        {
            if (!krakenactive)
            if (TimeTillMedium <= 0)
            {
                int randomIndex = Random.Range(0, medium.Length);
                GameObject instantiatedsmall = Instantiate(medium[randomIndex], MediumSpawn.position, Quaternion.identity) as GameObject;

                TimeTillMedium = initTimeTillMedium;
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
                GameObject instantiatedsmall = Instantiate(small[randomIndex], SmallSpawn.position, Quaternion.identity) as GameObject;
                
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


