using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAnimationEvents : MonoBehaviour
{
    [SerializeField]private KrakenManager krakenManager;
    [SerializeField]private TentacleAI tentacleAI;
    [SerializeField] private Tentacle tentacle;
    [SerializeField]private bool isCosmetic;
    public float intensity = 10f;
    public float duration = 0.1f;
    public void Shake()
    {
        CameraShake.Instance.ShakeCamera(intensity, duration);
    }

    public void DisableTentacles()
    {
        Debug.Log("Disabling tentacles");
        if (isCosmetic) {krakenManager.DisableCosmeticTentacles();}
        else {krakenManager.DisableTentacles();}
    }

    public void EnableHitBoxes()
    {
        tentacle.ToggleHitboxes(true);
    }

    public void DisableHitBoxes()
    {
        tentacle.ToggleHitboxes(false);
    }
    
}
