using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ItemStack : MonoBehaviour
{
    //delegate for the pickup event 
    protected Action<GameObject> OnPickUp;
    
    public Action<GameObject> OnDropOff;
    //the item that will be generated
    [SerializeField] public GameObject itemGenerated;
    
    //the pool that will be used to generate the items
    protected GameObjectPool pool;

    private void Start()
    {
        //initialize the pool
        pool = new GameObjectPool(itemGenerated, 10, transform); 
    }


    
    public virtual void TryPickUp(GameObject source)
    {
        //check if we have a source to work with
        Assert.IsNotNull(source);
            
        //ensure that the source has an inventory
        if (!source.TryGetComponent(out Inventory inv))
        {
            Debug.LogWarning("No inventory found on source");
            return; 
        }
        
        //check if the source already has an item
        if (inv.item != null)
        {
            //if a crab then drop, if player then return
            if (source.TryGetComponent(out AIBrain brain))
            {
                inv.DropItem(Vector3.zero,false);
            }
            else
            {
                return;
            }
        }
        
        //instantiate the item
        GameObject current = pool.GetObject();
        
        //add the item to the inventory
        inv.AddItem(current);
        SoundManager.PlayAudioClip("pick up", this.transform.position, 1f);
        OnPickUp?.Invoke(current);

    }
    
    
}
