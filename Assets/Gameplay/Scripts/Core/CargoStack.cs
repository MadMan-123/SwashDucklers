
using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CargoStack : ItemStack
{
    
    //reference to the text ui
    [SerializeField] public TextMeshProUGUI cargoText;
    //score to decrease
    public int score = 10;
    
     
    
    private void Start()
    {
        GameData.CargoCount = 5;
        //subscribe to the event
        OnPickUp += UpdateCargo;
        
        //set layer
        gameObject.layer = LayerMask.NameToLayer("Cargo");
        
        //initialize the pool
        pool = new GameObjectPool(itemGenerated, 5, transform); 
        
        //set the cargo count 
        GameData.CargoCount = GameData.CargoMax;

    }

    public override void TryPickUp(GameObject source)
    {
        if(GameData.CargoCount <= 0) return;
        //increment the cargo count in stage parameter
        GameData.CargoCount--;
        base.TryPickUp(source);
    }
    
    public void DropOff(GameObject src)
    {
        
        if(src.TryGetComponent(out AIBrain brain)) return;
        //increase the cargo count
        GameData.CargoCount++;


        //increase the score
        if(ScoreManager.Instance)
            ScoreManager.Instance.AddScore(score);
        
        
        //update the text
        if(cargoText)
            cargoText.text = GameData.CargoCount+"/"+GameData.CargoMax;
        
        //remove the item from the inventory
        if (src.TryGetComponent(out Inventory inv))
        {
            inv.RemoveItem();
        }
        
    }
    

    
    //Handles what happens when the cargo is picked up
    void UpdateCargo(GameObject src)
    {
        if(GameData.CargoCount <= 0) return;
        //increase the cargo count
        GameData.CargoCount--;
        //decrease the score
        ScoreManager.Instance.DecreaseScore(score);
        //update the text
        cargoText.text = GameData.CargoCount + "/" + GameData.CargoMax;
    }
}