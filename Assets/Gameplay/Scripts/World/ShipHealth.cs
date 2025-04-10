using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class ShipHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxShipHealth;
    [SerializeField] float shipHealth;
    [SerializeField] float regenRate;
    [SerializeField] public float percentageDamaged;
    bool regenerate;
    bool loop;
    private bool shipDestroyed = false;

    [Header("Damage")]
    [SerializeField] public float dmgRate;
    [SerializeField] float dmgSpeed;
    [SerializeField] int leaks;
    [SerializeField] float shipFilled;
    [SerializeField] GameManager gm;
    [SerializeField] GameObject ship;
    [SerializeField] Animator shipAnim;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] float transitionspeed;
    float displayhealth;

    [Header("Visuals")]
    [SerializeField] float currentShipHeight;
    [SerializeField] float maxShipHeight = 0f;
    [SerializeField] float minShipHeight = -6.5f;
    public static ShipHealth instance;
    //private CinemachineVirtualCamera vCam;

    //Death animation stuff
    [Header("Death Animation Stuff")]
    [SerializeField] private CinemachineTargetGroup cameraTarget;
    [SerializeField] GameObject DeathPlane;

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
        //Set the max health
        maxShipHealth = StageParameters.maxShipHealth;

        //SD:Set the max ship height to the current height of the ship in game space
        //Should prevent it teleporting at game start
        maxShipHeight = transform.position.y;
        //vCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
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

        shipAnim.SetInteger("Ship Health", (int)shipHealth);

        
        #if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.F))
            {
                DamageShip(5);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                RepairShip(5);
            }
        #endif
        StageParameters.currentShipHealth = shipHealth;

        if (shipHealth <= 0)
        {
            gm.gameOver = true;
            StartCoroutine(Death());
        }

        if (shipDestroyed)
        {
            //Death animation -SD
            currentShipHeight = currentShipHeight - 0.01f;
            ship.transform.position =
               new Vector3(ship.transform.position.x, currentShipHeight, ship.transform.position.z);

            //UnfocusCamera
            cameraTarget.m_Targets = new CinemachineTargetGroup.Target[0];

            //Disable Death Plane
            DeathPlane.SetActive(false);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (loop == true)
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
        //figure out the dmg speed using the level time
    }

    public void RepairShip(int repair, int bonus = 4)
    {
        if (!shipDestroyed)
        {
            shipHealth = Mathf.Clamp(shipHealth + repair + bonus, 0, maxShipHealth);
            leaks = Mathf.Clamp(leaks - 1, 0, 100);
            if (leaks < 1) regenerate = true;
            //percentageDamaged = Mathf.Clamp(100-(Mathf.Lerp(shipHealth, 0, maxShipHealth) * 100),0, 100);
        }
    }

    IEnumerator PassiveShipHealth(bool regenerate)
    {
        if (!shipDestroyed)
        {
            //percentageDamaged = Mathf.Lerp(maxShipHealth, 0, shipHealth) * 100;  //if ship is on 90% health this value shows 10% || 80% shows 20% et
            //percentageDamaged = Mathf.Lerp(maxShipHealth - shipHealth, 0, maxShipHealth) * 100;
            dmgSpeed = (dmgRate) / 20 * leaks;
            if (regenerate) //if regenerate
            {
                shipHealth = Mathf.Clamp(shipHealth + (regenRate / 10), 0, maxShipHealth); //gain hp

            }
            else if (!regenerate) //if not regenerate
            {
                shipHealth = Mathf.Clamp(shipHealth - dmgSpeed, 0, maxShipHealth); //take DOT proportional to leaks
            }

            percentageDamaged = Mathf.Clamp(shipHealth / maxShipHealth, 0f, 1f);
            //vCam.m_Lens.FieldOfView = 70 - (percentageDamaged * 10);
            yield return new WaitForSeconds(0.1f);
            loop = true;

            //shipFilled = Mathf.Clamp((shipFilled + fillSpeed), 0, 100);
            currentShipHeight = Mathf.Lerp(minShipHeight, maxShipHeight, percentageDamaged);
            ship.transform.position =
                new Vector3(ship.transform.position.x, currentShipHeight, ship.transform.position.z);
            //might rewrite this to move water for simplicity, will talk to designer
        }
    }

    public void UpdateScoreDisplay()
    {
        healthText.text = string.Format("ship health: {0:0}%", displayhealth);
    }


    private IEnumerator Death()
    {
        shipAnim.SetBool("Sinking", true);
        shipDestroyed = true;
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}

