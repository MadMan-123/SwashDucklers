using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveandcrabmanager : MonoBehaviour
{
    public Crabcontroller c_con;
    public int currentwave;
    public GameObject[] planks;

    public int target;

    // Start is called before the first frame update
    void Start()
    {
       planks =  GameObject.FindGameObjectsWithTag("HoleLocate");
    }

    // Update is called once per frame
    void Update()
    {
       
        currentwave = Random.Range(1, 5);
        target = Random.Range(0,planks.Length);

    }
}
