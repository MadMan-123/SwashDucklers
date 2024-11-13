using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    private Inventory current;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion pickupRotation;
    [SerializeField] public Type type;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!col) col = GetComponent<Collider>(); 
    }

    //type of item
    public void PickUp(GameObject source)
    {
        if (source.TryGetComponent(out Inventory inv) )
        {
            current = inv;
            if (current.AddItem(this))
            {
                //disable the rigidbody and collider
                rb.isKinematic = true;
                col.enabled = false;
                
                //set the transforms
                transform.SetParent(current.itemHolder,true);

                transform.localPosition = offset;
                transform.localRotation = pickupRotation;
            }
            
        }
    }
    
    public void DropItem(GameObject source)
    {
        if (source.TryGetComponent(out Inventory inv) && inv == current)
        {
            current = null;
            //set the transforms
            transform.SetParent(null);
        }
    }
   
    public enum Type 
    {
        CannonBall, Plank
        //other item types here
    }
}
