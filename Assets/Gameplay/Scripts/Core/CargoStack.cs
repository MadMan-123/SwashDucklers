
using System;
using Core;
using TMPro;
using UnityEngine;

public class CargoStack : ItemStack
{
    [SerializeField] public TextMeshProUGUI cargoText;
    public int score = 10;
    
    
    private void Start()
    {
        OnPickUp += UpdateCargoUI;
        //set layer
        gameObject.layer = LayerMask.NameToLayer("Cargo");
        
        pool = new GameObjectPool(itemGenerated, 10, transform); 
        
    }



    void UpdateCargoUI(GameObject src)
    {
        GameData.CargoCount++;
        ScoreManager.Instance.DecreaseScore(score);
        cargoText.text = $"Cargo: {GameData.CargoCount}/{GameData.CargoMax}";
    }
}