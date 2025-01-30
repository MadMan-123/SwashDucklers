using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemList;


    public enum Type
    {
        NoItem = -1,
        CannonBall,
        Plank
        //other item types here
    }


    //So far this is just to hold an array for prefabs if we ever need them but we could put more functionality in later if we need to
}
