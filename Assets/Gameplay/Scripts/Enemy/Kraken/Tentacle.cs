using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] GameObject[] tentacles;
    [SerializeField] GameObject[] hitBox;
    [SerializeField] Animator[] anim;
    [SerializeField] float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        AnimateTentacle();
    }

    IEnumerator AnimateTentacle()
    {
        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i].SetActive(true);
            anim[i].SetTrigger("KrakenSpawn");
        }
        yield return new WaitForSeconds(spawnTime);
        Knockback();
        Initialise();
    }
    public void Initialise()
    {
        for (int i = 0; i < tentacles.Length; ++i)
        {
            hitBox[i].SetActive(false);
            tentacles[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
    void Knockback()
    {

    }

}
