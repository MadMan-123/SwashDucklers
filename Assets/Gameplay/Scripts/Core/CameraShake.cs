using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cmCam;
    private float baseFrequency = 1;
    [SerializeField] private float shakeTimer;
    private bool toggle;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        cmCam = GetComponent<CinemachineVirtualCamera>();
        ShakeCamera(0, 0.1f);
    }

    // Update is called once per frame
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
                //Timer over!
                CinemachineBasicMultiChannelPerlin cmCamP =
                    cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cmCamP.m_AmplitudeGain = 0f;
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
        CinemachineBasicMultiChannelPerlin cmCamP = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmCamP.m_AmplitudeGain = intensity;
        cmCamP.m_FrequencyGain = frequency;
        shakeTimer = time;
    }

    public void ShakeCameraToggle(float intensity, float frequency, bool on)
    {
        CinemachineBasicMultiChannelPerlin cmCamP =
            cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (!on)
        {
            toggle = true;
            cmCamP.m_AmplitudeGain = intensity;
            cmCamP.m_FrequencyGain = frequency;
        }
        else
        {
            toggle = false;
            shakeTimer = 0f;
            cmCamP.m_AmplitudeGain = 0f;
            cmCamP.m_FrequencyGain = baseFrequency;
        }
    }
}
