using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShip : MonoBehaviour
{

    [SerializeField] int E_shiphealth;
    [SerializeField] AiMan AImgr;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        AImgr = GetComponent<AiMan>();

        anim = GetComponent<Animator>();

        E_shiphealth = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "projectile")
        {
            Hit();
        }

    }


    public void Hit()
    {
        E_shiphealth -= 10;

        if (E_shiphealth <= 0)
        {
            AImgr.WaveEnd();

            anim.Play("sink");
        }

    }
}
