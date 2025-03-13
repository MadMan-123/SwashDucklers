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

    void Start()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        rain.gameObject.SetActive(false);
    }

    public void KrakenSpawn()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        weatherState = WeatherState.KRAKEN;
        StartCoroutine(ChangeWeather());

    }

    public void KrakenDeSpawn()
    {
        waterRenderer = water.transform.GetChild(1).GetComponent<MeshRenderer>();
        weatherState = WeatherState.NORMAL;
        StartCoroutine(ChangeWeather());

    }

    IEnumerator ChangeWeather()
    {
        float progress = 0;
        float increment = smoothness / waterChangeDuration;
        var emission = rain.emission;
        //Material currentMat = waterRenderer.material;
        switch (weatherState)
        {
            case WeatherState.NORMAL:
                while (progress < 1)
                {
                    sunlight.intensity = Mathf.Lerp(1.5f, 2, progress);
                    emission.rateOverTime = 200 * (1 - progress);
                    //waterRenderer.material.Lerp(currentMat, waterMaterial, progress);
                    progress += increment;
                    yield return new WaitForSeconds(smoothness);
                }
                //GetComponent<Light>().intensity = 2;
                rain.gameObject.SetActive(false);
                break;


            case WeatherState.KRAKEN:
                rain.gameObject.SetActive(true);
                while (progress < 1)
                {
                    sunlight.intensity = Mathf.Lerp(2, 0.5f, progress);
                    emission.rateOverTime = 200 * progress;
                    //waterRenderer.material.Lerp(currentMat, krakenMaterial, progress);
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
