using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bucketuse : MonoBehaviour
{
    public ShipHealth s_health;
    bool bucketfull;

    // Start is called before the first frame update
    void Start()
    {
        s_health = FindAnyObjectByType<ShipHealth>();
        bucketfull = false;
    }

    public void Bucketed()
    {
        if (!bucketfull)
        {
        // bucketfull = true;// sets the buckets state to full so no more water can be collected
         s_health.Bucket();//annouce to shiphealth script to lower ammount of shipfilled
        }
    }

   public void Bucketempty()
    {
     bucketfull = false;

    }
}

