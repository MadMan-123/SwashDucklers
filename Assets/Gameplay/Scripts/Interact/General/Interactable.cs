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
        if(itemRequired != Item.Type.NoItem && !item) return;
        Assert.IsNotNull(OnInteract);
        OnInteract?.Invoke(source);
    }
}
