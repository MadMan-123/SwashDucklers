using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleAI : MonoBehaviour
{
    public float tentacleDeployedTime;
    public float waitBetweenTents;
    public GameObject[] tentacleObjects;
    int randomNumber;
    public float[] weightedSums;         //as many of these as locations
    public bool krakenAlive;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tentacleObjects.Length; i++)
        {
            tentacleObjects[i].SetActive(false);
        }
        weightedSums = new float[tentacleObjects.Length];
        randomNumber = Random.Range(0, tentacleObjects.Length);        //start with a random number
        TentacleAttack(randomNumber);
    }
    private void FixedUpdate()
    {
        for(int i = 0; i < weightedSums.Length;i++)
        {
            weightedSums[i] += Time.deltaTime;                        //constantly increases the weighted sums
        }
    }
    private void TentacleAttack()
    {
        int amount=0;
        int rndAmount = Random.Range(0, 9);
        amount = 0;

        //if (rndAmount <= 4) amount = 1;                 //4 in 10 chance for 1 tent
        //else if (rndAmount <= 8 && rndAmount >4) amount = 2;            //5 in 10 chance for 2 tent
        //else if (rndAmount == 9)amount = 3;    //1 in 10 chance for 3 tent

        int[] tentsToCall = new int[amount];

        for (int i = 0; i < amount; i++) 
        {
            bool dupeCheck = false;
            int tentAttack = PickTentacles();

            for (int j = 0; j < tentsToCall.Length; j++)                          //checks all values of tentsCalled to see if theres any dupes
            {
                if (tentAttack == tentsToCall[j]) dupeCheck = true;
            }
            if (!dupeCheck)
            {
                tentsToCall[i] = tentAttack;
                weightedSums[tentAttack] = 0;
            }
        }
        StartCoroutine(TentacleTimer(tentsToCall, tentacleDeployedTime, waitBetweenTents));
    }
    private void TentacleAttack(int attack)
    {
        StartCoroutine(TentacleTimer(attack, tentacleDeployedTime, waitBetweenTents));      
    }

    int PickTentacles()                                     //this adds all the weighted sums together, runs a random with the upper limit being the sum, the number gend is then used and where it falls, that attack is called
    {
        int attackSelected = 0;
        float totalSum = 0;
        for (int i = 0; i < weightedSums.Length; i++)
        {
            totalSum += weightedSums[i];                            //add them all together, random number using the sum as a cap, depending on the number is what formation is used. might take top 3 and rand between them
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


    IEnumerator TentacleTimer(int tentacle, float waitTime, float nextAttackTime)
    {
        tentacleObjects[tentacle].SetActive(true);
        yield return new WaitForSeconds(waitTime);

        tentacleObjects[tentacle].SetActive(false);
        yield return new WaitForSeconds(nextAttackTime);

        TentacleAttack();
    }
    IEnumerator TentacleTimer(int[] tentacle, float waitTime, float nextAttackTime)
    {
        for (int i = 0; i < tentacle.Length; ++i)
        {
            tentacleObjects[tentacle[i]].SetActive(true);
        }
        //tentacle.anim.play
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < tentacle.Length; ++i)
        {
            tentacleObjects[tentacle[i]].SetActive(false);
        }
        yield return new WaitForSeconds(nextAttackTime);

        TentacleAttack();
    }
}
//weighted sums