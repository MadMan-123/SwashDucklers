using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] KrakenManager krakenManager;
    void Start()
    {
        
    }

    public void DisableKrakenModel()
    {
        //Might have to lower model so its not on screen - TS
        krakenManager.DisableBody();
    }
}
