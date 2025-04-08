using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAndBreakSign : MonoBehaviour
{
    public Inventory inventory; // Reference to the inventory system
    public float destroyDelay = 0.2f; // Delay before destroying the object
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    public void DropItemAndDestroy()
    {
        if (inventory != null)
        {
            inventory.DropItem(Vector3.zero, true);
        }

        StartCoroutine(BreakAfterDelay());
    }

    IEnumerator BreakAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        animator.Play("Crab Break");
       
    }
}