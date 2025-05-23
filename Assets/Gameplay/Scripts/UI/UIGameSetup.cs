using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIGameSetup : MonoBehaviour
{

    [SerializeField] Toggle krakenToggle;

    [SerializeField] TextMeshProUGUI krakenHealthText;

    [SerializeField] TextMeshProUGUI LevelLengthText;

    [SerializeField] GameObject StartTransition;

    // Start is called before the first frame update
    void Start()
    {
        krakenHealthText.text = StageParameters.krakenHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Start Game
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            startGame();
        }

        //Return to previous menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            StartCoroutine(Transition(1));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

    }

    public void startGame()
    {
        StartCoroutine(Transition(3));
        //UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void kraken()
    {
        if (krakenToggle.isOn == true)
        {
            StageParameters.krakenEnabled = true;
            Debug.Log("Kraken On");
        }
        else
        {
            StageParameters.krakenEnabled = false;
            Debug.Log("Kraken Off");
        }
    }

    public void krakenHealthUp()
    {
        if (StageParameters.krakenHealth < 10)
        {
            StageParameters.krakenHealth = StageParameters.krakenHealth + 1;
            krakenHealthText.text = StageParameters.krakenHealth.ToString();
        }
    }

    public void krakenHealthDown()
    {
        if (StageParameters.krakenHealth > 1)
        {
            StageParameters.krakenHealth = StageParameters.krakenHealth - 1;
            krakenHealthText.text = StageParameters.krakenHealth.ToString();
        }
    }

    public void levelLengthUp()
    {
        switch (StageParameters.levelLength)
        {
            case Length.Short:
                StageParameters.levelLength = Length.Medium;
                LevelLengthText.text = "Medium";
                break;
            case Length.Medium:
                StageParameters.levelLength = Length.Long;
                LevelLengthText.text = "Long";
                break;
            case Length.Long:
                StageParameters.levelLength = Length.Endless;
                LevelLengthText.text = "Endless";
                break;
            case Length.Endless:
                break;
        }
    }

    public void levelLengthDown()
    {
        switch (StageParameters.levelLength)
        {
            case Length.Short:
                break;
            case Length.Medium:
                StageParameters.levelLength = Length.Short;
                LevelLengthText.text = "Short";
                break;
            case Length.Long:
                StageParameters.levelLength = Length.Medium;
                LevelLengthText.text = "Medium";
                break;
            case Length.Endless:
                StageParameters.levelLength = Length.Long;
                LevelLengthText.text = "Long";
                break;
        }
    }

    //Starts transition animation and waits till its done before moving scene
    //Ive coppied this into a bunch of scripts ideally we could set it up to be usable in any scene
    //It also would probably be better if the time wasnt hardcoded, and the animation had a loop while the next scene loads
    //-SD
    private IEnumerator Transition(int scene)
    {

        StartTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
