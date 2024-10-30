using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class playerHEALTH : MonoBehaviour//GM this script will be for the player's health/damage accumulation
{
    public float health = 0.0f; //GM: player's health starts of at the base value of 0.0%
    public TextMeshProUGUI healthText; // refernece to UI Text to display Health   

    //application of damage
    public void Update()
    {
        //health += damage; //GM: adds the incoming damage to the player's health
        updatePlayerHealth();
    }

    private void updatePlayerHealth()
    {
        //healthText.text = string.Format("player health: {00:0.0}%", health);//stuart's code
        healthText.text = health.ToString("F1") + "%"; //GM: this should display the health as a percentage with one decimal point | update: it doesn't :|
    }
}
