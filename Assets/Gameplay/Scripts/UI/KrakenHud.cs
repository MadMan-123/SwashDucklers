using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenHud : MonoBehaviour
{


    int startHealth = StageParameters.krakenHealth;

    [SerializeField] public GameObject Bomb;
    [SerializeField] public List<GameObject> BombList = null;

    [SerializeField] float rightPosition;
    [SerializeField] float leftPosition;

    private void OnEnable()
    {

        BombList.Clear();
        for (int i = 0; i < startHealth; i++)
        {

            float percentage = (((float)i) / (float)startHealth);

            //Debug.Log(Screen.height - 325);

            //Bomb positionX = furthest left position + (furthest right position * id of bomb/start health)
            float positionX = (leftPosition + (rightPosition * percentage));
            //Vector3 position = new Vector3(positionX, 0, 0);

            BombList.Add(Instantiate(Bomb, this.transform,false));

            //BombList[^1].transform.position = new Vector3(positionX * (Screen.width/1920f), BombList[^1].transform.position.y, BombList[BombList.Count - 1].transform.position.z);

            BombList[^1].GetComponent<RectTransform>().localPosition = new Vector3(positionX , 10, 0);
        }


       
    }

    private void OnDisable()
    {
 
        for (int i = 0; i < BombList.Count; i++)
        {
            //should we be destroying the object or just setting it to inactive?
            Destroy(BombList[i]);
        }
        BombList.Clear();

    }

    public void KrakenHit()
    {
        if (BombList[^1].TryGetComponent(out ImageAnimation imageAnimation)) { imageAnimation.activated = true;}
        BombList.Remove(BombList[^1]);
    }



}
