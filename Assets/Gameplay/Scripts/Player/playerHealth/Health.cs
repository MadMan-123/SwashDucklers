using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//Maybe change this to just Health instead of playerHEALTH? - MW
public class Health : MonoBehaviour//GM this script will be for the player's health/damage accumulation
{
    [SerializeField] private float health = 0.0f; //GM: player's health starts of at the base value of 0.0%
    [SerializeField] private float maxHealth = StageParameters.krakenHealth; //GM: Maximum health cap
    [SerializeField] private TextMeshProUGUI healthText; //GM: refernece to UI Text to display Health   
    [SerializeField] private UnityEvent<GameObject> onDamageTaken;
    public void TakeDamage(GameObject source,float damage)
    {
        //GM: Accumulate damage and cap health at 100%
        health += damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        
        //sean suggested a quack when the player gets slapped so ill implament a quick on damage
        onDamageTaken?.Invoke(source);
    }




    private void UpdateHealthUI()
    {
        //if there is no health text return
        if (!healthText) return;
        healthText.text = $"HP: {health}%"; //updating the text 
        healthText.color = Color.white;//gm: STILL WORKING ON THIS
    }
    
    //Get Health : float
    public float GetHealth() => health;

    public float GetMaxHealth() => maxHealth;

    public void SetHealth(float getMaxHealth)
    {
        health = getMaxHealth;
        UpdateHealthUI();
    }
}
