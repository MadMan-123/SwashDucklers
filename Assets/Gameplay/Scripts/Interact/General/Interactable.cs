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
        print("1");
        if(itemRequired != Item.Type.NoItem && !item) return;
        print("2");
        Assert.IsNotNull(OnInteract);
        print("3");
        OnInteract?.Invoke(source);
    }
}
