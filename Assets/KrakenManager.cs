using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KrakenManager : MonoBehaviour
{
    [SerializeField] GameObject kraken;
    [SerializeField] GameObject tentacles;
    [SerializeField] float SpawnTime;
    [SerializeField] float UpTime;

    //Scene changes
    [SerializeField] GameObject water;
    Renderer waterRenderer;
    [SerializeField] float smoothness;
    [SerializeField] Light light;
    [SerializeField] float waterChangeDuration;
    [SerializeField] ParticleSystem rain;

    float timeBeforeNext;
    bool isActive = false;

    Color WaterbaseColor;
    Color WaterShadeColor;
    Color WaterShade2Color;
    Color WaterHighlightColor;
    Color StormbaseColor;
    Color StormShadeColor;
    Color StormShade2Color;
    Color StormHighlightColor;

    // Start is called before the first frame update
    void Start()
    {
        kraken.SetActive(false);
        StartCoroutine(KrakenSpawnTimer());

        //Default Colors
        WaterbaseColor = new Color32(0, 153, 243, 255);
        WaterShadeColor = new Color32(34, 62, 135, 255);
        WaterShade2Color = new Color32(0, 19, 34, 255);
        WaterHighlightColor = new Color32(74, 146, 233, 255);
        StormbaseColor = new Color32(51, 97, 123, 255);
        WaterShadeColor = new Color32(21, 60, 82, 255);
        StormShade2Color = new Color32(0, 19, 34, 255);
        StormHighlightColor = new Color32(74, 146, 233, 255);

        waterRenderer = water.transform.GetChild(1).GetComponent<Renderer>();
        waterRenderer.material.SetColor("_BaseColor", WaterbaseColor); //Light Color
        waterRenderer.material.SetColor("_1st_ShadeColor", WaterShadeColor); //Shaded Color
        waterRenderer.material.SetColor("_2nd_ShadeColor", WaterShade2Color); //Shaded Color
        waterRenderer.material.SetColor("_HighColor", WaterHighlightColor); //Shaded Color

        for (int i = 0; i < waterRenderer.material.shader.GetPropertyCount(); i++)
        {
            Debug.Log(waterRenderer.material.shader.GetPropertyName(i));
        }

    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeNext = Time.time + SpawnTime;
        if (Time.time > timeBeforeNext)
        {
           // Kraken.SetActive(true);
        }
        //Kraken.SetActive(false);

    }

    IEnumerator KrakenSpawnTimer()
    {
        yield return new WaitForSecondsRealtime(SpawnTime);
        kraken.SetActive(true);
        KrakenWater();
        yield return new WaitForSecondsRealtime(UpTime);
        kraken.SetActive(false);
        NormalWater();
        StartCoroutine(KrakenSpawnTimer());
    }

    public void KrakenWater()
    {
        CameraShake.Instance.ShakeCamera(5f, waterChangeDuration + 0.5f);
        StartCoroutine(WaterColour(true));
    }

    public void NormalWater()
    {
        CameraShake.Instance.ShakeCamera(5f, waterChangeDuration + 0.5f);
        StartCoroutine(WaterColour(false));
    }

    public IEnumerator WaterColour(bool spawn)
    {
        float progress = 0;
        float increment = smoothness / waterChangeDuration;
        var emission = rain.emission;
        if (spawn)
        {
            rain.gameObject.SetActive(true);
            while (progress < 1)
            {
                light.intensity = Mathf.Lerp(2, 0.5f, progress);
                emission.rateOverTime = 200 * progress;
                waterRenderer.material.SetColor("_BaseColor", Color.Lerp(WaterbaseColor, StormbaseColor, progress));
                waterRenderer.material.SetColor("_1st_ShadeColor", Color.Lerp(WaterShadeColor, StormShadeColor, progress));
                waterRenderer.material.SetColor("_2nd_ShadeColor", Color.Lerp(WaterShade2Color, StormShade2Color, progress));
                waterRenderer.material.SetColor("_HighColor", Color.Lerp(WaterHighlightColor, StormHighlightColor, progress));
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
            kraken.gameObject.SetActive(true);
            tentacles.gameObject.SetActive(true);
        }
        else
        {
            while (progress < 1)
            {
                light.intensity = Mathf.Lerp(0.5f, 2, progress);
                emission.rateOverTime = 200 * (1 - progress);
                waterRenderer.material.SetColor("_BaseColor", Color.Lerp(StormbaseColor, WaterbaseColor, progress));
                waterRenderer.material.SetColor("_1st_ShadeColor", Color.Lerp(StormShadeColor, WaterShadeColor, progress));
                waterRenderer.material.SetColor("_2nd_ShadeColor", Color.Lerp(StormShade2Color, WaterShade2Color, progress));
                waterRenderer.material.SetColor("_HighColor", Color.Lerp(StormHighlightColor, WaterHighlightColor, progress));
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
            light.intensity = 2;
            rain.gameObject.SetActive(false);
            kraken.gameObject.SetActive(false);
            tentacles.gameObject.SetActive(false);
        }
    }

}
