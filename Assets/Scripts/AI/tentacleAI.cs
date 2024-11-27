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
            TentacleAttack();
    }
    private void FixedUpdate()
    {
        for(int i = 0; i < weightedSums.Length;i++)
        {
            weightedSums[i] += Time.deltaTime;
        }
    }
    private void TentacleAttack()
    {
        int amount=0;
        int tempRndHold = Random.Range(0, 10);
        if (tempRndHold <= 4) amount = 1;
        else if (tempRndHold <= 9) amount = 2;
        else amount = 3;

    }

    public int PickTentacles(int number)
    {
        int attackSelected = 0;
        int numberOfTentacles = Random.Range(1, 3);
        float totalSum = 0;
        for (int i = 0; i < weightedSums.Length; i++)
        {
            totalSum += weightedSums[i];                            //add them all together, random number using the sum as a cap. Depending on the number is what formation is used, might take top 3 and rand between them
        }
        float ranNumber = Random.Range(0, totalSum);
        float count = 0;
        for(int i = 0;i < weightedSums.Length;i++)
        {
            float temp = count;
            count += weightedSums[i];
            if(ranNumber > temp && ranNumber < count)
            {
                attackSelected = i;
            }
        }
        return attackSelected;
    }



    IEnumerator TentacleTimer(GameObject tentacle, int time, int waitTime)
    {
        tentacle.SetActive(true);
        //tentacle.anim.play
        yield return new WaitForSeconds(time);
        tentacle.SetActive(false);
    }
}
//weighted sums