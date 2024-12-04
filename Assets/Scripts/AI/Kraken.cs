using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Kraken : MonoBehaviour
{

    [SerializeField] GameObject water;
    Renderer waterRenderer;
    [SerializeField] Light light;
    [SerializeField] GameObject eye;
    [SerializeField] GameObject eyeTarget;
    [SerializeField] int damping = 2;
    [SerializeField] PlayerManager pm;

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
        

    }

    private void OnEnable()
    {
        //When kraken appears start storm
        light.intensity = 0.5f;
        waterRenderer.material.SetColor("_BaseColor", StormbaseColor); //Light Color
        waterRenderer.material.SetColor("_1st_ShadeColor", StormShadeColor); //Shaded Color
        waterRenderer.material.SetColor("_2nd_ShadeColor", StormShade2Color); //Shaded Color
        waterRenderer.material.SetColor("_HighColor", StormHighlightColor); //Shaded Color
    }

    private void OnDisable()
    {
        //When kraken is dead disable storm
        light.intensity = 2;
        waterRenderer.material.SetColor("_BaseColor", WaterbaseColor); //Light Color
        waterRenderer.material.SetColor("_1st_ShadeColor", WaterShadeColor); //Shaded Color
        waterRenderer.material.SetColor("_2nd_ShadeColor", WaterShade2Color); //Shaded Color
        waterRenderer.material.SetColor("_HighColor", WaterHighlightColor); //Shaded Color
    }

    public void EyeFollow()
    {
        for (int i = 0;i < pm.players.Count;i++)
        {
            if(pm.players[i].TryGetComponent(out Health hp))
            {
                var temp = hp.GetHealth();
               if (temp > eyeTarget.GetComponent<Health>().GetHealth())
                {
                    eyeTarget = pm.players[i];
                }
            }
        }
        transform.LookAt(eyeTarget.transform.position);
        //var lookPos = eyeTarget.transform.position - transform.position;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * damping);
        EyeFollow();
    }
}
