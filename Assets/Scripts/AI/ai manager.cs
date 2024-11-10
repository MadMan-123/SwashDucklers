using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
public class AImanager : MonoBehaviour
{
    [SerializeField] EnemyShip e_ship;
    [SerializeField] GameObject enemy;

    [SerializeField] int currentwave;
    [SerializeField] int enemycount;
    [SerializeField] int maxenemycount = 10;
    [SerializeField] bool enemySpawn;
    [SerializeField] bool randomise;

    [SerializeField] float StartTimer;
    [SerializeField] float sec = 1;


    void Start()
    {
        e_ship = GetComponent<EnemyShip>();

        currentwave = Random.Range(1, 5);
        enemySpawn = false;

        //grace period before the first wave starts
        StartTimer = 20;
        

    }

    // Update is called once per frame
    void Update()
    { 
        StartTimer -= sec * Time.deltaTime;
        StartTimer = Mathf.Clamp(StartTimer, 0, 20);
        
        

        if (StartTimer <= 0)
        {
            StartCoroutine(Waverandomiser());
        }
    }
   


        IEnumerator Waverandomiser()
        {
     
         
            while (true)
            {
                // Get a random value
                currentwave = Random.Range(1, 5);
              yield return new WaitForSeconds(1);
            
            // start a new wave
                if (currentwave == 1)
                {
                    WaveShip();
                
            }

                if (currentwave == 2)
                {
                    WaveSquid();
               
            }


                yield return new WaitForSeconds(2);
            StopCoroutine(Waverandomiser());
            }

        }
    

      public void WaveShip()
        {
        enemySpawn = true;
     

        if (enemycount == maxenemycount )
        {
            enemySpawn = false;
        }

        var position = new Vector3(Random.Range(-12, -4), 5, Random.Range(-4, 6));

        if (enemySpawn)
        {
            Instantiate(enemy, position, transform.rotation);
            enemycount += 1;
        }
        
    

    }
       public void WaveSquid()
        {


        }

     public   void WaveEnd()
        {
            StartTimer = 20;
        }
    }

