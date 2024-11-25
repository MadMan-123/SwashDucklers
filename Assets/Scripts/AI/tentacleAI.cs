using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleAI : MonoBehaviour
{
    public GameObject[] tentacleObjects;
    int randomNumber;
    public bool krakenAlive;
    public float[] weightedSums;         //as many of these as locations

    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(0, tentacleObjects.Length);        //start with a random number
        for (int i = 0; i < weightedSums.Length; i++)
        {
            weightedSums[i] = Random.Range(0, 10);
        }
            PickTentacles(randomNumber);
    }
    private void FixedUpdate()
    {
        for(int i = 0; i < weightedSums.Length;i++)
        {
            weightedSums[i] += Time.deltaTime;
        }
    }
    public void PickTentacles(int number)
    {
        float totalSum = 0;
        for (int i = 0; i < weightedSums.Length; i++)
        {
            totalSum += weightedSums[i];                            //add them all together, random number using the sum as a cap. Depending on the number is what formation is used, might take top 3 and rand between them
        }
    }



    IEnumerator TentacleTimer(GameObject[] tentacles, int time, int waitTime)
    {
        foreach(GameObject tentacle in tentacles)
        {
            //tentacle.setActive(true);       //or instantiate
            //tentacle.anim.play              //lowkey forgot how to do animations plus they dont exist yet
        }
        yield return new WaitForSeconds(time);

        foreach(GameObject tentacle in tentacles)
        {
            //delete / set inactive
        }
        yield return new WaitForSeconds(waitTime);
    }
}
//weighted sums