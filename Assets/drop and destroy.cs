using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAndDestroy : MonoBehaviour
{
    public Inventory inventory; // Reference to the inventory system
    public float destroyDelay = 0.2f; // Delay before destroying the object

    public void DropItemAndDestroy()
    {
        if (inventory != null)
        {
            inventory.DropItem(Vector3.zero, true);
        }

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}