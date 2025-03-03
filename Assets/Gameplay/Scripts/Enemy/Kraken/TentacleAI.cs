using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAI : MonoBehaviour
{
    //This should control when the tentacles are spawned, disabling previous tentacle smoothly and other

    public float tentacleDeployedTime;
    public float waitBetweenTents;
    public GameObject[] tentacleObjects;
    private int lastCalled;

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (GameObject obj in tentacleObjects)
        {
            obj.SetActive(false);
        }
        TentacleAttack();
    }
    private void TentacleAttack()
    {
        int tentAttack = Random.Range(0, tentacleObjects.Length);
        if (tentAttack == lastCalled){tentAttack = Random.Range(0, tentacleObjects.Length);}
        lastCalled = tentAttack;
        StartCoroutine(TentacleTimer(tentAttack, tentacleDeployedTime, waitBetweenTents));
        Debug.Log("Coroutine Started");
    }

    IEnumerator TentacleTimer(int tentacle, float waitTime, float nextAttackTime)
    {
        Debug.Log("SetActive");
        tentacleObjects[tentacle].SetActive(true);
        yield return new WaitForSeconds(waitBetweenTents);
        Debug.Log("SetDeactive");
        tentacleObjects[tentacle].SetActive(false);
        yield return new WaitForSeconds(tentacleDeployedTime);
        TentacleAttack();
    }
}