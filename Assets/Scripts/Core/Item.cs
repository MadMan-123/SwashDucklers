using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    private Inventory current;

    [SerializeField] public Vector3 offset;
    [SerializeField] public Quaternion pickupRotation;
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
        if (source.TryGetComponent(out Inventory inv) )       //try to see if player has an inv
        {
            current = inv;
            if (current.AddItem(this))                              //add this to inv
            {
                //disable the rigidbody and collider
                rb.isKinematic = true;                           //bunch of positioning stuff
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
            transform.SetParent(null);                                  //remove from parent (should make it drop?)
        }
    }
   
    public enum Type 
    {
        NoItem = -1,
        CannonBall, 
        Plank
        //other item types here
    }
}
