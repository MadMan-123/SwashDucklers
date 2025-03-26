using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Skip()
    {
        SceneManager.LoadScene("Boat Scene");
    }

    public void dontSkip()
    {
        SceneManager.LoadScene("island tutorial test");
    }

}
