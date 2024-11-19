using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShipHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxShipHealth;
    [SerializeField] float shipHealth;
    [SerializeField] float regenRate;
    [SerializeField] float percentageDamaged;
    bool regenerate;
    bool loop;

    [Header("Damage")]
    [SerializeField] public float dmgRate;
    [SerializeField] float dmgSpeed;
    [SerializeField] int leaks;
    [SerializeField] float shipFilled;
    [SerializeField] GameManager gm;
    [SerializeField] GameObject ship;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] float transitionspeed;
    float displayhealth;

    [Header("Visuals")]
    [SerializeField] float currentShipHeight;
    [SerializeField] float maxShipHeight = 0f;
    [SerializeField] float minShipHeight = -6.5f;
    public static ShipHealth instance;


    private void Awake()
    {
        //singleton boilerplate
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        shipHealth = maxShipHealth;
        shipFilled = 0;
        displayhealth = 100;
        leaks = 0;
        regenerate = true;
        loop = true;
        gm = gameObject.AddComponent<GameManager>();
    }
    
   void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            DamageShip(5);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            RepairShip(5);
        }

        if (shipHealth <= 0)
        {
            gm.gameOver = true;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (loop==true)
        {
            StartCoroutine(PassiveShipHealth(regenerate));
            loop = false;
        }

        //displayhealth = Mathf.MoveTowards(displayhealth, 0, shipHealth * Time.deltaTime); //updates the ship health ui with the ships heal
        //UpdateScoreDisplay();
    }

    public void DamageShip(int damage)
    {
        shipHealth = Mathf.Clamp(shipHealth - damage, 0, maxShipHealth);
        leaks += 1;
        regenerate = false;
    }

    public void RepairShip(int repair)
    {
        shipHealth = Mathf.Clamp(shipHealth + repair, 0, maxShipHealth);
        leaks = Mathf.Clamp(leaks-1, 0 ,100);
        if (leaks < 1) regenerate = true;
        //percentageDamaged = Mathf.Clamp(100-(Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100),0, 100);
    }

    IEnumerator PassiveShipHealth(bool regenerate)
    {
        //percentageDamaged = Mathf.Lerp(maxShipHealth, 0, shipHealth) * 100;  //if ship is on 90% health this value shows 10% || 80% shows 20% et
        //percentageDamaged = Mathf.Lerp(maxShipHealth - shipHealth, 0, maxShipHealth) * 100;
        dmgSpeed = (dmgRate)/10 * leaks;                                     
        if(regenerate) //if regenerate
        {
            shipHealth = Mathf.Clamp(shipHealth + (regenRate / 10), 0 ,maxShipHealth); //gain hp
            percentageDamaged = maxShipHealth - shipHealth;
        }
        else if (!regenerate) //if not regenerate
        {
            shipHealth = Mathf.Clamp(shipHealth - dmgSpeed, 0, maxShipHealth); //take DOT proportional to leaks
            percentageDamaged = maxShipHealth - shipHealth;
        }
  
        yield return new WaitForSeconds(0.1f);
        loop = true;

        //shipFilled = Mathf.Clamp((shipFilled + fillSpeed), 0, 100);
        currentShipHeight = Mathf.Clamp((Mathf.Lerp(0,1,percentageDamaged/100)* minShipHeight),minShipHeight, maxShipHeight);       
        ship.transform.position = new Vector3(ship.transform.position.x,currentShipHeight,ship.transform.position.z);
        //might rewrite this to move water for simplicity, will talk to designer
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

