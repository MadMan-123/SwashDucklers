using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankStack : MonoBehaviour
{
    private Inventory current;

    [SerializeField] public Item.Type type; //The type lives in Item - MW
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] Item plankItem;
    [SerializeField] GameObject plank;

    // Start is called before the first frame update
    public void PickUp(GameObject source, float time)
    {
        if (source.TryGetComponent(out Inventory inv))       //try to see if gameobject has an inv
        {
            GameObject p = Instantiate(plank);
            current = inv;
            if (p.TryGetComponent(out Item item))
            {
                if (!current.AddItem(item)) return; //add this to inv
                //disable the rigidbody and collider
                p.GetComponent<Rigidbody>().isKinematic = true;                           //bunch of positioning stuff
                p.GetComponent<BoxCollider>().enabled = false;
                           
                //set the transforms
                p.transform.SetParent(current.itemHolder, true);
            
                //Exactly what we want but just with the items data - MW
                p.transform.localPosition = item.offset;
                p.transform.localRotation = item.pickupRotation;
            }
            

        }
    }


    //in item.cs want to find out how to get the player thats interacting with this object while skipping almost all the steps



}
