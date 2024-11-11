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
        if (item) return false;
        
        Collider[] colliders = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * 0.5f , 1f, colliders);
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
        if (item)
        {
            item.DropItem(gameObject);
        }
    }
    //get current item
    
    //Mb just realised apologies for having taste :/
    public bool AddItem(Item newItem)
    {
        item = newItem;
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
        item.gameObject.SetActive(false);
    }

    public Item GetItem() => item;
}
