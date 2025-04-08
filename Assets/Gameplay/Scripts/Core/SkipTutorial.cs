using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTutorial : MonoBehaviour
{

    [SerializeField] GameObject StartTransition;

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
        //SceneManager.LoadScene("Boat Scene");
        StartCoroutine(Transition(3));
    }

    public void dontSkip()
    {
        //SceneManager.LoadScene("island tutorial test");
        StartCoroutine(Transition(7));
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
