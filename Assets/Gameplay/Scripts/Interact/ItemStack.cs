using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemStack : MonoBehaviour
{
    [SerializeField] GameObject itemGenerated;
    private GameObjectPool pool;

    private void Start()
    {
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
        if (inv.item != null) return ;
        
        //instantiate the item
        GameObject current = pool.GetObject();
        
        //add the item to the inventory
        inv.AddItem(current);


    }
    
    
}
