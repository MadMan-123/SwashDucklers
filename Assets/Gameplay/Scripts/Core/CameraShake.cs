using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cmCam;
    [SerializeField] private float shakeTimer;
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
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                //Timer over!
                CinemachineBasicMultiChannelPerlin cmCamP = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cmCamP.m_AmplitudeGain = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) { ShakeCamera(10, 10); }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmCamP = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmCamP.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}
