using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticle : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateMe()
    {
        //anim.SetTrigger("Repair");
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
