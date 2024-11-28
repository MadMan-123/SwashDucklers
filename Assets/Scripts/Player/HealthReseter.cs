using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReseter : MonoBehaviour
{
    public void ResetHealth(GameObject source)
    {
        if (source == null)
            return;

        if(source.TryGetComponent(out Health hp))
        {
            hp.SetHealth(0);
        }
    }
}
