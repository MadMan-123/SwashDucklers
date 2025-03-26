using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<GameObject> OnInteract;
    public Item.Type itemRequired = Item.Type.NoItem;

    public void Interact(Item item,GameObject source)
    {
        if(source.TryGetComponent(out AIBrain brain)) return;
        if(itemRequired != Item.Type.NoItem && (!item || item.type != itemRequired)) return;
        Assert.IsNotNull(OnInteract);
        OnInteract?.Invoke(source);
    }

    public void ToggleFlash(bool isOn)
    {
        if (TryGetComponent(out MeshRenderer rend))
        {
            rend.material.SetFloat("ShouldFlash", isOn ? 1 : 0);
        }
    }
}
