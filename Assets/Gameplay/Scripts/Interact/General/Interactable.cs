using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private static readonly int ShouldFlash = Shader.PropertyToID("_ShouldFlash");
    public UnityEvent<GameObject> OnInteract;
    public Item.Type itemRequired = Item.Type.NoItem;

    public List<Material> outlineMaterials = new List<Material>();

    private void Start()
    {
        TryGetOutlineMaterials(); 
    }

    protected void TryGetOutlineMaterials()
    {
        //get all renderers in children and children of children, if they have the material with the "outline shader" as its name
        var rends = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < rends.Length; i++)
        {
            var rend = rends[i];
            //try get all materials
            var mats = rend.materials;
            
            //get the materials that have the outline shader
            var outlineMats = mats.Where(mat => mat.name.Contains("white outline")).ToList();
            
            //add them to the list
            outlineMaterials.AddRange(outlineMats);
            
        }
    }

    public void Interact(Item item,GameObject source)
    {
        if(source.TryGetComponent(out AIBrain brain)) return;
        if(itemRequired != Item.Type.NoItem && (!item || item.type != itemRequired)) return;
        Assert.IsNotNull(OnInteract);
        OnInteract?.Invoke(source);
    }
    
    public void ToggleFlash(bool isOn)
    {
        for (var i = 0; i < outlineMaterials.Count; i++)
        {
            //check if the material is valid
            if (!outlineMaterials[i])
            {
                Debug.LogError("Material is null");
                //remove the material from the list
                outlineMaterials.RemoveAt(i);
                continue;
            }
            var mat = outlineMaterials[i];
            mat.SetInt(ShouldFlash, isOn ? 1 : 0);

            //try and read the value
            var val = mat.GetInt(ShouldFlash);
            if (val != (isOn ? 1 : 0))
            {
                Debug.LogError("Failed to set the outline value");
            }
        }
    }
}
