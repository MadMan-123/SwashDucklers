using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Weather : MonoBehaviour
{
    //Should keep this modular for other world events

    [SerializeField] GameObject water;
    [SerializeField] Renderer waterRenderer;
    [SerializeField] float smoothness;
    [SerializeField] Light sunlight;
    [SerializeField] float waterChangeDuration;
    [SerializeField] ParticleSystem rain;
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
        /*WaterbaseColor = new Color32(0, 153, 243, 255);
        WaterShadeColor = new Color32(34, 62, 135, 255);
        WaterShade2Color = new Color32(0, 19, 34, 255);
        WaterHighlightColor = new Color32(74, 146, 233, 255);*/

        KrakenbaseColor = new Color32(51, 97, 123, 255);
        KrakenShadeColor = new Color32(21, 60, 82, 255);
        KrakenShade2Color = new Color32(0, 19, 34, 255);
        KrakenHighlightColor = new Color32(74, 146, 233, 255);

        //waterRenderer = water.transform.GetChild(1).GetComponent<Renderer>();
        /*waterRenderer.material.SetColor("_BaseColor", WaterbaseColor); //Light Color
        waterRenderer.material.SetColor("_1st_ShadeColor", WaterShadeColor); //Shaded Color
        waterRenderer.material.SetColor("_2nd_ShadeColor", WaterShade2Color); //Shaded Color
        waterRenderer.material.SetColor("_HighColor", WaterHighlightColor); //Shaded Color
        for (int i = 0; i < waterRenderer.material.shader.GetPropertyCount(); i++)
        {
            Debug.Log(waterRenderer.material.shader.GetPropertyName(i));
        }*/

        rain.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void KrakenSpawn()
    {
        weatherState = WeatherState.KRAKEN;
        //StartCoroutine(ChangeWaterColor());

    }

    public void KrakenDeSpawn()
    {
        weatherState = WeatherState.NORMAL;
        //StartCoroutine(ChangeWaterColor());

    }

    public void WorldEvent()
    {
        //switch (weatherState)
        //{
        //    case WeatherState.NORMAL:
        //        //StartCoroutine(KrakenSpawn());
        //        break;

        //}
    }

    IEnumerator ChangeWaterColor()
    {
        float progress = 0;
        float increment = smoothness / waterChangeDuration;
        var emission = rain.emission;

        Color currentBaseColor = waterRenderer.material.GetColor("_BaseColor");
        Color currentShadeColor = waterRenderer.material.GetColor("_1st_ShadeColor");
        Color currentShade2Color = waterRenderer.material.GetColor("_2nd_ShadeColor");
        Color currentHighlightColor = waterRenderer.material.GetColor("_HighColor");

        switch (weatherState)
        {
            case WeatherState.NORMAL:
                //CameraShake.Instance.ShakeCamera(1f, waterChangeDuration + 0.5f);
                while (progress < 1)
                {
                    sunlight.intensity = Mathf.Lerp(1.5f, 2, progress);
                    emission.rateOverTime = 200 * (1 - progress);
                    waterRenderer.material.SetColor("_BaseColor", Color.Lerp(currentBaseColor, WaterbaseColor, progress));
                    waterRenderer.material.SetColor("_1st_ShadeColor", Color.Lerp(currentShadeColor, WaterShadeColor, progress));
                    waterRenderer.material.SetColor("_2nd_ShadeColor", Color.Lerp(currentShade2Color, WaterShade2Color, progress));
                    waterRenderer.material.SetColor("_HighColor", Color.Lerp(currentHighlightColor, WaterHighlightColor, progress));
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
                    waterRenderer.material.SetColor("_BaseColor", Color.Lerp(currentBaseColor, KrakenbaseColor, progress));
                    waterRenderer.material.SetColor("_1st_ShadeColor", Color.Lerp(currentShadeColor, KrakenShadeColor, progress));
                    waterRenderer.material.SetColor("_2nd_ShadeColor", Color.Lerp(currentShade2Color, KrakenShade2Color, progress));
                    waterRenderer.material.SetColor("_HighColor", Color.Lerp(currentHighlightColor, KrakenHighlightColor, progress));
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
