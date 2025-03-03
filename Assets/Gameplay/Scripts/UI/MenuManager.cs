using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    
    public GameObject gameOverPanel;
    
    //singleton pattern
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool toggle;
    public void LoadScene(int index)
    {
        // Load the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
    
    public void ExitGame()
    {
        // Quit the game
        Application.Quit();
    }
    
    public void LoadPanel(GameObject panel)
    {
        // Set the settings panel to active
        panel.SetActive(true);
    }
    
    public void ClosePanel(GameObject panel)
    {
        // Set the settings panel to inactive
        panel.SetActive(false);
    }
    
    public void PausePanelToggle(GameObject panel)
    {
        toggle = !toggle;
        if (toggle)
            PauseGame();
        else
            ResumeGame();
        // Set the settings panel to active
        panel.SetActive(toggle);
    }

    private void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0;
        
        //unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1;
        
        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


   // public void LoadGameOver()
    //{
     //   if (!gameOverPanel)
     //   {
     //       #if UNITY_EDITOR
     //                   Debug.LogError("No Game Over Panel found.");
     //       #endif
      //  }
        //pause the game
    //    PauseGame();
    //    LoadScene(4);
        //

    //
}
