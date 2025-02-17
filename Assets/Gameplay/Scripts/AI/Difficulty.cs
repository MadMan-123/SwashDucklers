using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DifficultyManager : MonoBehaviour
{
    // Enum representing the difficulty levels
    public enum Difficulty 
    { 
        
        Hatchling, //easy difficulty
        Crewmate,  //normal difficulty
        SwashDuckler //hard difficulty

    }

    // Current difficulty level, default set to Hatchling (easy)
    public Difficulty currentDifficulty = Difficulty.Hatchling;

    // References to the external scripts that will be affected by difficulty changes
    /*public CannonBall;
    public GameTimer;*/

    void Update()
    {
        // Check if the player presses the 'O' key
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Cycle through difficulties, so the difficulty should be done in this progression: Hatcling > Crewmate > SwashDuckler > then back to Hatchling
            CycleDifficulty();
        }
    }

    void CycleDifficulty()
    {
        // Move to the next difficulty level, loop back to the start if at the end
        currentDifficulty++;
        if ((int)currentDifficulty > 2)
        {
            currentDifficulty = Difficulty.Hatchling; //Hatchling (easy) is the base difficulty
        }

        // Apply changes based on the selected difficulty
        ApplyDifficulty();
    }

    void ApplyDifficulty()
    {
        // Adjust game settings according to the current difficulty level
        /*switch (currentDifficulty)
        {
            case Difficulty.Hatchling:
                GameTimer.SetTimeScale(0.5f);  // Slows down the game timer by 50%
                CannonBall.SetDamage(2f);       // Increases cannonball damage to 2
                Debug.Log("Difficulty set to Hatchling (Easy)");
                break;

            case Difficulty.Crewmate:
                GameTimer.SetTimeScale(1f);    // Keeps the game timer normal
                CannonBall.SetDamage(1f);      // Keeps cannonball damage normal
                Debug.Log("Difficulty set to Crewmate (Medium)");
                break;

            case Difficulty.SwashDuckler:
                GameTimer.SetTimeScale(1.5f);  // Speeds up the game timer by 50%
                CannonBall.SetDamage(0.5f);    // Reduces cannonball damage to 0.5
                Debug.Log("Difficulty set to SwashDuckler (Hard)");
                break;
        }*/
    }
}

