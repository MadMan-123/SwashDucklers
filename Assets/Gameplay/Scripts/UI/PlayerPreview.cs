using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreview : MonoBehaviour
{

    //This script exists on the players in the character creator screen
    //It just handles visuals for the players

    [SerializeField] int playerID;
    private Transform modelTransform;
    private Renderer bodyRenderer;
    private GameObject hat;
    private int currentHat;
    [SerializeField] Transform hatTransform;

    public Color litColor;

    private Vector3 hatposition;

    // Start is called before the first frame update
    void Start()
    {
        modelTransform = transform;
        bodyRenderer = modelTransform.GetComponent<Renderer>();

        hatposition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //Test
        //hat = Instantiate(hatTest, transform.position, transform.rotation, this.transform);

    }

    void SetHatLogic()
    {
        var hatToCheck = playerID switch
        {
            0 => PlayerStats.player1Hat,
            1 => PlayerStats.player2Hat,
            2 => PlayerStats.player3Hat,
            3 => PlayerStats.player4Hat,
            _ => 0
        };
        
        
        //Set Color
        if (hatToCheck != currentHat) //This if prevents unnessisary reinstantiation each frame
        {
            Destroy(hat);
            if (PlayerStats.Hatlist[PlayerStats.player1Hat].model != null)
            {
                hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player1Hat].model, hatposition + PlayerStats.Hatlist[PlayerStats.player1Hat].previewPosition, transform.rotation, hatTransform);
            }
            currentHat = PlayerStats.player1Hat;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //Get Color and hats
        SetHatLogic(); 
        
        litColor = playerID switch
        {
            0 => PlayerStats.player1Color,
            1 => PlayerStats.player2Color,
            2 => PlayerStats.player3Color,
            3 => PlayerStats.player4Color,
            _ => Color.white
        };
        
        
        //Set Color
        bodyRenderer.material.SetColor("_Color", litColor); //Light Color
        
        
    }
}
