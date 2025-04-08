using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    
    public GameObject gameOverPanel;

    [SerializeField] GameObject StartTransition;

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
        StartCoroutine(Transition(index));
        //UnityEngine.SceneManagement.SceneManager.LoadScene(index);
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

    //Starts transition animation and waits till its done before moving scene
    //Ive coppied this into a bunch of scripts ideally we could set it up to be usable in any scene
    //It also would probably be better if the time wasnt hardcoded, and the animation had a loop while the next scene loads
    //-SD
    private IEnumerator Transition(int scene)
    {

        StartTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        //AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

        //while (!asyncLoad.isDone)
        //{
        //   yield return null;
        //}

        //return null;
    }


}
