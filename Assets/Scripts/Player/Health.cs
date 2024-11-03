using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//Maybe change this to just Health instead of playerHEALTH? - MW
public class Health : MonoBehaviour//GM this script will be for the player's health/damage accumulation
{
    [SerializeField] private float health = 0.0f; //GM: player's health starts of at the base value of 0.0%
    [SerializeField] private TextMeshProUGUI healthText; // refernece to UI Text to display Health   
    [SerializeField] private UnityEvent<float> onDamageTaken;

    //application of damage
    public void Update()
    {
        //health += damage; //GM: adds the incoming damage to the player's health
        //updatePlayerHealth(); 
        //you had the right idea but we should move this into a sepereate function rather than on update
    }

    public void TakeDamage(float damage)
    {
        //accumulate damage
        health += damage;  
        //here we can update the player health ui
        UpdateHealthUI();
        
        //sean suggested a quack when the player gets slapped so ill implament a quick on damage
        onDamageTaken?.Invoke(health);
        
    }
    
    private void UpdateHealthUI()
    {
        //if there is no health text return
        if (!healthText) return;
        //healthText.text = string.Format("player health: {00:0.0}%", health);//stuart's code
        //healthText.text = health.ToString("F1") + "%"; //GM: this should display the health as a percentage with one decimal point | update: it doesn't :|
        healthText.text = $"{gameObject.name} Health: {health}%"; //updating the text 
    }
    
    //Get Health : float
    public float GetHealth() => health;



}
