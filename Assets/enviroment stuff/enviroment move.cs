using System;
using UnityEngine;

public class Enviromentmove : MonoBehaviour
{
    public bool ismedium;
    public bool islarge;
    public bool issmall;

    private int mediumspd;
    private int largespd;
    private int smallspd;
    enviroment getspeed;

    private Transform small;
    private Transform medium;
    private Transform large;
 
    private float x;

    private float disableDistance = 200;
    
    // Start is called before the first frame update
    void Start()
    {  
        
        
        
        
        //getspeed = GameObject.FindWithTag("environment").GetComponent<enviroment>();
        getspeed = GameObject.FindObjectOfType<enviroment>();
        large = getspeed.LargeSpawn;
        small = getspeed.SmallSpawn;
        medium = getspeed.MediumSpawn;
        
        if (issmall)
        {
            transform.position = small.position;
        }
        if (ismedium)
        {
            transform.position = medium.position;
        }
        if (islarge)
        {
            transform.position = large.position;
        }
       
        
    }



    // Update is called once per frame
   public void Update()
    {
       
       
        
        if (issmall)
        {
            if (!getspeed.krakenactive)
            {
                smallspd = getspeed.SpeedSmall;
            }
            else if (getspeed.krakenactive)
            {
                smallspd = 0;
            }
            
            var move =  Time.deltaTime * smallspd;
            transform.Translate(new Vector3(0,0,move));
            
            float distance = Vector3.Distance(transform.position, getspeed.SmallSpawn.transform.position);

            if (distance > disableDistance )
            {
                gameObject.SetActive(false); 
            }
            if (!gameObject.activeSelf)
            {
                transform.position = small.position;
            }
        }
        
        else
        if (islarge)
        {
            
            if (!getspeed.krakenactive)
            {
                largespd = getspeed.SpeedLarge;
            }
            else if (getspeed.krakenactive)
            {
                largespd = 0;
            }
            
            var move =  Time.deltaTime * largespd;
            transform.Translate(new Vector3(0,0,move));
            
            float distance = Vector3.Distance(transform.position, getspeed.LargeSpawn.transform.position);

            if (distance > disableDistance)
            {
                gameObject.SetActive(false); 
            }
            if (!gameObject.activeSelf)
            {
                transform.position = large.position;
            }
        }
        else
        if (ismedium)
        {

            mediumspd = getspeed.SpeedMedium;
            var move =  Time.deltaTime * mediumspd;
            transform.Translate(new Vector3(0,0,move));
            
            float distance = Vector3.Distance(transform.position, getspeed.SmallSpawn.transform.position);

            if (distance > disableDistance)
            {
                gameObject.SetActive(false); 
            }
            
            if (!gameObject.activeSelf)
            {
                transform.position = medium.position;
            }
        }
    }
}
