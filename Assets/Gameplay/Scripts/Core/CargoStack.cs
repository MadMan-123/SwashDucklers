
using System;
using Core;
using TMPro;
using UnityEngine;

public class CargoStack : ItemStack
{
    //reference to the text ui
    [SerializeField] public TextMeshProUGUI cargoText;
    //score to decrease
    public int score = 10;
    
    
    private void Start()
    {
        //subscribe to the event
        OnPickUp += UpdateCargo;
        //set layer
        gameObject.layer = LayerMask.NameToLayer("Cargo");
        
        //initialize the pool
        pool = new GameObjectPool(itemGenerated, 10, transform); 
        
    }


    
    //Handles what happens when the cargo is picked up
    void UpdateCargo(GameObject src)
    {
        //increase the cargo count
        GameData.CargoCount++;
        //decrease the score
        ScoreManager.Instance.DecreaseScore(score);
        //update the text
        cargoText.text = $"Cargo: {GameData.CargoCount}/{GameData.CargoMax}";
    }
}