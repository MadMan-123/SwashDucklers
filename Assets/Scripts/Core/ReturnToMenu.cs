using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{


    //Moved to player controler - sean

    // Update is called once per frame
   // void Update()
    //{
        //GM: Check if the "K" key is pressed
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        ReturnToMenuScene();
    //    }

    //    //GM: Check if the "B" button (ButtonEast) is pressed on a game controller
    //    if (Input.GetKeyDown(KeyCode.JoystickButton1)) //GM: for this I used buttonEast (B - Xbox, A - Switch, Circle - Playstation controller) to set the player back the the menu screen, originally I wanted it to be the left trigger but for some reason code didn't work :|
    //    {
    //        ReturnToMenuScene();
    //    }
    //}

    //void ReturnToMenuScene()
    //{
    //    SceneManager.LoadScene("menu test"); //GM: returns to the "menu test" screen
    //    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    //}
}
