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

    public Color litColor;

    private Vector3 hatposition;

    // Start is called before the first frame update
    void Start()
    {
        modelTransform = transform;
        bodyRenderer = modelTransform.GetComponent<Renderer>();

        hatposition = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        //Test
        //hat = Instantiate(hatTest, transform.position, transform.rotation, this.transform);

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(PlayerStats.player1Hat);

        Debug.Log(PlayerStats.Hatlist[PlayerStats.player1Hat].name);

        //Get Color and hats
        switch (playerID)
        {
            case 0:
                litColor = PlayerStats.player1Color;
                if (PlayerStats.player1Hat != currentHat) //This if prevents unnessisary reinstantiation each frame
                {
                    Destroy(hat);
                    if (PlayerStats.Hatlist[PlayerStats.player1Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player1Hat].model, hatposition, transform.rotation);
                    }
                    currentHat = PlayerStats.player1Hat;
                    Debug.Log(hat);
                }
                break;
            case 1:
                litColor = PlayerStats.player2Color;
                if (PlayerStats.player2Hat != currentHat)
                {
                    Destroy(hat);
                    if (PlayerStats.Hatlist[PlayerStats.player2Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player2Hat].model, hatposition, transform.rotation);
                    }
                    currentHat = PlayerStats.player2Hat;
                }
                break;
            case 2:
                litColor = PlayerStats.player3Color;
                if (PlayerStats.player3Hat != currentHat)
                {
                    Destroy(hat);
                    if (PlayerStats.Hatlist[PlayerStats.player3Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player3Hat].model, hatposition, transform.rotation);
                    }
                    currentHat = PlayerStats.player3Hat;
                }
                break;
            case 3:
                litColor = PlayerStats.player4Color;
                if (PlayerStats.player4Hat != currentHat)
                {
                    Destroy(hat);
                    if (PlayerStats.Hatlist[PlayerStats.player4Hat].model != null)
                    {
                        hat = Instantiate(PlayerStats.Hatlist[PlayerStats.player4Hat].model, hatposition, transform.rotation);
                    }
                    currentHat = PlayerStats.player4Hat;
                }
                break;
        }

        //Set Color
        bodyRenderer.material.SetColor("_BaseColor", litColor); //Light Color
    }
}
