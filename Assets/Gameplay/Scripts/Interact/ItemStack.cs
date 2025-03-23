using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemStack : MonoBehaviour
{
    //delegate for the pickup event 
    protected Action<GameObject> OnPickUp;
    //the item that will be generated
    [SerializeField] public GameObject itemGenerated;
    
    //the pool that will be used to generate the items
    protected GameObjectPool pool;

    private void Start()
    {
        //initialize the pool
       pool = new GameObjectPool(itemGenerated, 10, transform); 
    }

    public void TryPickUp(GameObject source)
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
        if (inv.item != null) inv.RemoveItem();
        
        //instantiate the item
        GameObject current = pool.GetObject();
        
        //add the item to the inventory
        inv.AddItem(current);
        
        OnPickUp?.Invoke(current);

    }
    
    
}
