using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    [SerializeField] GameObject water;
    [SerializeField] Renderer waterRenderer;
    [SerializeField] float smoothness;
    [SerializeField] Light light;
    [SerializeField] float waterChangeDuration;
    [SerializeField] ParticleSystem rain;

    //water and light and rain and stuff
    Color WaterbaseColor;
    Color WaterShadeColor;
    Color WaterShade2Color;
    Color WaterHighlightColor;


    // Start is called before the first frame update
    void Start()
    {
        WaterbaseColor = new Color32(0, 153, 243, 255);
        WaterShadeColor = new Color32(34, 62, 135, 255);
        WaterShade2Color = new Color32(0, 19, 34, 255);
        WaterHighlightColor = new Color32(74, 146, 233, 255);

        //waterRenderer = water.transform.GetChild(1).GetComponent<Renderer>();
        waterRenderer.material.SetColor("_BaseColor", WaterbaseColor); //Light Color
        waterRenderer.material.SetColor("_1st_ShadeColor", WaterShadeColor); //Shaded Color
        waterRenderer.material.SetColor("_2nd_ShadeColor", WaterShade2Color); //Shaded Color
        waterRenderer.material.SetColor("_HighColor", WaterHighlightColor); //Shaded Color
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
