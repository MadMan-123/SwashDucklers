using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenHud : MonoBehaviour
{


    int startHealth = StageParameters.krakenHealth;
    [SerializeField] public int currentHealth;

    [SerializeField] public GameObject Bomb;
    [SerializeField] public List<GameObject> BombList = null;

    [SerializeField] float rightPosition;
    [SerializeField] float leftPosition;

    private void OnEnable()
    {
        currentHealth = startHealth;

        BombList.Clear();
        for (int i = 0; i < startHealth; i++)
        {

            float percentage = (((float)i) / (float)startHealth);

            //Debug.Log(Screen.height - 325);

            //Bomb positionX = furthest left position + (furthest right position * id of bomb/start health)
            float positionX = leftPosition + (rightPosition * percentage);
            Vector3 position = new Vector3(positionX, 982, 0);

            BombList.Add(Instantiate(Bomb, position, Quaternion.identity, this.transform));
        }


       
    }

    private void OnDisable()
    {
 
        for (int i = 0; i < BombList.Count; i++)
        {
            Destroy(BombList[i]);
        }
        BombList.Clear();

    }

    public void KrakenHit()
    {
        if (currentHealth == 0)
        {
            //Kraken is dead
        }
        else
        {
            currentHealth = currentHealth - 1;

            Destroy(BombList[BombList.Count - 1]);
            BombList.Remove(BombList[BombList.Count - 1]);

        }
    }
}
