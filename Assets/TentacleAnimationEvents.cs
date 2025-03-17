using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAnimationEvents : MonoBehaviour
{
    private KrakenManager krakenManager;
    public float intensity = 10f;
    public float duration = 0.1f;
    public void Shake()
    {
        CameraShake.Instance.ShakeCamera(intensity, duration);
    }

    public void DisableTentacles()
    {
        Debug.Log("Disabling tentacles");
        krakenManager.DisableTentacles();
    }
}
