using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class SpriteFadeOut : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float smoothness;
    [SerializeField] private float fadeTime;
    
    private void Start()
    {
        TryGetComponent(out spriteRenderer);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float progress = 0;
        Color startColor = spriteRenderer.color;
        Color transparent = new Color(1, 1, 1, 0);
        var increment = smoothness / fadeTime;
        while (progress < 1)
        {
            spriteRenderer.color = Color.Lerp(startColor,Color.clear, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
