
using System;
using Core;
using UnityEngine;

public class CargoStack : ItemStack 
{
    
    
    private void Start()
    {
        //set layer
        gameObject.layer = LayerMask.NameToLayer("Cargo");
        
        pool = new GameObjectPool(itemGenerated, 10, transform); 
    }
}