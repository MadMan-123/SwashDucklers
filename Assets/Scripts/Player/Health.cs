using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//Maybe change this to just Health instead of playerHEALTH? - MW
public class Health : MonoBehaviour//GM this script will be for the player's health/damage accumulation
{
    [SerializeField] private float health = 0.0f; //GM: player's health starts of at the base value of 0.0%
    [SerializeField] private float maxHealth = 100.0f; //GM: Maximum health cap
    [SerializeField] private float maxKnockbackForce = 10.0f; //GM: Maximum knockback force
    [SerializeField] private Rigidbody rb; //GM: Reference to Rigidbody component
    [SerializeField] private TextMeshProUGUI healthText; //GM: refernece to UI Text to display Health   
    [SerializeField] private Color healthTextColor = Color.white; // Color for health text, default is white GM: STILL WORKING ON THIS
    [SerializeField] private UnityEvent<float> onDamageTaken;

    //application of damage
    public void Update()
    {
        //health += damage; //GM: adds the incoming damage to the player's health
        //updatePlayerHealth(); 
        //you had the right idea but we should move this into a sepereate function rather than on update
    }

    private void Awake()
    {
        //GM: Make sure the Rigidbody is assigned in the inspector or get it from the component
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)
    {
        //GM: Accumulate damage and cap health at 100%
        health += damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        
        //sean suggested a quack when the player gets slapped so ill implament a quick on damage
        onDamageTaken?.Invoke(health);

        //We were already doing knockback in the interact component -MW
        //GM: Apply knockback
        //ApplyKnockback();
    }

    public void ApplyKnockback(Vector3 dir)
    {
        // GM: Calculate the knockback force based on the player's current health percentage.
        // As the player's health increases, the knockback force also increases, up to maxKnockbackForce.
        float knockbackForce = (health / maxHealth) * maxKnockbackForce;

        // GM: Set the direction of the knockback.
        // We use '-transform.forward' to push the player backward relative to their current facing direction.
        // Adjust this direction if you want knockback to go in a different direction.


        // GM: Apply the calculated knockback force to the player's Rigidbody.
        //'AddForce' applies the force instantly (due to 'ForceMode.Impulse') to simulate a knockback effect.
        // The direction and magnitude of the force are determined by 'knockbackDirection * knockbackForce'.
        rb.AddForce(dir + (Vector3.up * 0.1f) * knockbackForce, ForceMode.Impulse);
    }



    private void UpdateHealthUI()
    {
        //if there is no health text return
        if (!healthText) return;
        //healthText.text = string.Format("player health: {00:0.0}%", health);//stuart's code
        //healthText.text = health.ToString("F1") + "%"; //GM: this should display the health as a percentage with one decimal point | update: it doesn't :|
        healthText.text = $"HP: {health}%"; //updating the text 
        // Ensure health text uses the assigned color
        healthText.color = healthTextColor;//gm: STILL WORKING ON THIS
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
