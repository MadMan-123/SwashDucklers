using System.Collections;
using UnityEngine;

public class waterinteract : MonoBehaviour
{
    public bucketuse w_bucket;
    public GameObject ship;

    public void Start()
    {
        ship = GameObject.FindWithTag("Boat");
        w_bucket = FindAnyObjectByType<bucketuse>();
        ship.GetComponent<ShipHealth>().DamageShip(5);
    }

    public void Startdrain(GameObject source,float time)
    {
        IEnumerator pop = drain(source,time);
        StartCoroutine(pop);
    }

    IEnumerator drain(GameObject player, float sec)
    {
        w_bucket.Bucketed();//anounces to the bucket use script that its picking up water
        yield return new WaitForSeconds(sec);
    }

}
