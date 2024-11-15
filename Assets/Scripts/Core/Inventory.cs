using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public Transform itemHolder;

    //Change to Item Class?
    [SerializeField] public Item item;
    
    //pickup
    
    public bool TryPickUp()
    {
        if (item) return false;            //if it is an item / item isnt being held?
        
        Collider[] colliders = new Collider[10];                         //array incase multiple items are tried to be picked up?
        int count = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * 0.5f , 1f, colliders);         //sphere area stuff
        for (int i = 0; i < count; i++)                        
        {
            if (colliders[i].TryGetComponent(out Item item)  )
            {
                item.PickUp(gameObject);
                return true;
            }
        }

        return false;
    }
    //drop
    public void DropItem()
    {
        if (item)  //if holding item?
        {
            item.DropItem(gameObject);
        }
    }
    //get current item
    public bool AddItem(Item newItem)
    {

        item = newItem;            //assigned item to the new item
        return item;
    }

    public void RemoveItem()
    {
        //remove the current item
        //destroy the item
        Destroy(item.gameObject);
        item = null;
    }

    public void Return()
    {
        item.gameObject.SetActive(false);             //dunno what this is for will report back, used in interactArea
    }

    public Item GetItem() => item;             //return item?
}
