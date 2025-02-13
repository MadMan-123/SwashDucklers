using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] GameObject tentacles;
    [SerializeField] GameObject hitBox;
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

    public void Initialise()
    {
        tentacles.SetActive(true);
        hitBox.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator AnimateTentacle()
    {
        //anim[i].SetTrigger("KrakenSpawn");
        yield return new WaitForSeconds(spawnTime);
        Knockback();
        hitBox.SetActive(false);
    }

    void Knockback()
    {

    }
    public void KrakenRetreat()
    {
        foreach (Animator anim in anim)
        {
            anim.SetTrigger("KrakenRetreat");
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

}
