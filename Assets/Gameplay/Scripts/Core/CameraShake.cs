using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Assertions;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cmCam;
    private float baseFrequency = 1;
    [SerializeField] private float shakeTimer;
    private bool toggle;
    CinemachineBasicMultiChannelPerlin cmCamp;
    
    private void Awake()
    {
        Instance = this;
        if (!TryGetComponent(out cmCam))
        {
            Debug.LogError("No Cinemachine Virtual Camera found in scene!");
        }
        
        //try and get the cmCamp component
        cmCamp = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        //assert a null check
        Assert.IsNotNull(cmCamp);
        
        ShakeCamera(0, 0.1f);
    }

    
    
    void Update()
    {
        if (!toggle)
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
            }
            else if (shakeTimer <= 0f)
            {
                cmCamp.m_AmplitudeGain = 0f;
                cmCam.m_Lens.FieldOfView = 60f;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ShakeCamera(10, 10);
            }
        }
    }

    public void ShakeCamera(float intensity, float time, float frequency = 1)
    {
        //see if we can get 
        cmCamp.m_AmplitudeGain = intensity;
        cmCamp.m_FrequencyGain = frequency;
        shakeTimer = time;
    }

    public void ShakeCameraToggle(float intensity, float frequency, bool on)
    {
        if (!on)
        {
            toggle = true;
            cmCamp.m_AmplitudeGain = intensity;
            cmCamp.m_FrequencyGain = frequency;
        }
        else
        {
            toggle = false;
            shakeTimer = 0f;
            cmCamp.m_AmplitudeGain = 0f;
            cmCamp.m_FrequencyGain = baseFrequency;
        }
    }
}
