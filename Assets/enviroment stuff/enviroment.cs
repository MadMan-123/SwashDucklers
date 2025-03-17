using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enviroment : MonoBehaviour
{
    #region objects
    public GameObject[] large;
    public GameObject[] medium;
    public GameObject[] small;
    public GameObject[] kraken;


    public Kraken kracked;
    #endregion

    #region variables
    public int SpeedLarge;
    public int SpeedMedium;
    public int SpeedSmall;
    private float TimeTillLarge = 0;
    private float TimeTillMedium = 0;
    private float TimeTillSmall = 0;
    private float TimeTillkraken = 0;

    public float initTimeTillLarge = 0;
    public float initTimeTillMedium = 0;
    public float initTimeTillSmall = 0;
    public float initTimeTillkraken = 0;

    public bool IsSpawner;
    public bool krakenactive;
    #endregion

    #region spawn locations
    public Transform LargeSpawn;
    public Transform MediumSpawn;
    public Transform SmallSpawn;
    #endregion

    private Dictionary<GameObject, Vector3> spawnPositions = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        krakenactive = false;
        
    }

    private void Update()
    {
        MoveObjects(large, SpeedLarge);
        MoveObjects(medium, SpeedMedium);
        MoveObjects(small, SpeedSmall);
        MoveObjects(kraken, SpeedMedium);

        Large();
        Small();
        Medium();
        Kraken();

        TimeTillSmall -= Time.deltaTime;
        TimeTillMedium -= Time.deltaTime;
        TimeTillLarge -= Time.deltaTime;
        TimeTillkraken -= Time.deltaTime;
        if (kracked.isActiveAndEnabled)
        {
            krakenactive = true;
        }
        else if (!kracked.isActiveAndEnabled)
        {
            krakenactive = false;
        }
    }

    void MoveObjects(GameObject[] objects, float speed)
    {
        foreach (GameObject obj in objects)
        {
            if (obj.activeSelf)
            {
                
                float currentSpeed = speed;

                // Reduce speed when Kraken is active (adjust as needed)
                if (krakenactive && (objects == large || objects == small)) 
                {
                    currentSpeed *= 0f; // Slow down to 50% speed
                }

                obj.transform.position += Vector3.left * currentSpeed * Time.deltaTime;

                // Store spawn position when the object is first activated
                if (!spawnPositions.ContainsKey(obj))
                {
                    spawnPositions[obj] = obj.transform.position;
                }

                // Reset position if it moves too far
                if (Vector3.Distance(obj.transform.position, spawnPositions[obj]) > 200f)
                {
                    obj.SetActive(false);
                    obj.transform.position = spawnPositions[obj]; // Reset to spawn point
                }
            }
        }
    }

    void Large()
    {
        if (IsSpawner)
        {
            int randomIndex = Random.Range(0, large.Length);
            GameObject largeobj = large[randomIndex];

            if (TimeTillLarge <= 0 && !largeobj.activeSelf)
            {
                largeobj.SetActive(true);
                spawnPositions[largeobj] = LargeSpawn.position; // Store spawn position
                largeobj.transform.position = LargeSpawn.position;
                TimeTillLarge = initTimeTillLarge;
            }
        }
    }

    void Medium()
    {
        if (IsSpawner)
        {
            int randomIndex = Random.Range(0, medium.Length);
            GameObject mediumobj = medium[randomIndex];

            if (TimeTillMedium <= 0 && !mediumobj.activeSelf)
            {
                mediumobj.SetActive(true);
                spawnPositions[mediumobj] = MediumSpawn.position; // Store spawn position
                mediumobj.transform.position = MediumSpawn.position;
                TimeTillMedium = initTimeTillMedium;
            }
        }
    }

    void Small()
    {
        if (IsSpawner)
        {
            int randomIndex = Random.Range(0, small.Length);
            GameObject smallobj = small[randomIndex];

            if (TimeTillSmall <= 0 && !smallobj.activeSelf)
            {
                smallobj.SetActive(true);
                spawnPositions[smallobj] = SmallSpawn.position; // Store spawn position
                smallobj.transform.position = SmallSpawn.position;
                TimeTillSmall = initTimeTillSmall;
            }
        }
    }

    void Kraken()
    {
        if (krakenactive)
        {
            if (TimeTillkraken <= 0)
            {
                int randomIndex = Random.Range(0, kraken.Length);
                GameObject krakenObj = kraken[randomIndex];

                krakenObj.SetActive(true);
                spawnPositions[krakenObj] = krakenObj.transform.position; // Store spawn position
                TimeTillkraken = initTimeTillkraken;
            }
        }
    }
}