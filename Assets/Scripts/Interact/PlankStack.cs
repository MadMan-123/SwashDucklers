using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankStack : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] GameObject itemGenerated;
    [SerializeField] GameObject[] item;
    public void PickUp(GameObject source, float time)
    {
        
        print($"{source.name} is picking up a plank");
        if (source.TryGetComponent(out Inventory inv))       //try to see if gameobject has an inv
        {
            if (inv.item != null) return; 
            GameObject p = Instantiate(itemGenerated);
            if (p.TryGetComponent(out Item item))
            {
                if (!inv.AddItem(item)) return; //add this to inv
                //disable the rigidbody and collider
                p.GetComponent<Rigidbody>().isKinematic = true;                           //bunch of positioning stuff
                p.GetComponent<BoxCollider>().enabled = false;

                //set the transforms
                p.transform.SetParent(inv.itemHolder, true);

                //Exactly what we want but just with the items data - MW
                p.transform.localPosition = item.offset;
                p.transform.localRotation = item.pickupRotation;
            }
        }
    }


    //Have a talk to madoc about an item manager / list type thing that holds names and objects and types that item.cs can take from and modularised plankstack
}
