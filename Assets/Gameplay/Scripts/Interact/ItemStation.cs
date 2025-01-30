using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStation : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    public ItemManager.Type type;
    GameObject itemGenerated;
    ItemManager iM;

    private void Start()
    {
        iM = (GameObject.Find("[ItemManager]")).GetComponent<ItemManager>();
            itemGenerated = iM.itemList[Convert.ToInt32(type)];
    }

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
                p.GetComponent<Collider>().enabled = false;

                //set the transforms
                p.transform.SetParent(inv.itemHolder, true);

                //Exactly what we want but just with the items data - MW
                p.transform.localPosition = item.offset;
                p.transform.localRotation = item.pickupRotation;
            }
        }
    }
}
