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
            anim[i].SetTrigger("Spawned");
        }
        yield return new WaitForSeconds(spawnTime);
        Knockback();
        foreach (GameObject hb in hitBox)
        {
            hb.SetActive(true);
        }
    }

    void Knockback()
    {

    }

}
