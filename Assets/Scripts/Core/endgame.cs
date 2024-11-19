using UnityEngine;
using UnityEngine.SceneManagement;

public class endGAME : MonoBehaviour
{
    [SerializeField] GameManager gm;
    // Update is called once per frame
    void Update()
    {
        // Check if the gm.gameOver is true, if it is then the game will end and then players will be returned to the menu screen
        if (gm.gameOver == true)
        {
            ReturnToMenuScene();
        }
    }

    void ReturnToMenuScene()//GM: this will load the players back to the main menu
    {
        SceneManager.LoadScene("menu test");
    }
}