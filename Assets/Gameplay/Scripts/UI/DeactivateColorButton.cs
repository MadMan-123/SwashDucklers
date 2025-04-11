using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateColorButton : MonoBehaviour
{

    [SerializeField] Button thisButton;
    [SerializeField] DuckColors thisColor;

    // Update is called once per frame
    void Update()
    {

        thisButton.interactable = checkColor();

    }

    bool checkColor()
    {
        switch (thisColor)
        {
            case DuckColors.Yellow:
                if (PlayerStats.yellowTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Orange:
                if (PlayerStats.orangeTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Red:
                if (PlayerStats.redTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Green:
                if (PlayerStats.greenTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Pink:
                if (PlayerStats.pinkTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.White:
                if (PlayerStats.whiteTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Blue:
                if (PlayerStats.blueTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Cyan:
                if (PlayerStats.cyanTaken == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case DuckColors.Purple:
                if (PlayerStats.purpleTaken == false)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
                break;
        }
        return false;
    }
}
