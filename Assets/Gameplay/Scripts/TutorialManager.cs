using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] GameObject imageBox;
    [SerializeField] GameObject imageBox2;
    [SerializeField] GameObject movementImage;
    [SerializeField] GameObject dropImage;
    [SerializeField] GameObject fixImage;
    [SerializeField] GameObject cannonImage;
    [SerializeField] GameObject grabImage;
    [SerializeField] GameObject slapImage;
    [SerializeField] GameObject timmyTurnersDad;

    [SerializeField] GameObject exampleLeak;
    [SerializeField] GameObject leakArrow;
    [SerializeField] GameObject moreExampleLeaks;

    [SerializeField] GameObject examplePlanks;
    [SerializeField] GameObject plankArrow;

    [SerializeField] EnemySpawner crabs;

    [SerializeField] GameObject exampleCannons;
    [SerializeField] GameObject cannonballs;
    [SerializeField] GameObject carboardKraken;

    [SerializeField] int flagsTriggered = 0;

    // Start is called before the first frame update
    void Start()
    {

        //Tutorial events are called in sequence here

        //Hide Tutorial Objects we dont need yet
        imageBox.SetActive(false);
        imageBox2.SetActive(false);
        movementImage.SetActive(false);
        dropImage.SetActive(false);
        fixImage.SetActive(false);
        cannonImage.SetActive(false);
        grabImage.SetActive(false);
        slapImage.SetActive(false);
        timmyTurnersDad.SetActive(false);

        exampleLeak.SetActive(false);
        leakArrow.SetActive(false);
        moreExampleLeaks.SetActive(false);

        examplePlanks.SetActive(false);
        plankArrow.SetActive(false);

        crabs.enabled = false;

        exampleCannons.SetActive(false);
        cannonballs.SetActive(false);
        carboardKraken.SetActive(false);

        //Start tutorial
        StartCoroutine(tutorial());

        //Intro Message
        //Wait a bit
        //Movement tutorial
        //Wait a bit
        //Leaks
        //Spawn leak and explain
        //wait
        //Spawn wood and explain
        //Wait for player to fix leak
        //Start counter for leaks
        //Spawn leak manager
        //At half way point spawn crabs
        //Explain slap
        //Once done despawn leaks wood and crabs
        //Explain Kraken
        //Spawn cannon and cannonball
        //Wait for 3 hits
        //This is where id put my cargo tutorial
        //IF I HAD ONE
        //Good luck message
        //Go to main scene

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void triggerFlag()
    {
        flagsTriggered = flagsTriggered + 1;
    }

    bool firstLeakFixed()
    {
        return (flagsTriggered == 1);
    }

    bool halfLeaksFixed()
    {
        return (flagsTriggered == 5);
    }

    bool allLeaksFixed()
    {
        return (flagsTriggered == 10);
    }

    bool cannonsFired()
    {
        return (flagsTriggered == 3);
    }

    private IEnumerator tutorial()
    {
        //Intro
        StartCoroutine(TypeText("Welcome to Swashducklers!!!"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Before we set sail, We need to make sure you know what your doing."));
        yield return new WaitForSeconds(5f);

        //Movement Tutorial
        StartCoroutine(TypeText("First, lets make sure you have your land legs, use the right stick to move"));
        imageBox.SetActive(true);
        movementImage.SetActive(true);
        yield return new WaitForSeconds(10f);
        StartCoroutine(TypeText("Looks like you`re getting the hang of it"));
        imageBox.SetActive(false);
        movementImage.SetActive(false);
        yield return new WaitForSeconds(5f);

        //Leak Tutorial
        StartCoroutine(TypeText("While on our journey, There will be all sorts of Dangers"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("When you see a leak like this, that means the ships taking damage :("));
        leakArrow.SetActive(true);
        exampleLeak.SetActive(true);

        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("You`ll need to work together to repair it post-haste!"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Head over to the wood pile, and press the left button to grab a plank"));
        leakArrow.SetActive(false);
        examplePlanks.SetActive(true);
        plankArrow.SetActive(true);
        imageBox.SetActive(true);
        grabImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Once you`ve got one, you can use the button again to repair a leak"));
        imageBox2.SetActive(true);
        fixImage.SetActive(true);

        yield return new WaitUntil(firstLeakFixed);//is repaired
        StopCoroutine("TypeText");
        flagsTriggered = 0;

        plankArrow.SetActive(false);
        imageBox.SetActive(false);
        grabImage.SetActive(false);
        imageBox2.SetActive(false);
        fixImage.SetActive(false);
        StartCoroutine(TypeText("Good Job! but just to make sure everyones got a hang of it..."));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Try Fixing a few more!"));
        moreExampleLeaks.SetActive(true);
        //Spawn leak counter

        yield return new WaitUntil(halfLeaksFixed);//5/10 reached
        StopCoroutine("TypeText");

        StartCoroutine(TypeText("Oh no here come some crabs!"));
        crabs.enabled = true;
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Try using the left button to fight them off!"));
        imageBox.SetActive(true);
        slapImage.SetActive(true);

        yield return new WaitUntil(allLeaksFixed);//10/10 reached
        StopCoroutine("TypeText");
        flagsTriggered = 0;

        imageBox.SetActive(false);
        slapImage.SetActive(false);
        crabs.enabled = false;
        exampleLeak.SetActive(false);
        moreExampleLeaks.SetActive(false);
        examplePlanks.SetActive(false);
        StartCoroutine(TypeText("Good Job! Theres just one last thing to teach you!"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("You might encounter the legendary Kraken!"));
        carboardKraken.SetActive(true);
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("You`ll need to fight them off or the ship will be stuck in place!"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Grab a cannonball and load it into the cannon to fight them off!"));
        imageBox.SetActive(true);
        cannonImage.SetActive(true);
        exampleCannons.SetActive(true);
        cannonballs.SetActive(true);
        //Show kraken health
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("You can also use the right button to drop items if you need to!"));
        imageBox2.SetActive(true);
        dropImage.SetActive(true);

        yield return new WaitUntil(cannonsFired);//Kraken Defeated
        StopCoroutine("TypeText");
        flagsTriggered = 0;

        imageBox.SetActive(false);
        cannonImage.SetActive(false);
        imageBox2.SetActive(false);
        dropImage.SetActive(false);
        exampleCannons.SetActive(false);
        cannonballs.SetActive(false);
        carboardKraken.SetActive(false);

        StartCoroutine(TypeText("Thats it!, youve really got the hang of this now"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("This is where id put my cargo tutorial"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("IF I HAD ONE"));
        imageBox.SetActive(true);
        timmyTurnersDad.SetActive(true);
        yield return new WaitForSeconds(5f);
        imageBox.SetActive(false);
        timmyTurnersDad.SetActive(false);

        StartCoroutine(TypeText("With that, its time for you to get going!"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeText("Good luck! and try not to sink!"));
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Boat Scene");
    }

    private IEnumerator TypeText(string Text)
    {

        tutorialText.text = "";//Blank out text

        //Type in all text
        for (int i = 0; i < Text.Length; i++)
        {
            tutorialText.text = tutorialText.text + Text[i];
            yield return new WaitForSeconds(0.05f);
        }

    }

}
