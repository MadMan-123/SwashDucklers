using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] float maxShipHealth;
    [SerializeField] float fillRate;
    [SerializeField] float shipFilled;
    [SerializeField] float shipHealth;
    [SerializeField] float percentageDamaged;
    [SerializeField] float fillSpeed;
    float displayhealth;
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] float transitionspeed;
    [SerializeField] GameManager gm;
    bool fill=true;

    // Start is called before the first frame update
    void Start()
    {
        shipHealth = maxShipHealth;
        shipFilled = 0;


    }
    
   void Update()
    {
        shipHealth -= fillSpeed * Time.deltaTime;
       
        displayhealth = Mathf.MoveTowards(displayhealth, shipHealth,  transitionspeed * Time.deltaTime);
        UpdateScoreDisplay();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(percentageDamaged != 0 && fill)
        {
            StartCoroutine(FillShip());
            fill = false;
        }
    }

    public void DamageShip(int damage)
    {
        shipHealth = Mathf.Clamp(shipHealth - damage, 0, maxShipHealth);
        percentageDamaged = Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100;
    }

    public void RepairShip(int repair)
    {
        shipHealth = Mathf.Clamp(shipHealth + repair, 0, maxShipHealth);
        percentageDamaged = 100-(Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100);
    }

    IEnumerator FillShip()
    {
        fillSpeed = percentageDamaged / fillRate;
        shipFilled=shipFilled + Mathf.Clamp(fillSpeed,0,100);
        if (shipFilled == 100)
        {
            gm.gameOver = true;
        }
        yield return new WaitForSeconds(0.5f);
        fill = true;
    }

    public void UpdateScoreDisplay()
    {
        healthText.text = string.Format("ship health: {0:0}%", displayhealth);
    }

    public void EmptyShip(float remove)
    {
        shipFilled = Mathf.Clamp(0,100,remove);
    }
}
