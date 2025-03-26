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
        //If we have an item already return
        if (item) return false;            
       
        //buffer for colliders
        Collider[] colliders = new Collider[10];                         
        
        //check for items in front of the player
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
    public void DropItem(Vector3 direction, bool shouldLaunch = false)
    {
        if (item)  //if holding item?
        {
             item.DropItem(gameObject,shouldLaunch,direction); //drop the item
            //enable rigidbody and collider
        }

    }
    //get current item
    public bool AddItem(GameObject itemObj)
    {
        if (!itemObj.TryGetComponent(out Item newItem) || item != null) return false;
            
        //if (!inv.AddItem(item)) return; //add this to inv
        //disable the rigidbody and collider
        itemObj.GetComponent<Rigidbody>().isKinematic = true;
        itemObj.GetComponent<Collider>().enabled = false;
            //set the transforms
            itemObj.transform.SetParent(itemHolder, true);
        //Exactly what we want but just with the items data - MW
        itemObj.transform.localPosition = newItem.offset;
        itemObj.transform.localRotation = newItem.pickupRotation;

        this.item = newItem;
        return true;

    }

    public void RemoveItem()
    {
        //remove the current item
        
        //check if the item is pooled, if so return it
        if (item.CompareTag("Pooled"))
        {
            item.gameObject.SetActive(false);
            item = null;
            return;
        }
            
        
        
        //destroy the item
        Destroy(item.gameObject);
        item = null;
    }

    
    //used for Object pools (cannonBalls)
    public void Return()
    {
        item.gameObject.SetActive(false); 
        item = null;
    }

    //returns the current item
    public Item GetItem() => item;

    public void TakeItem(Inventory inv)
    {
        if(item == null) return;
        //take the current item, move it to the passed inventory and remove it from the current inventory
        inv.AddItem(item.gameObject);
        inv.GetItem().current = inv;
        
        item = null;
    }
}
