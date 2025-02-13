using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Weather : MonoBehaviour
{
    //Should keep this modular for other world events

    [SerializeField] GameObject water;
    [SerializeField] MeshRenderer waterRenderer;
    [SerializeField] float smoothness;
    [SerializeField] Light sunlight;
    [SerializeField] float waterChangeDuration;
    [SerializeField] ParticleSystem rain;
    [SerializeField] Material waterMaterial;
    [SerializeField] Material krakenMaterial;
    WeatherState weatherState;


    //water and light and rain and stuff
    Color WaterbaseColor;
    Color WaterShadeColor;
    Color WaterShade2Color;
    Color WaterHighlightColor;

    Color KrakenbaseColor;
    Color KrakenShadeColor;
    Color KrakenShade2Color;
    Color KrakenHighlightColor;

    // Start is called before the first frame update
    void Start()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        rain.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void KrakenSpawn()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        weatherState = WeatherState.KRAKEN;
        StartCoroutine(ChangeWaterColor());

    }

    public void KrakenDeSpawn()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        weatherState = WeatherState.NORMAL;
        StartCoroutine(ChangeWaterColor());

    }

    IEnumerator ChangeWaterColor()
    {
        float progress = 0;
        float increment = smoothness / waterChangeDuration;
        var emission = rain.emission;
        Material currentMat = waterRenderer.material;

        switch (weatherState)
        {
            case WeatherState.NORMAL:
                //CameraShake.Instance.ShakeCamera(1f, waterChangeDuration + 0.5f);
                while (progress < 1)
                {
                    sunlight.intensity = Mathf.Lerp(1.5f, 2, progress);
                    emission.rateOverTime = 200 * (1 - progress);
                    waterRenderer.material.Lerp(currentMat, waterMaterial, progress);
                    progress += increment;
                    yield return new WaitForSeconds(smoothness);
                }
                GetComponent<Light>().intensity = 2;
                rain.gameObject.SetActive(false);
                break;


            case WeatherState.KRAKEN:
                //CameraShake.Instance.ShakeCamera(1.5f, waterChangeDuration + 0.5f);
                rain.gameObject.SetActive(true);
                while (progress < 1)
                {
                    sunlight.intensity = Mathf.Lerp(2, 0.5f, progress);
                    emission.rateOverTime = 200 * progress;
                    //waterRenderer.material.SetColor("_BaseColor", Color.Lerp(currentBaseColor, KrakenbaseColor, progress));
                    //waterRenderer.material.SetColor("_1st_ShadeColor", Color.Lerp(currentShadeColor, KrakenShadeColor, progress));
                    //waterRenderer.material.SetColor("_2nd_ShadeColor", Color.Lerp(currentShade2Color, KrakenShade2Color, progress));
                    //waterRenderer.material.SetColor("_HighColor", Color.Lerp(currentHighlightColor, KrakenHighlightColor, progress));
                    waterRenderer.material.Lerp(currentMat, krakenMaterial, progress);
                    progress += increment;
                    yield return new WaitForSeconds(smoothness);
                }
                break;
        }
    }

    enum WeatherState
    {
        NORMAL,
        KRAKEN,
    }
}
