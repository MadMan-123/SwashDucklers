using UnityEngine;
using UnityEngine.SceneManagement;

public class endGAME : MonoBehaviour
{
    [SerializeField] GameManager gm;

    // Update is called once per frame
    void Update()
    {
        // Check if gm.gameOver is true
        if (gm.gameOver == true)
        {
            ReturnToGameOverScreen();
        }
    }

    void ReturnToGameOverScreen() // Loads the GameOverScreen scene
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
