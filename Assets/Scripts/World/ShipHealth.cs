using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.InputSystem.Controls;

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
    [SerializeField] GameObject ship;
    float currentShipHeight;
    [SerializeField]float maxShipHeight = 0f;
    [SerializeField] float minShipHeight = -6.5f;
    bool fill=true;

    // Start is called before the first frame update
    void Start()
    {
        shipHealth = maxShipHealth;
        shipFilled = 0;
        displayhealth = 100;

    }
    
   void Update()
    {
        
        shipHealth -= fillSpeed / fillRate; // reduces the ships health over time while the ship is damaged
       
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (shipHealth != 0 && fill)
        {
            StartCoroutine(FillShip());
            fill = false;
        }

        displayhealth = Mathf.MoveTowards(displayhealth, 0, shipHealth * Time.deltaTime); //updates the ship health ui with the ships heal
        UpdateScoreDisplay();
    }

    public void DamageShip(int damage)
    {
        shipHealth = Mathf.Clamp(shipHealth - damage, 0, maxShipHealth);
        percentageDamaged = Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100;
    }

    public void RepairShip(int repair)
    {
        shipHealth = Mathf.Clamp(shipHealth + repair, 0, maxShipHealth);
        percentageDamaged = Mathf.Clamp(100-(Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100),0, 100);
    }

    IEnumerator FillShip()
    {
        fillSpeed = percentageDamaged / fillRate;
        shipFilled= Mathf.Clamp((shipFilled + fillSpeed),0,100);
        if (shipFilled == 100)
        {
            gm.gameOver = true;
        }
        yield return new WaitForSeconds(0.25f);
        fill = true;
        currentShipHeight = Mathf.Clamp((Mathf.Lerp(0,1,shipFilled/100)* minShipHeight),minShipHeight, maxShipHeight);
        ship.transform.position = new Vector3(ship.transform.position.x,currentShipHeight,ship.transform.position.z);
    }

    public void UpdateScoreDisplay()
    {
        healthText.text = string.Format("ship health: {0:0}%", displayhealth);
    }

    public void EmptyShip(float remove)
    {
        shipFilled = Mathf.Clamp(0,100,remove);
    }


    public void bucket()
    {
        if (shipFilled > 10 && shipFilled < 100)
        {
            shipFilled -= 10;
            shipHealth += 10;
        }
    }
}

